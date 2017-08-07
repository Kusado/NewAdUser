using Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management.Automation;
using System.Security;
using System.Windows.Forms;

namespace NewAdUser {

  public partial class Form1 : Form {
    private const string DefaultPassword = "Zasqw12";
    private Helpers.TransliterationType TransType;
    private List<Helpers.SqlInstance> SqlInstances;
    private Splash StartupSplash;
    private bool FormLoaded = false;
    private SqlConnection Sql;

    public Form1() {
      StartupSplash = Splash.ShowSplash();
      InitializeComponent();
      SetupVars();
      this.StartupSplash.Status = "Рисуем форму";
    }

    private void SetupVars() {
      this.StartupSplash.Status = "Получаем список доменов";
      comboBoxDomain.DataSource = Enum.GetValues(typeof(Helpers.AdDomain));
      this.StartupSplash.Status = "Получаем список почтовых доменов";
      comboBoxMailDomain.DataSource = Enum.GetValues(typeof(Helpers.MailDomain));
      this.StartupSplash.Status = "Получаем список инстансов в сети";
      //this.SqlInstances = GetSqlServersFromNetwork();
      //this.comboBoxInstances.DataSource = this.SqlInstances;
      //this.comboBoxInstances.DisplayMember = "InstanceFullName";
      //this.comboBoxInstances.SelectedIndex = -1;
      this.checkBoxAddMail.Checked = true;
      this.checkBoxPassword.Checked = true;
      checkBox1_CheckedChanged(null, null);
      checkBoxAddMail_CheckedChanged(null, null);
    }

    private void Form1_Load(object sender, EventArgs e) {
      TransType = Helpers.TransliterationType.Gost;
    }

    private void textBoxLastname_TextChanged(object sender, EventArgs e) {
      SetLogin();
      SetFullName();
    }

    private void textBoxName_TextChanged(object sender, EventArgs e) {
      SetLogin();
      SetFullName();
    }

    private void textBoxSurname_TextChanged(object sender, EventArgs e) {
      SetFullName();
    }

    private void SetFullName() {
      textBoxFullname.Text = ComposeFullName();
    }

    private void SetLogin() {
      textBoxLogin.Text = ComposeLogin();
    }

    private string ComposeLogin() {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(textBoxName.Text)) {
        result += Helpers.Transliteration.Front(textBoxName.Text.Substring(0, 1), TransType);
      }
      if (!string.IsNullOrEmpty(textBoxSurName.Text)) {
        result += Helpers.Transliteration.Front(textBoxSurName.Text, TransType);
      }

      return result;
    }

    private string ComposeFullName() {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(textBoxSurName.Text)) {
        result += $"{textBoxSurName.Text}";
      }
      if (!string.IsNullOrEmpty(textBoxName.Text)) {
        result += $" {textBoxName.Text}";
      }
      if (!string.IsNullOrEmpty(textBoxSecondName.Text)) {
        result += $" {textBoxSecondName.Text}";
      }
      return result;
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e) {
      textBoxUserPassword.Enabled = !checkBoxPassword.Checked;
      textBoxUserPassword.Text = checkBoxPassword.Checked ? DefaultPassword : String.Empty;
    }

    private void gOSTToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
      if (sender is ToolStripMenuItem m) {
        if (m.Checked) {
          ChangeTransitType(Helpers.TransliterationType.Gost);
          gOSTToolStripMenuItem.Checked = true;
          iSOToolStripMenuItem.Checked = !gOSTToolStripMenuItem.Checked;
        }
      }
    }

    private void ChangeTransitType(Helpers.TransliterationType tType) {
      TransType = tType;
      SetLogin();
    }

    private void iSOToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
      if (sender is ToolStripMenuItem m) {
        if (m.Checked) {
          ChangeTransitType(Helpers.TransliterationType.ISO);
          iSOToolStripMenuItem.Checked = true;
          gOSTToolStripMenuItem.Checked = false;
        }
      }
    }

    private void checkBoxAddMail_CheckedChanged(object sender, EventArgs e) {
      this.comboBoxMailDomain.Enabled = this.checkBoxPassword.Checked;
    }

    private void Form1_Shown(object sender, EventArgs e) {
      this.StartupSplash.CloseSplash();
      this.Focus();
      this.Activate();
      this.FormLoaded = true;
    }

    private void comboBoxInstances_SelectedIndexChanged(object sender, EventArgs e) {
      if (!this.FormLoaded) return;
      if (sender is ComboBox cmbx) {
        Helpers.SqlInstance instance = cmbx.SelectedItem as Helpers.SqlInstance;

        string ConnectionString = (instance.InstanceName == "DEFAULT") ? $@"Server={instance.Host.FQDN};Database=KB;Trusted_Connection=True;Connection Timeout=2;" : $@"Server={instance.InstanceFullName};Database=KB;Trusted_Connection=True;Connection Timeout=2;";
        if (this.Sql == null) this.Sql = new SqlConnection(ConnectionString);
        switch (this.Sql.State) {
          case ConnectionState.Open:
          case ConnectionState.Connecting:
          case ConnectionState.Executing:
          case ConnectionState.Fetching:
          case ConnectionState.Broken:
            this.Sql.Close();
            this.Sql = new SqlConnection(ConnectionString);
            break;
        }
        this.Sql.Open();

        List<Roles> roles = GetRoles(instance);

        if (roles.Any()) {
          this.listBoxRoles.Enabled = true;
          this.listBoxRoles.DataSource = roles;
          this.listBoxRoles.DisplayMember = "RoleName";
          this.listBoxRoles.ClearSelected();
        }
        else {
          this.listBoxRoles.Items.Clear();
          this.listBoxRoles.Enabled = false;
        }
        List<KBMenu> menus = GetMenus(instance);
        if (roles.Any()) {
          this.listBoxMenu.Enabled = true;
          this.listBoxMenu.DataSource = menus;
          this.listBoxMenu.DisplayMember = "MenuName";
          this.listBoxMenu.ClearSelected();
        }
        else {
          this.listBoxMenu.Items.Clear();
          this.listBoxMenu.Enabled = false;
        }
      }
    }

    private List<Roles> GetRoles(Helpers.SqlInstance instance) {
      List<Roles> result = new List<Roles>();
      string query = @"SELECT * FROM [KB].[dbo].[Roles]";
      DataTable dt = Helpers.GetSqlDataTable(query, this.Sql);
      try {
        result = dt.ToList<Roles>();
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }
      return result;
    }

    private List<KBMenu> GetMenus(Helpers.SqlInstance instance) {
      List<KBMenu> result = new List<KBMenu>();
      string query = @"SELECT * FROM [KB].[dbo].[Menu] where IdMenuType = 4";
      DataTable dt = Helpers.GetSqlDataTable(query, this.Sql);
      try {
        result = dt.ToList<KBMenu>();
      }
      catch (Exception e) {
        Console.WriteLine(e);
        throw;
      }

      return result;
    }

    public static T ConvertToTypedDataTable<T>(System.Data.DataTable dtBase) where T : DataTable, new() {
      T dtTyped = new T();
      dtTyped.Merge(dtBase);

      return dtTyped;
    }

    private void buttonExit_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void ButtonAddKbUser_Click(object sender, EventArgs e) {
      if (!(this.listBoxRoles.SelectedItems.Count > 0 && this.listBoxMenu.SelectedItems.Count > 0)) { MessageBox.Show("Надо выбрать хотябы одну роль и меню.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
      string s = String.Empty;
      s += $"Добавляем пользователя {this.textBoxLogin.Text}@{this.comboBoxDomain.Text} на сервере {((Helpers.SqlInstance)this.comboBoxInstances.SelectedItem).InstanceFullName}{Environment.NewLine}";
      s += "Даём ему роль:";
      foreach (Roles item in this.listBoxRoles.SelectedItems) {
        s += $"{item.RoleName},{Environment.NewLine}";
      }
      s += "Включаем ему меню:";
      foreach (KBMenu item in this.listBoxMenu.SelectedItems) {
        s += $"{item.MenuName},{Environment.NewLine}";
      }
      DialogResult mbx = MessageBox.Show(s, "Добавляем?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
      if (mbx != DialogResult.Yes) return;
      string login = $"{ this.comboBoxDomain.SelectedText }\\{ this.textBoxLogin.Text}";
      AddUserToSql(login);
      AddUserToDB(login);
    }

    private void AddUserToSql(string login) {
      string query = $"IF NOT EXISTS(SELECT loginname FROM sys.syslogins WHERE loginname = '{login}') EXEC sp_grantlogin '{login}'";
      SqlCommand cmd = new SqlCommand(query, this.Sql);
      cmd.ExecuteNonQuery();
    }

    private void AddUserToDB(string login) {
      string query = $"IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = '{login}') CREATE USER [{login}] FOR LOGIN [{login}]";
      SqlCommand cmd = new SqlCommand(query, this.Sql);
      cmd.ExecuteNonQuery();
    }

    private void buttonAddUser_Click(object sender, EventArgs e) {

      string runasUsername = @"radarias\iabramov";
      string runasPassword = "*********";

      SecureString ssRunasPassword = new SecureString();
      foreach (char x in runasPassword) {
        ssRunasPassword.AppendChar(x);
      }
      PSCredential credentials = new PSCredential(runasUsername, ssRunasPassword);

      //AdUser NewUser = new AdUser(this.textBoxLogin.Text,this.textBoxLogin.Text + "@" + this.comboBoxDomain.SelectedItem,this.textBoxName.Text, this.textBoxSecondName.Text,this.textBoxSurName.Text);

      AdUser NewUser = new AdUser(this.textBoxLogin.Text,this.textBoxName.Text,this.textBoxSecondName.Text,this.textBoxSurName.Text,(Helpers.AdDomain)this.comboBoxDomain.SelectedItem);


      if (!this.checkBoxPassword.Checked) NewUser.password = this.textBoxUserPassword.Text;

      Helpers.addADUser(NewUser, (Helpers.AdDomain)this.comboBoxDomain.SelectedItem, credentials);



    }
  }
}