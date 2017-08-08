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
    ///  <param name="Login">Логин пользователя</param>
    ///  <param name="Name">Имя пользователя</param>
    ///  <param name="SecondName">Отчество пользователя</param>
    ///  <param name="SurName">Фамилия пользователя</param>
    /// <param name="domain">Домен Active Directory, где работает пользователь.</param>
    /// <param name="mailDomain">Почтовый домен, в котором нужно созадавать почту пользователю.</param>
    /// <param name="password">Пароль пользователя</param>
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