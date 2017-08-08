using System.Collections.Generic;

namespace NewAdUser {

  public class Domain {
    public AdDomain AD { get; set; }
    public MailDomain Mail { get; set; }
    public string AdFqdn => this.DictAdFQDN[this.AD];
    public string MailFqdn => this.DictMailFQDN[this.Mail];
    public string MailboxOU => this.LinkedMailboxPath[this.AD];
    public string LoginOU => this.DictLoginOUs[this.AD];
    public string DC => this.DomainController[this.AD];
    private Dictionary<AdDomain, string> DomainController { get; }
    private Dictionary<AdDomain, string> LinkedMailboxPath { get; }
    private Dictionary<MailDomain, string> DictMailFQDN { get; }
    private Dictionary<AdDomain, string> DictAdFQDN { get; }
    private Dictionary<AdDomain, string> DictLoginOUs { get; }

    public Domain() {
      this.DictMailFQDN = new Dictionary<MailDomain, string> {
        {MailDomain.Formulabi, "formulabi.ru"},
        {MailDomain.Radar, "radarias.ru"},
        //{MailDomain.ExHelp, "ex-help.ru"},
        //{MailDomain.SherpSoft, "sherpsoft.ru"}
      };
      this.DictAdFQDN = new Dictionary<AdDomain, string> {
        {AdDomain.Formulabi, "formulabi.local"},
        {AdDomain.Radarias, "radarias.local"},
      };
      this.DomainController = new Dictionary<AdDomain, string> {
        {AdDomain.Formulabi,  "SRV-DC03.FormulaBI.local"},
        {AdDomain.Radarias, "dc01.radarias.local"},
      };
      this.LinkedMailboxPath = new Dictionary<AdDomain, string> {
        {AdDomain.Formulabi, "fbi.local/Mailboxes/Linked Mailboxes/Formula" },
        {AdDomain.Radarias, "fbi.local/Mailboxes/Linked Mailboxes/Radar" },
      };
      this.DictLoginOUs = new Dictionary<AdDomain, string> {
        {AdDomain.Formulabi, "OU=AutoCreated,OU=Пользователи,OU=FormulaBI,DC=FormulaBI,DC=local" },
        {AdDomain.Radarias, "OU=AutoCreated,OU=Users,OU=radarias,DC=radarias,DC=local" },
      };
    }

    public enum AdDomain {
      Formulabi = 0,
      Radarias
    }

    public enum MailDomain {
      Formulabi = 0,
      Radar,
      //ExHelp,
      //SherpSoft
    }
  }
}