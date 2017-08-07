using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Net.NetworkInformation;
using System.ServiceProcess;

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
          if (PingHost(srv.Host.FQDN, 500, out IPStatus status)) {
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
      public string InstanceFullName { get { return ToString(); } }
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

    //public enum AdDomain {
    //  Formulabi = 0,
    //  Radarias
    //}

    //public enum MailDomain {
    //  Formulabi = 0,
    //  Radar,
    //  ExHelp
    //}

    public static Dictionary<Domains.MailDomain, string> MailDomains = new Dictionary<Domains.MailDomain, string>();

    public static bool addADUser(AdUser user, PSCredential credential) {
      switch (user.domain) {
        case Domains.ActiveDirectory.Formulabi:
          return AddFbiUser(user, credential);

        case Domains.ActiveDirectory Radarias:
          return AddRadarUser(user, credential);

        default:
          return false;
      }
    }

    private static bool AddFbiUser(AdUser user, PSCredential credential) {
      return false;
    }

    private static bool AddRadarUser(AdUser user, PSCredential credential) {
      //posh.AddUser("dc01.radarias.local", credential, "Full Name Test", "OU=test,OU=Users,OU=radarias,DC=radarias,DC=local", "TestUser01", "ОтображаемоеИмя", "Имя", "Фамилия", "testuser@radarias.local");
      posh.AddUser(user, "dc01.radarias.local", credential, "OU=test,OU=Users,OU=radarias,DC=radarias,DC=local");
      return false;
    }
  }
}