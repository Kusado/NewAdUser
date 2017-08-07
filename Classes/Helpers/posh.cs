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
    /// <param name="server">Имя контроллера домена к которому нужно подключиться</param>
    /// <param name="creds"></param>
    /// <param name="path">Путь к OU</param>
    public static void AddUser(AdUser user, string server, PSCredential creds, string path) {
      string Script = $"import-module activedirectory;" +
      $"$sec_pass = ConvertTo-SecureString {user.password} -AsPlainText -Force;" +
      $"New-ADUser -Name '{user.FullName}'" +
                      $" -AccountPassword $sec_pass" +
                      $" -DisplayName '{user.DisplayName}'" +
                      $" -GivenName '{user.Name}'" +
                      $" -Surname '{user.SurName}'" +
                      $" -Path '{path}'" +
                      $" -SamAccountName '{user.login}'" +
                      $" -UserPrincipalName '{user.UPN}'" +
                      $" -enabled $true;";
      Debug.WriteLine(Script);

      WSManConnectionInfo connInfo = new WSManConnectionInfo(new Uri($"http://{server}:5985"), "http://schemas.microsoft.com/powershell/Microsoft.PowerShell", creds);
      Runspace runspace = RunspaceFactory.CreateRunspace(connInfo);
      PowerShell shell = PowerShell.Create(RunspaceMode.NewRunspace);

      runspace.Open();
      shell.Runspace = runspace;

      shell.AddScript(Script);
      try {
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