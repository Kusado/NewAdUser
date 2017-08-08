using System;

namespace NewAdUser {
  public class AdUser {
    public string FullName;
    public string DisplayName;
    public string Name;
    public string SurName;
    public string SecondName;
    public string login;
    public Domains.AdDomain domain;
    public Domains.MailDomain mailDomain;
    public string Email => this.login + Domains.MailFQDN[this.mailDomain];
    public string UPN => this.login + Domains.AdFQDN[this.domain];
    public string password;

    public AdUser() {
    }

    ///  <summary>
    /// 
    ///  </summary>
    ///  <param name="Login">����� ������������</param>
    ///  <param name="Name">��� ������������</param>
    ///  <param name="SecondName">�������� ������������</param>
    ///  <param name="SurName">������� ������������</param>
    /// <param name="domain">����� Active Directory, ��� �������� ������������.</param>
    /// <param name="mailDomain">�������� �����, � ������� ����� ���������� ����� ������������.</param>
    /// <param name="password">������ ������������</param>
    public AdUser(string Login, string Name, string SecondName, string SurName, Domains.AdDomain domain, Domains.MailDomain mailDomain, string password = "Zasqw12") {
      this.login = Login;
      this.FullName = $"{SurName} {Name} {SecondName}";
      this.Name = Name;
      this.SecondName = SecondName;
      this.SurName = SurName;
      this.DisplayName = this.FullName;
      this.password = password;
      this.domain = domain;
      this.mailDomain = mailDomain;
    }
  }
}