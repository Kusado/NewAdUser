using System;
using System.Net;

namespace NewAdUser {
  public static class NS {

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
        this.Name = this.FQDN.Substring(0, this.FQDN.IndexOf("."));
        this.Domain = this.FQDN.Remove(0, this.Name.Length + 1);
      }
    }
  }
}