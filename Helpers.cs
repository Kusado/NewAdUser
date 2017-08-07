using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace NewAdUser {

   public static partial class Helpers {
      public static List<SqlInstance> GetSqlServersFromNetwork(bool test = false) {
         List<SqlInstance> sqlInstances = new List<SqlInstance>();
         string output;
         using (Process proc = new Process()) {
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "sqlcmd.exe";
            proc.StartInfo.Arguments = "-L";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            if (!test) proc.Start();
            output = test ? $"\r\nServers:\r\n URAN\\LG\r\n MARS\\RFM\r\n" : proc.StandardOutput.ReadToEnd();
         }
         string[] serversStrings = output.Replace("Servers:", "").Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
         foreach (string s in serversStrings) {
            string[] server = s.Split(new string[] { "\\" }, StringSplitOptions.None);
            SqlInstance temp = (server.Length > 1 ? new SqlInstance(server[0], server[1]) : new SqlInstance(server[0]));
            if (temp.Host != null) sqlInstances.Add(temp);
         }

         GetSqlStatus(ref sqlInstances);
         List<SqlInstance> result = sqlInstances.Where(x => x.ServiceStatus == ServiceControllerStatus.Running).ToList();

         return result;
      }

      private static void GetSqlStatus(ref List<SqlInstance> servers) {
         ServiceController sc;
         foreach (IGrouping<string, SqlInstance> server in servers.GroupBy(x => x.Host.FQDN)) {
            SqlInstance srv = server.First();
            if (srv.Host.Domain?.ToLower() == "formulabi.local") {
               if (Helpers.PingHost(srv.Host.FQDN, 500, out IPStatus status)) {
                  sc = new ServiceController();
                  sc.MachineName = srv.Host.FQDN;
                  foreach (SqlInstance service in server) {
                     service.ServerStatus = status;
                     sc.ServiceName = service.ServiceName;
                     try {
                        service.ServiceStatus = sc.Status;
                     }
                     catch (Exception e) {
                        service.Info = e.Message;
                     }
                  }
               }
               else {
                  foreach (SqlInstance service in server) {
                     service.ServerStatus = status;
                  }
               }
            }
         }
      }

      public class SqlInstance {
         public NS.Host Host { get; set; }
         public IPStatus ServerStatus { get; set; }
         public string InstanceName { get; set; }
         public string InstanceFullName { get { return this.ToString(); } }
         public string ServiceName { get; set; }
         public int ServerMemoryRunning { get; set; }
         public int ServerMemoryMax { get; set; }
         public int DatabasesSize { get; set; }
         public string Info { get; set; }
         public string Description { get; set; }
         public ServiceControllerStatus ServiceStatus { get; set; }

         public SqlInstance() {
         }

         public override string ToString() {
            return this.Host.Name + "\\" + this.InstanceName;
         }

         public SqlInstance(string ServerName, string InstanceName = "DEFAULT") {
            this.Host = NS.Host.GetHostEntry(ServerName.Replace(" ", ""));
            this.InstanceName = InstanceName;
            this.ServiceName = this.InstanceName == "DEFAULT" ? "MSSQLSERVER" : "MSSQL$" + this.InstanceName;
         }
      }

      public static DataTable GetSqlDataTable(string q, SqlConnection conn) {
         DataTable dt = new DataTable();
         SqlCommand command;
         if (conn.State != ConnectionState.Open) conn.Open();
         command = new SqlCommand(q, conn);
         SqlDataReader r = command.ExecuteReader();
         dt.Load(r);
         return dt;
      }

      public static DataTable GetSqlDataTable(string q, string connectionString) {
         DataTable dt = new DataTable();
         SqlCommand command;
         using (SqlConnection conn = new SqlConnection(connectionString)) {
            conn.Open();
            command = new SqlCommand(q, conn);
            SqlDataReader r = command.ExecuteReader();
            dt.Load(r);
         }
         return dt;
      }

      public class NS {

         [Serializable]
         public class Host {
            public string FQDN { get; set; }

            [NonSerialized]
            public string Name;

            [NonSerialized]
            public string Domain;

            [NonSerialized]
            private IPAddress[] addresses;

            public override string ToString() {
               return this.FQDN;
            }

            public static Host GetHostEntry(string NameOrIp) {
               IPHostEntry ipHostEntry;
               try {
                  ipHostEntry = Dns.GetHostEntry(NameOrIp);
               }
               catch (System.Net.Sockets.SocketException socketException) {
                  if (socketException.NativeErrorCode == 11001) {
                     return null;
                     //return new Host(NameOrIp);
                  }
                  else {
                     throw;
                  }
               }
               catch (Exception e) {
                  throw e;
               }
               if (!ipHostEntry.HostName.Contains(".")) return null;
               return new Host(ipHostEntry);
            }

            private Host(string FQDN) {
               Host tmp = GetHostEntry(FQDN);
               //foreach (PropertyInfo property in tmp.GetType().GetProperties(BindingFlags.DeclaredOnly)) {
               //  this.GetType().GetProperties().First(x => x.Name == property.Name)=property;
               //}
               this.FQDN = tmp.FQDN;
               this.Domain = tmp.Domain;
               this.Name = tmp.Name;
               this.addresses = tmp.addresses;
            }

            private Host(IPHostEntry ipHostEntry) {
               this.FQDN = ipHostEntry.HostName;
               this.addresses = ipHostEntry.AddressList;
               this.Name = FQDN.Substring(0, FQDN.IndexOf("."));
               this.Domain = this.FQDN.Remove(0, this.Name.Length + 1);
            }
         }
      }

      public static bool PingHost(string nameOrAddress, int timeout, out IPStatus status) {
         status = IPStatus.Unknown;
         bool pingable = false;
         Ping pinger = new Ping();
         try {
            PingReply reply = pinger.Send(nameOrAddress, timeout);
            status = reply.Status;
            pingable = status == IPStatus.Success;
         }
         catch (PingException) {
            // Discard PingExceptions and return false;
         }
         return pingable;
      }

      public enum AdDomain {
         Formulabi = 0,
         Radarias
      }

      public enum MailDomain {
         Formulabi=0,
         Radar,
         ExHelp
      }

      public static Dictionary<MailDomain, string> MailDomains=new Dictionary<MailDomain, string>();

      public static bool addADUser(AdUser user, AdDomain domain, PSCredential credential) {
         switch (domain) {
            case AdDomain.Formulabi:
               return AddFbiUser(user, domain, credential);

            case AdDomain.Radarias:
               return AddRadarUser(user, domain, credential);

            default:
               return false;
         }
      }

      private static bool AddFbiUser(AdUser user, AdDomain domain, PSCredential credential) {
         return false;
      }

      private static bool AddRadarUser(AdUser user, AdDomain domain, PSCredential credential) {
         //posh.AddUser("dc01.radarias.local", credential, "Full Name Test", "OU=test,OU=Users,OU=radarias,DC=radarias,DC=local", "TestUser01", "ОтображаемоеИмя", "Имя", "Фамилия", "testuser@radarias.local");
         posh.AddUser(user, "dc01.radarias.local", credential, "OU=test,OU=Users,OU=radarias,DC=radarias,DC=local");
         return false;
      }
   }
   public class AdUser {
      //$"New-ADUser -Name '{FullName}'" +
      //$"-AccountPassword $sec_pass" +
      //$"-DisplayName '{DisplayName}'" +
      //$"-GivenName '{Name}'" +
      //$"-Surname '{surName}'" +
      //$"-Path '{path}'" +
      //$"-SamAccountName '{login}'" +
      //$"-UserPrincipalName '{upn}'" +
      //$"-enabled $true;";
      public string FullName;
      public string DisplayName;
      public string Name;
      public string SurName;
      public string SecondName;
      public string login;
      public Helpers.AdDomain domain;

      public string UPN {
         get {
            string domainName;
            switch (this.domain) {
               case Helpers.AdDomain.Formulabi:
                  domainName = "@formulabi.local";
                  break;
               case Helpers.AdDomain.Radarias:
                  domainName = "@radarias.local";
                  break;
               default:
                  throw new ArgumentOutOfRangeException();
            }
            return this.login + "@" + this.domain.ToString() + ".local";
         }
      }

      public string password;
      public AdUser() {
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Login">Логин пользователя</param>
      /// <param name="Name">Имя пользователя</param>
      /// <param name="SecondName">Отчество пользователя</param>
      /// <param name="SurName">Фамилия пользователя</param>
      /// <param name="password">Пароль пользователя</param>
      public AdUser(string Login, string Name, string SecondName, string SurName, Helpers.AdDomain domain, string password = "Zasqw12") {
         this.login = Login;
         this.FullName = $"{SurName} {Name} {SecondName}";
         this.DisplayName = this.FullName;
         this.password = password;
         this.domain = domain;
      }
   }
}