using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewAdUser {
  public static class Domains {
    public enum AdDomain {
      Formulabi = 0,
      Radarias
    }

    public enum MailDomain {
      Formulabi = 0,
      Radar,
      ExHelp,
      SherpSoft
    }

    public static Dictionary<MailDomain, string> MailFQDN;
    public static Dictionary<AdDomain, string> AdFQDN;
    static Domains() {
      MailFQDN = new Dictionary<MailDomain, string> {
        {MailDomain.Formulabi, "@formulabi.ru"},
        {MailDomain.Radar, "@radarias.ru"},
        {MailDomain.ExHelp, "@ex-help.ru"},
        {MailDomain.SherpSoft, "@sherpsoft.ru"}
      };
      AdFQDN = new Dictionary<AdDomain, string> {
        {AdDomain.Formulabi, "@formulabi.local"},
        {AdDomain.Radarias, "@radarias.local"},
      };
    }
  }
}
