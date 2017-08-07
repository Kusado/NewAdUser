using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAdUser {
  public static class Domains {
    public enum ActiveDirectory {
      Formulabi = 0,
      Radarias
    }

    public enum MailDomain {
      Formulabi = 0,
      Radar,
      ExHelp
    }

    public static KeyValuePair<MailDomain, string> MXDomain;
    static Domains() {
      MXDomain = new KeyValuePair<MailDomain, string>();
      MXDomain.
    }
  }
}
