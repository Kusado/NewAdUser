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
    private List<SqlInstance> SqlInstances;
    private Splash StartupSplash;
    private bool FormLoaded = false;
    private SqlConnection Sql;

    public Form1() {
      this.StartupSplash = Splash.ShowSplash();
      InitializeComponent();
      SetupVars();
      this.StartupSplash.Status = "Рисуем форму";
    }

    private void SetupVars() {
      //checkTrustedHosts();
      this.StartupSplash.Status = "Получаем список доменов";
      this.comboBoxAdDomain.DataSource = Enum.GetValues(typeof(Domain.AdDomain));
      this.StartupSplash.Status = "Получаем список почтовых доменов";
      this.comboBoxMailDomain.DataSource = Enum.GetValues(typeof(Domain.MailDomain));
      this.StartupSplash.Status = "Получаем список инстансов в сети";
#if DEBUG
      this.SqlInstances = SqlInstance.GetSqlServersFromNetwork(debug: true);
#else
       this.SqlInstances = SqlInstance.GetSqlServersFromNetwork(debug: false);
#endif
      this.comboBoxInstances.DataSource = this.SqlInstances;
      this.comboBoxInstances.DisplayMember = "InstanceFullName";
      this.comboBoxInstances.SelectedIndex = -1;
      this.checkBoxAddMail.Checked = true;
      this.checkBoxPassword.Checked = true;
      checkBox1_CheckedChanged(null, null);
      checkBoxAddMail_CheckedChanged(null, null);
      this.TransType = Helpers.TransliterationType.Gost;
    }

    private void checkTrustedHosts() {
      throw new NotImplementedException();
    }

    private void Form1_Load(object sender, EventArgs e) {
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
      this.textBoxFullname.Text = ComposeFullName();
    }

    private void SetLogin() {
      this.textBoxLogin.Text = ComposeLogin();
    }

    private string ComposeLogin() {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(this.textBoxName.Text)) {
        result += Helpers.Transliteration.Front(this.textBoxName.Text.Substring(0, 1), this.TransType);
      }
      if (!string.IsNullOrEmpty(this.textBoxSurName.Text)) {
        result += Helpers.Transliteration.Front(this.textBoxSurName.Text, this.TransType);
      }

      return result;
    }

    private string ComposeFullName() {
      string result = string.Empty;
      if (!string.IsNullOrEmpty(this.textBoxSurName.Text)) {
        result += $"{this.textBoxSurName.Text}";
      }
      if (!string.IsNullOrEmpty(this.textBoxName.Text)) {
        result += $" {this.textBoxName.Text}";
      }
      if (!string.IsNullOrEmpty(this.textBoxSecondName.Text)) {
        result += $" {this.textBoxSecondName.Text}";
      }
      return result;
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e) {
      this.textBoxUserPassword.Enabled = !this.checkBoxPassword.Checked;
      this.textBoxUserPassword.Text = this.checkBoxPassword.Checked ? DefaultPassword : String.Empty;
    }

    private void gOSTToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
      if (sender is ToolStripMenuItem m) {
        if (m.Checked) {
          ChangeTransitType(Helpers.TransliterationType.Gost);
          this.gOSTToolStripMenuItem.Checked = true;
          this.iSOToolStripMenuItem.Checked = !this.gOSTToolStripMenuItem.Checked;
        }
      }
    }

    private void ChangeTransitType(Helpers.TransliterationType tType) {
      this.TransType = tType;
      SetLogin();
    }

    private void iSOToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
      if (sender is ToolStripMenuItem m) {
        if (m.Checked) {
          ChangeTransitType(Helpers.TransliterationType.ISO);
          this.iSOToolStripMenuItem.Checked = true;
          this.gOSTToolStripMenuItem.Checked = false;
        }
      }
    }

    private void checkBoxAddMail_CheckedChanged(object sender, EventArgs e) {
      this.comboBoxMailDomain.Enabled = this.checkBoxAddMail.Checked;
      this.checkBoxOnlyMail.Enabled = this.checkBoxAddMail.Checked;
    }

    private void Form1_Shown(object sender, EventArgs e) {
      this.StartupSplash.CloseSplash();
      Focus();
      Activate();
      this.FormLoaded = true;
    }

    private void comboBoxInstances_SelectedIndexChanged(object sender, EventArgs e) {
      if (!this.FormLoaded) return;
      if (sender is ComboBox cmbx) {
        SqlInstance instance = cmbx.SelectedItem as SqlInstance;

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

    private void buttonExit_Click(object sender, EventArgs e) {
      Close();
    }

    private void ButtonAddKbUser_Click(object sender, EventArgs e) {
      if (!(this.listBoxRoles.SelectedItems.Count > 0 && this.listBoxMenu.SelectedItems.Count > 0)) { MessageBox.Show("Надо выбрать хотябы одну роль и меню.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); return; }
      string s = String.Empty;
      s += $"Добавляем пользователя {this.textBoxLogin.Text}@{this.comboBoxAdDomain.Text} на сервере {((SqlInstance)this.comboBoxInstances.SelectedItem).InstanceFullName}{Environment.NewLine}";
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
      string login = $"{ this.comboBoxAdDomain.SelectedText }\\{ this.textBoxLogin.Text}";
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
      string runasUsername;
      string runasPassword;
      Splash splash = Splash.ShowSplash();
      splash.Status = "Выбираем домены.";
      switch ((Domain.AdDomain)this.comboBoxAdDomain.SelectedIndex) {
        case Domain.AdDomain.Formulabi:
          runasUsername = AccountDetails.FormulaLogin;
          runasPassword = AccountDetails.FormulaPassword;
          break;

        case Domain.AdDomain.Radarias:
          runasUsername = AccountDetails.RadarLogin;
          runasPassword = AccountDetails.RadarPassword;
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }
      splash.Status = "Секурим пароли.";
      SecureString ssRunasPassword = new SecureString();
      foreach (char x in runasPassword) {
        ssRunasPassword.AppendChar(x);
      }
      splash.Status = "Создаём учётные данные.";
      PSCredential credentials = new PSCredential(runasUsername, ssRunasPassword);

      splash.Status = "Генерим шаблон пользователя.";

      AdUser NewUser = new AdUser(this.textBoxLogin.Text, this.textBoxName.Text, this.textBoxSecondName.Text, this.textBoxSurName.Text, (Domain.AdDomain)this.comboBoxAdDomain.SelectedIndex, (Domain.MailDomain)this.comboBoxMailDomain.SelectedIndex);

      if (!this.checkBoxPassword.Checked) NewUser.Password = this.textBoxUserPassword.Text;

      splash.Status = "Начинаем создание пользователя.";
      if (this.checkBoxOnlyMail.Enabled && !this.checkBoxOnlyMail.Checked) {
        Helpers.AddADUser(NewUser, credentials, splash);
      }

      if (this.checkBoxAddMail.Checked) {
        string ExchangeUsername = AccountDetails.FbiLogin;
        string ExchangePassword = AccountDetails.FbiPassword;
        SecureString SecExchangePassword = new SecureString();
        foreach (char x in ExchangePassword) {
          SecExchangePassword.AppendChar(x);
        }
        PSCredential ExchangeCreds = new PSCredential(ExchangeUsername, SecExchangePassword);
        Helpers.AddMailUser(NewUser, ExchangeCreds, splash);
      }
      splash?.CloseSplash();
    }
  }
}