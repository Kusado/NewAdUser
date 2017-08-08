using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace NewAdUser {

  public partial class Form1 {

    private void ButtonAddKbUser_Click(object sender, EventArgs e) {
      //if (!(this.listBoxRoles.SelectedItems.Count > 0 && this.listBoxMenu.SelectedItems.Count > 0)) {
      //  MessageBox.Show("Надо выбрать хотябы одну роль и меню.", "Внимание!", MessageBoxButtons.OK,
      //    MessageBoxIcon.Exclamation);
      //  return;
      //}
      //string s = String.Empty;
      //s +=
      //  $"Добавляем пользователя {this.textBoxLogin.Text}@{this.comboBoxAdDomain.Text} на сервере {((SqlInstance)this.comboBoxInstances.SelectedItem).InstanceFullName}{Environment.NewLine}";
      //s += "Даём ему роль:";
      //foreach (Roles item in this.listBoxRoles.SelectedItems) {
      //  s += $"{item.RoleName},{Environment.NewLine}";
      //}
      //s += "Включаем ему меню:";
      //foreach (KBMenu item in this.listBoxMenu.SelectedItems) {
      //  s += $"{item.MenuName},{Environment.NewLine}";
      //}
      //DialogResult mbx = MessageBox.Show(s, "Добавляем?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      //if (mbx != DialogResult.Yes) return;
      string login = $"{(Domain.AdDomain)this.comboBoxAdDomain.SelectedItem}\\{this.textBoxLogin.Text}";
      try {
        AddUserToSql(login);
        AddUserToDB(login);
        AddUserToKB(login);
      }
      catch (Exception exception) {
        Console.WriteLine(exception);
        throw;
      }
    }

    private void AddUserToSql(string login) {
      string query =
        $"IF NOT EXISTS(SELECT loginname FROM sys.syslogins WHERE loginname = '{login}') EXEC sp_grantlogin '{login}'";
      SqlCommand cmd = new SqlCommand(query, this.Sql);
      cmd.ExecuteNonQuery();
    }

    private void AddUserToDB(string login) {
      string query =
        $"IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = '{login}') CREATE USER [{login}] FOR LOGIN [{login}]";
      SqlCommand cmd = new SqlCommand(query, this.Sql);
      cmd.ExecuteNonQuery();
    }

    private void AddUserToKB(string login) {
      string query =
        $@" IF NOT EXISTS(SELECT 1 FROM Logins WHERE Login = '{login}') BEGIN" +
        $" DECLARE @IDUser int;" +
        $" INSERT INTO Users (LastName, FirstName, MiddleName, EMail) VALUES ('{this.textBoxSurName.Text}', '{this.textBoxName.Text}', '{this.textBoxSecondName.Text}', NULL)" +
        $" SET @IDUser = SCOPE_IDENTITY();" +
        $" INSERT INTO Logins (IDUser, Login) VALUES (@IDUser, '{login}')" +
        $" END";
      SqlCommand cmd = new SqlCommand(query, this.Sql);
      cmd.ExecuteNonQuery();
    }

    private List<Roles> GetRoles(SqlInstance instance) {
      List<Roles> result = new List<Roles>();
      string query = @"SELECT * FROM [KB].[dbo].[Roles]";
      DataTable dt = SqlInstance.GetSqlDataTable(query, this.Sql);
      try {
        result = dt.ToList<Roles>();
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
      return result;
    }

    private List<KBMenu> GetMenus(SqlInstance instance) {
      List<KBMenu> result = new List<KBMenu>();
      string query = @"SELECT * FROM [KB].[dbo].[Menu] where IdMenuType = 4";
      DataTable dt = SqlInstance.GetSqlDataTable(query, this.Sql);
      try {
        result = dt.ToList<KBMenu>();
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }

      return result;
    }

    public static T ConvertToTypedDataTable<T>(DataTable dtBase) where T : DataTable, new() {
      T dtTyped = new T();
      dtTyped.Merge(dtBase);

      return dtTyped;
    }
  }
}