namespace NewAdUser {

  public class AdUser {
    public string FullName;
    public string DisplayName;
    public string FirstName;
    public string SurName;
    public string SecondName;
    public string Login;
    public Domain Domain;
    public string Email => this.Login + "@" + this.Domain.MailFqdn;
    public string UPN => this.Login + "@" + this.Domain.AdFqdn;//  Domain.AdFQDN[this.domain];
    public string Password;

    public AdUser() {
    }

    ///  <summary>
    ///
    ///  </summary>
    ///  <param name="Login">����� ������������</param>
    ///  <param name="firstName">��� ������������</param>
    ///  <param name="SecondName">�������� ������������</param>
    ///  <param name="SurName">������� ������������</param>
    /// <param name="domain">����� Active Directory, ��� �������� ������������.</param>
    /// <param name="mailDomain">�������� �����, � ������� ����� ���������� ����� ������������.</param>
    /// <param name="password">������ ������������</param>
    public AdUser(string Login, string firstName, string SecondName, string SurName, Domain.AdDomain domain, Domain.MailDomain mailDomain, string password = "Zasqw12") {
      this.Login = Login;
      this.FullName = $"{SurName} {firstName} {SecondName}";
      this.FirstName = firstName;
      this.SecondName = SecondName;
      this.SurName = SurName;
      this.DisplayName = this.FullName;
      this.Password = password;
      this.Domain = new Domain() {
        AD = domain,
        Mail = mailDomain
      }; ;
    }
  }
}