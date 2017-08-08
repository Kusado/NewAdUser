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
      this.Host = NS.Host.GetHostEntry(ServerName.Replace(" ", ""));
      this.InstanceName = InstanceName;
      this.ServiceName = this.InstanceName == "DEFAULT" ? "MSSQLSERVER" : "MSSQL$" + this.InstanceName;
    }
  }
}