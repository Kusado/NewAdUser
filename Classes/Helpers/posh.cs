using Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace NewAdUser {

  public static class posh {

    ///  <summary>
    /// Создать пользователя в домене AD, подключившись к контроллеру по WinRM:Powershell
    ///  </summary>
    /// <param name="user"></param>
   // /// <param name="server">Имя контроллера домена к которому нужно подключиться</param>
    /// <param name="creds"></param>
    /// <param name="path">Путь к OU</param>
    public static void AddUser(AdUser user, PSCredential creds, Splash splash) {
      if (splash != null) splash.Status = "Генерим скрипт.";
      string Script = $"import-module activedirectory;" +
      $"$sec_pass = ConvertTo-SecureString {user.Password} -AsPlainText -Force;" +
      $"New-ADUser -Name '{user.FullName}'" +
                      $" -AccountPassword $sec_pass" +
                      $" -DisplayName '{user.DisplayName}'" +
                      $" -GivenName '{user.FirstName}'" +
                      $" -Surname '{user.SurName}'" +
                      $" -Path '{user.Domain.LoginOU}'" +
                      $" -SamAccountName '{user.Login}'" +
                      $" -UserPrincipalName '{user.UPN}'" +
                      $" -enabled $true;";
      Debug.WriteLine(Script);

      if (splash != null) splash.Status = "Создаём ранспейс.";
      WSManConnectionInfo connInfo = new WSManConnectionInfo(new Uri($"http://{user.Domain.DC}:5985"), "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", creds);
      Runspace runspace = RunspaceFactory.CreateRunspace(connInfo);
      PowerShell shell = PowerShell.Create(RunspaceMode.NewRunspace);
      if (splash != null) splash.Status = $"Открываем ранспейс на сервере {user.Domain.DC}";
      runspace.Open();
      shell.Runspace = runspace;
      if (splash != null) splash.Status = $"Добавляем скрипт в пайплайн.";
      shell.AddScript(Script);
      try {
        if (splash != null) splash.Status = $"Выполняем скрипт.";
        Collection<PSObject> result = shell.Invoke();
        if (shell.HadErrors) {
          foreach (ErrorRecord errorRecord in shell.Streams.Error) {
            Debug.WriteLine(errorRecord.Exception);
            throw errorRecord.Exception;
          }
        }
        foreach (PSObject psObject in result) {
          Debug.WriteLine(psObject.ToString());
        }
      }
      catch (Exception e) {
        Debug.WriteLine(e);
        //throw;
      }

      runspace.CloseAsync();
      shell.Dispose();
    }

    public static void AddMailUser(AdUser user, PSCredential creds, Splash splash, string server = "fbi-exch01.fbi.local") {
      if (splash != null) splash.Status = $"Генерим скрипт.";
      string Script = $"New-Mailbox -Name '{user.FullName}'" +
                          $" -Alias '{user.Login}'" +
                          $" -OrganizationalUnit '{user.Domain.MailboxOU}'" +
                          $" -UserPrincipalName '{user.Login}@fbi.local'" +
                          $" -SamAccountName '{user.Login}'" +
                          $" -FirstName '{user.FirstName}'" +
                          $" -Initials ''" +
                          $" -LastName '{user.SurName}'" +
                          $" -LinkedMasterAccount '{user.Domain.AD}\\{user.Login}'" +
                          $" -LinkedDomainController '{user.Domain.DC}'";
      Debug.WriteLine(Script);
      if (splash != null) splash.Status = $"Создаём подключение к {server}.";
      WSManConnectionInfo connInfo = new WSManConnectionInfo(new Uri($"https://{server}/Powershell"), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", creds);
      Runspace runspace = RunspaceFactory.CreateRunspace(connInfo);
      PowerShell shell = PowerShell.Create(RunspaceMode.NewRunspace);
      if (splash != null) splash.Status = $"Открываем подключение к {server}.";
      runspace.Open();
      shell.Runspace = runspace;

      shell.AddScript(Script);
      try {
        if (splash != null) splash.Status = $"Выполняем скрипт.";
        Collection<PSObject> result = shell.Invoke();
        if (shell.HadErrors) {
          foreach (ErrorRecord errorRecord in shell.Streams.Error) {
            Debug.WriteLine(errorRecord.Exception);
            throw errorRecord.Exception;
          }
        }
        foreach (PSObject psObject in result) {
          Debug.WriteLine(psObject.ToString());
        }
      }
      catch (Exception e) {
        Debug.WriteLine(e);
        //throw;
      }

      runspace.CloseAsync();
      shell.Dispose();
    }
  }
}