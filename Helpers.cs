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

  public static class Helpers {

    public enum TransliterationType {
      Gost,
      ISO
    }

    public static class Transliteration {
      private static Dictionary<string, string> gost = new Dictionary<string, string>(); //ГОСТ 16876-71
      private static Dictionary<string, string> iso = new Dictionary<string, string>(); //ISO 9-95

      public static string Front(string text) {
        return Front(text, TransliterationType.ISO);
      }

      public static string Front(string text, TransliterationType type) {
        string output = text;

        output = Regex.Replace(output, @"\s|\.|\(", " ");
        output = Regex.Replace(output, @"\s+", " ");
        output = Regex.Replace(output, @"[^\s\w\d-]", "");
        output = output.Trim();

        Dictionary<string, string> tdict = GetDictionaryByType(type);

        foreach (KeyValuePair<string, string> key in tdict) {
          output = output.Replace(key.Key, key.Value);
        }
        return output;
      }

      public static string Back(string text) {
        return Back(text, TransliterationType.ISO);
      }

      public static string Back(string text, TransliterationType type) {
        string output = text;
        Dictionary<string, string> tdict = GetDictionaryByType(type);

        foreach (KeyValuePair<string, string> key in tdict) {
          output = output.Replace(key.Value, key.Key);
        }
        return output;
      }

      private static Dictionary<string, string> GetDictionaryByType(TransliterationType type) {
        Dictionary<string, string> tdict = iso;
        if (type == TransliterationType.Gost) tdict = gost;
        return tdict;
      }

      static Transliteration() {
        gost.Add("Є", "EH");
        gost.Add("І", "I");
        gost.Add("і", "i");
        gost.Add("№", "#");
        gost.Add("є", "eh");
        gost.Add("А", "A");
        gost.Add("Б", "B");
        gost.Add("В", "V");
        gost.Add("Г", "G");
        gost.Add("Д", "D");
        gost.Add("Е", "E");
        gost.Add("Ё", "JO");
        gost.Add("Ж", "ZH");
        gost.Add("З", "Z");
        gost.Add("И", "I");
        gost.Add("Й", "JJ");
        gost.Add("К", "K");
        gost.Add("Л", "L");
        gost.Add("М", "M");
        gost.Add("Н", "N");
        gost.Add("О", "O");
        gost.Add("П", "P");
        gost.Add("Р", "R");
        gost.Add("С", "S");
        gost.Add("Т", "T");
        gost.Add("У", "U");
        gost.Add("Ф", "F");
        gost.Add("Х", "KH");
        gost.Add("Ц", "C");
        gost.Add("Ч", "CH");
        gost.Add("Ш", "SH");
        gost.Add("Щ", "SHH");
        gost.Add("Ъ", "'");
        gost.Add("Ы", "Y");
        gost.Add("Ь", "");
        gost.Add("Э", "EH");
        gost.Add("Ю", "YU");
        gost.Add("Я", "YA");
        gost.Add("а", "a");
        gost.Add("б", "b");
        gost.Add("в", "v");
        gost.Add("г", "g");
        gost.Add("д", "d");
        gost.Add("е", "e");
        gost.Add("ё", "jo");
        gost.Add("ж", "zh");
        gost.Add("з", "z");
        gost.Add("и", "i");
        gost.Add("й", "jj");
        gost.Add("к", "k");
        gost.Add("л", "l");
        gost.Add("м", "m");
        gost.Add("н", "n");
        gost.Add("о", "o");
        gost.Add("п", "p");
        gost.Add("р", "r");
        gost.Add("с", "s");
        gost.Add("т", "t");
        gost.Add("у", "u");

        gost.Add("ф", "f");
        gost.Add("х", "kh");
        gost.Add("ц", "c");
        gost.Add("ч", "ch");
        gost.Add("ш", "sh");
        gost.Add("щ", "shh");
        gost.Add("ъ", "");
        gost.Add("ы", "y");
        gost.Add("ь", "");
        gost.Add("э", "eh");
        gost.Add("ю", "yu");
        gost.Add("я", "ya");
        gost.Add("«", "");
        gost.Add("»", "");
        gost.Add("—", "-");
        gost.Add(" ", "-");

        iso.Add("Є", "YE");
        iso.Add("І", "I");
        iso.Add("Ѓ", "G");
        iso.Add("і", "i");
        iso.Add("№", "#");
        iso.Add("є", "ye");
        iso.Add("ѓ", "g");
        iso.Add("А", "A");
        iso.Add("Б", "B");
        iso.Add("В", "V");
        iso.Add("Г", "G");
        iso.Add("Д", "D");
        iso.Add("Е", "E");
        iso.Add("Ё", "YO");
        iso.Add("Ж", "ZH");
        iso.Add("З", "Z");
        iso.Add("И", "I");
        iso.Add("Й", "J");
        iso.Add("К", "K");
        iso.Add("Л", "L");
        iso.Add("М", "M");
        iso.Add("Н", "N");
        iso.Add("О", "O");
        iso.Add("П", "P");
        iso.Add("Р", "R");
        iso.Add("С", "S");
        iso.Add("Т", "T");
        iso.Add("У", "U");
        iso.Add("Ф", "F");
        iso.Add("Х", "X");
        iso.Add("Ц", "C");
        iso.Add("Ч", "CH");
        iso.Add("Ш", "SH");
        iso.Add("Щ", "SHH");
        iso.Add("Ъ", "'");
        iso.Add("Ы", "Y");
        iso.Add("Ь", "");
        iso.Add("Э", "E");
        iso.Add("Ю", "YU");
        iso.Add("Я", "YA");
        iso.Add("а", "a");
        iso.Add("б", "b");
        iso.Add("в", "v");
        iso.Add("г", "g");
        iso.Add("д", "d");
        iso.Add("е", "e");
        iso.Add("ё", "yo");
        iso.Add("ж", "zh");
        iso.Add("з", "z");
        iso.Add("и", "i");
        iso.Add("й", "j");
        iso.Add("к", "k");
        iso.Add("л", "l");
        iso.Add("м", "m");
        iso.Add("н", "n");
        iso.Add("о", "o");
        iso.Add("п", "p");
        iso.Add("р", "r");
        iso.Add("с", "s");
        iso.Add("т", "t");
        iso.Add("у", "u");
        iso.Add("ф", "f");
        iso.Add("х", "x");
        iso.Add("ц", "c");
        iso.Add("ч", "ch");
        iso.Add("ш", "sh");
        iso.Add("щ", "shh");
        iso.Add("ъ", "");
        iso.Add("ы", "y");
        iso.Add("ь", "");
        iso.Add("э", "e");
        iso.Add("ю", "yu");
        iso.Add("я", "ya");
        iso.Add("«", "");
        iso.Add("»", "");
        iso.Add("—", "-");
        iso.Add(" ", "-");
      }
    }

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
      Formulabi,
      Radarias
    }

    public enum MailDomain {
      Formulabi,
      Radar,
      ExHelp
    }

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
        return this.login + "@" + this.domain.ToString();
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
    }
  }
}