using System;

namespace NewAdUser {
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
    public Domains.ActiveDirectory domain;

    public string UPN {
      get {
        string domainName;
        switch (this.domain) {
          case Domains.ActiveDirectory.Formulabi:
            domainName = "@formulabi.local";
            break;

          case Domains.ActiveDirectory.Radarias:
            domainName = "@radarias.local";
            break;

          default:
            throw new ArgumentOutOfRangeException();
        }
        return this.login+domainName;
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
    public AdUser(string Login, string Name, string SecondName, string SurName, Domains.ActiveDirectory domain, string password = "Zasqw12") {
      this.login = Login;
      this.FullName = $"{SurName} {Name} {SecondName}";
      this.DisplayName = this.FullName;
      this.password = password;
      this.domain = domain;
    }
  }
}