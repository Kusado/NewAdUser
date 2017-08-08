using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceProcess;

namespace NewAdUser {

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
      this.Host = NS.Host.GetHost(ServerName.Replace(" ", ""));
      this.InstanceName = InstanceName;
      this.ServiceName = this.InstanceName == "DEFAULT" ? "MSSQLSERVER" : "MSSQL$" + this.InstanceName;
    }

    public static List<SqlInstance> GetSqlServersFromNetwork(bool debug = false) {
      List<SqlInstance> sqlInstances = new List<SqlInstance>();
      string output;
      using (Process proc = new Process()) {
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.FileName = "sqlcmd.exe";
        proc.StartInfo.Arguments = "-L";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        if (!debug) proc.Start();
        output = debug ? $"\r\nServers:\r\n URAN\\LG\r\n MARS\\RFM\r\n" : proc.StandardOutput.ReadToEnd();
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
  }
}