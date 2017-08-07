namespace NewAdUser {
   partial class Form1 {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.textBoxSurName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.textBoxName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.textBoxSecondName = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.textBoxFullname = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.textBoxLogin = new System.Windows.Forms.TextBox();
      this.contextMenuTranslitType = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.iSOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.gOSTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.textBoxUserPassword = new System.Windows.Forms.TextBox();
      this.checkBoxPassword = new System.Windows.Forms.CheckBox();
      this.comboBoxDomain = new System.Windows.Forms.ComboBox();
      this.comboBoxMailDomain = new System.Windows.Forms.ComboBox();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.checkBoxAddMail = new System.Windows.Forms.CheckBox();
      this.buttonAddUser = new System.Windows.Forms.Button();
      this.buttonAddKBUser = new System.Windows.Forms.Button();
      this.buttonExit = new System.Windows.Forms.Button();
      this.listBoxMenu = new System.Windows.Forms.ListBox();
      this.label11 = new System.Windows.Forms.Label();
      this.listBoxRoles = new System.Windows.Forms.ListBox();
      this.label10 = new System.Windows.Forms.Label();
      this.comboBoxInstances = new System.Windows.Forms.ComboBox();
      this.label9 = new System.Windows.Forms.Label();
      this.contextMenuTranslitType.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBoxSurName
      // 
      this.textBoxSurName.Location = new System.Drawing.Point(3, 25);
      this.textBoxSurName.Name = "textBoxSurName";
      this.textBoxSurName.Size = new System.Drawing.Size(186, 20);
      this.textBoxSurName.TabIndex = 0;
      this.textBoxSurName.Text = "фамилия";
      this.textBoxSurName.TextChanged += new System.EventHandler(this.textBoxLastname_TextChanged);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(3, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(186, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Фамилия";
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(3, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(186, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Имя";
      // 
      // textBoxName
      // 
      this.textBoxName.Location = new System.Drawing.Point(3, 64);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new System.Drawing.Size(186, 20);
      this.textBoxName.TabIndex = 2;
      this.textBoxName.Text = "имя";
      this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(3, 87);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(186, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Отчество";
      // 
      // textBoxSecondName
      // 
      this.textBoxSecondName.Location = new System.Drawing.Point(3, 103);
      this.textBoxSecondName.Name = "textBoxSecondName";
      this.textBoxSecondName.Size = new System.Drawing.Size(186, 20);
      this.textBoxSecondName.TabIndex = 4;
      this.textBoxSecondName.Text = "отчество";
      this.textBoxSecondName.TextChanged += new System.EventHandler(this.textBoxSurname_TextChanged);
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(3, 126);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(186, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "ФИО";
      // 
      // textBoxFullname
      // 
      this.textBoxFullname.Location = new System.Drawing.Point(3, 142);
      this.textBoxFullname.Name = "textBoxFullname";
      this.textBoxFullname.Size = new System.Drawing.Size(186, 20);
      this.textBoxFullname.TabIndex = 6;
      // 
      // label5
      // 
      this.label5.Location = new System.Drawing.Point(3, 279);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(186, 13);
      this.label5.TabIndex = 15;
      this.label5.Text = "Пароль";
      // 
      // label6
      // 
      this.label6.Location = new System.Drawing.Point(3, 240);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(186, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Почтовый домен";
      // 
      // label7
      // 
      this.label7.Location = new System.Drawing.Point(3, 201);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(186, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Домен";
      // 
      // label8
      // 
      this.label8.Location = new System.Drawing.Point(3, 162);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(186, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Имя пользователя:";
      // 
      // textBoxLogin
      // 
      this.textBoxLogin.ContextMenuStrip = this.contextMenuTranslitType;
      this.textBoxLogin.Location = new System.Drawing.Point(3, 178);
      this.textBoxLogin.Name = "textBoxLogin";
      this.textBoxLogin.Size = new System.Drawing.Size(186, 20);
      this.textBoxLogin.TabIndex = 8;
      // 
      // contextMenuTranslitType
      // 
      this.contextMenuTranslitType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iSOToolStripMenuItem,
            this.gOSTToolStripMenuItem});
      this.contextMenuTranslitType.Name = "contextMenuTranslitType";
      this.contextMenuTranslitType.Size = new System.Drawing.Size(105, 48);
      this.contextMenuTranslitType.Text = "sfdasdfa";
      // 
      // iSOToolStripMenuItem
      // 
      this.iSOToolStripMenuItem.Checked = true;
      this.iSOToolStripMenuItem.CheckOnClick = true;
      this.iSOToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
      this.iSOToolStripMenuItem.Name = "iSOToolStripMenuItem";
      this.iSOToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
      this.iSOToolStripMenuItem.Text = "ISO";
      this.iSOToolStripMenuItem.CheckedChanged += new System.EventHandler(this.iSOToolStripMenuItem_CheckedChanged);
      // 
      // gOSTToolStripMenuItem
      // 
      this.gOSTToolStripMenuItem.CheckOnClick = true;
      this.gOSTToolStripMenuItem.Name = "gOSTToolStripMenuItem";
      this.gOSTToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
      this.gOSTToolStripMenuItem.Text = "GOST";
      this.gOSTToolStripMenuItem.CheckedChanged += new System.EventHandler(this.gOSTToolStripMenuItem_CheckedChanged);
      // 
      // textBoxUserPassword
      // 
      this.textBoxUserPassword.Location = new System.Drawing.Point(3, 295);
      this.textBoxUserPassword.Name = "textBoxUserPassword";
      this.textBoxUserPassword.Size = new System.Drawing.Size(119, 20);
      this.textBoxUserPassword.TabIndex = 14;
      this.textBoxUserPassword.UseSystemPasswordChar = true;
      // 
      // checkBoxPassword
      // 
      this.checkBoxPassword.AutoSize = true;
      this.checkBoxPassword.Checked = true;
      this.checkBoxPassword.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxPassword.Location = new System.Drawing.Point(128, 297);
      this.checkBoxPassword.Name = "checkBoxPassword";
      this.checkBoxPassword.Size = new System.Drawing.Size(58, 17);
      this.checkBoxPassword.TabIndex = 16;
      this.checkBoxPassword.Text = "default";
      this.checkBoxPassword.UseVisualStyleBackColor = true;
      this.checkBoxPassword.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // comboBoxDomain
      // 
      this.comboBoxDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxDomain.FormattingEnabled = true;
      this.comboBoxDomain.Location = new System.Drawing.Point(3, 218);
      this.comboBoxDomain.Name = "comboBoxDomain";
      this.comboBoxDomain.Size = new System.Drawing.Size(186, 21);
      this.comboBoxDomain.TabIndex = 17;
      // 
      // comboBoxMailDomain
      // 
      this.comboBoxMailDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxMailDomain.FormattingEnabled = true;
      this.comboBoxMailDomain.Location = new System.Drawing.Point(3, 255);
      this.comboBoxMailDomain.Name = "comboBoxMailDomain";
      this.comboBoxMailDomain.Size = new System.Drawing.Size(119, 21);
      this.comboBoxMailDomain.TabIndex = 18;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.checkBoxAddMail);
      this.splitContainer1.Panel1.Controls.Add(this.buttonAddUser);
      this.splitContainer1.Panel1.Controls.Add(this.label1);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxSurName);
      this.splitContainer1.Panel1.Controls.Add(this.comboBoxMailDomain);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxName);
      this.splitContainer1.Panel1.Controls.Add(this.comboBoxDomain);
      this.splitContainer1.Panel1.Controls.Add(this.label2);
      this.splitContainer1.Panel1.Controls.Add(this.checkBoxPassword);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxSecondName);
      this.splitContainer1.Panel1.Controls.Add(this.label5);
      this.splitContainer1.Panel1.Controls.Add(this.label3);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxUserPassword);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxFullname);
      this.splitContainer1.Panel1.Controls.Add(this.label6);
      this.splitContainer1.Panel1.Controls.Add(this.label4);
      this.splitContainer1.Panel1.Controls.Add(this.label7);
      this.splitContainer1.Panel1.Controls.Add(this.textBoxLogin);
      this.splitContainer1.Panel1.Controls.Add(this.label8);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.buttonAddKBUser);
      this.splitContainer1.Panel2.Controls.Add(this.buttonExit);
      this.splitContainer1.Panel2.Controls.Add(this.listBoxMenu);
      this.splitContainer1.Panel2.Controls.Add(this.label11);
      this.splitContainer1.Panel2.Controls.Add(this.listBoxRoles);
      this.splitContainer1.Panel2.Controls.Add(this.label10);
      this.splitContainer1.Panel2.Controls.Add(this.comboBoxInstances);
      this.splitContainer1.Panel2.Controls.Add(this.label9);
      this.splitContainer1.Size = new System.Drawing.Size(408, 343);
      this.splitContainer1.SplitterDistance = 196;
      this.splitContainer1.TabIndex = 19;
      // 
      // checkBoxAddMail
      // 
      this.checkBoxAddMail.AutoSize = true;
      this.checkBoxAddMail.Checked = true;
      this.checkBoxAddMail.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxAddMail.Location = new System.Drawing.Point(128, 255);
      this.checkBoxAddMail.Name = "checkBoxAddMail";
      this.checkBoxAddMail.Size = new System.Drawing.Size(56, 17);
      this.checkBoxAddMail.TabIndex = 20;
      this.checkBoxAddMail.Text = "Почта";
      this.checkBoxAddMail.UseVisualStyleBackColor = true;
      this.checkBoxAddMail.CheckedChanged += new System.EventHandler(this.checkBoxAddMail_CheckedChanged);
      // 
      // buttonAddUser
      // 
      this.buttonAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonAddUser.Location = new System.Drawing.Point(26, 317);
      this.buttonAddUser.Name = "buttonAddUser";
      this.buttonAddUser.Size = new System.Drawing.Size(139, 23);
      this.buttonAddUser.TabIndex = 19;
      this.buttonAddUser.Text = "Добавить пользователя";
      this.buttonAddUser.UseVisualStyleBackColor = true;
      this.buttonAddUser.Click += new System.EventHandler(this.buttonAddUser_Click);
      // 
      // buttonAddKBUser
      // 
      this.buttonAddKBUser.Location = new System.Drawing.Point(10, 237);
      this.buttonAddKBUser.Name = "buttonAddKBUser";
      this.buttonAddKBUser.Size = new System.Drawing.Size(186, 23);
      this.buttonAddKBUser.TabIndex = 27;
      this.buttonAddKBUser.Text = "Добавить пользователя Sherp";
      this.buttonAddKBUser.UseVisualStyleBackColor = true;
      this.buttonAddKBUser.Click += new System.EventHandler(this.ButtonAddKbUser_Click);
      // 
      // buttonExit
      // 
      this.buttonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonExit.Location = new System.Drawing.Point(130, 317);
      this.buttonExit.Name = "buttonExit";
      this.buttonExit.Size = new System.Drawing.Size(75, 23);
      this.buttonExit.TabIndex = 26;
      this.buttonExit.Text = "Выход";
      this.buttonExit.UseVisualStyleBackColor = true;
      this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
      // 
      // listBoxMenu
      // 
      this.listBoxMenu.Enabled = false;
      this.listBoxMenu.FormattingEnabled = true;
      this.listBoxMenu.Location = new System.Drawing.Point(10, 162);
      this.listBoxMenu.Name = "listBoxMenu";
      this.listBoxMenu.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listBoxMenu.Size = new System.Drawing.Size(186, 69);
      this.listBoxMenu.TabIndex = 25;
      // 
      // label11
      // 
      this.label11.Location = new System.Drawing.Point(10, 146);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(186, 13);
      this.label11.TabIndex = 24;
      this.label11.Text = "Меню";
      // 
      // listBoxRoles
      // 
      this.listBoxRoles.Enabled = false;
      this.listBoxRoles.FormattingEnabled = true;
      this.listBoxRoles.Location = new System.Drawing.Point(10, 63);
      this.listBoxRoles.Name = "listBoxRoles";
      this.listBoxRoles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.listBoxRoles.Size = new System.Drawing.Size(186, 82);
      this.listBoxRoles.TabIndex = 23;
      // 
      // label10
      // 
      this.label10.Location = new System.Drawing.Point(10, 47);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(186, 13);
      this.label10.TabIndex = 20;
      this.label10.Text = "Роли";
      // 
      // comboBoxInstances
      // 
      this.comboBoxInstances.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxInstances.FormattingEnabled = true;
      this.comboBoxInstances.Location = new System.Drawing.Point(10, 25);
      this.comboBoxInstances.Name = "comboBoxInstances";
      this.comboBoxInstances.Size = new System.Drawing.Size(186, 21);
      this.comboBoxInstances.TabIndex = 19;
      this.comboBoxInstances.SelectedIndexChanged += new System.EventHandler(this.comboBoxInstances_SelectedIndexChanged);
      // 
      // label9
      // 
      this.label9.Location = new System.Drawing.Point(10, 8);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(186, 13);
      this.label9.TabIndex = 18;
      this.label9.Text = "Инстанс";
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonExit;
      this.ClientSize = new System.Drawing.Size(408, 343);
      this.Controls.Add(this.splitContainer1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.Text = "Добавление нового пользователя";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.Shown += new System.EventHandler(this.Form1_Shown);
      this.contextMenuTranslitType.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TextBox textBoxSurName;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox textBoxName;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox textBoxSecondName;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox textBoxFullname;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.TextBox textBoxLogin;
      private System.Windows.Forms.TextBox textBoxUserPassword;
      private System.Windows.Forms.CheckBox checkBoxPassword;
      private System.Windows.Forms.ComboBox comboBoxDomain;
      private System.Windows.Forms.ComboBox comboBoxMailDomain;
      private System.Windows.Forms.ContextMenuStrip contextMenuTranslitType;
      private System.Windows.Forms.ToolStripMenuItem iSOToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem gOSTToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.CheckBox checkBoxAddMail;
    private System.Windows.Forms.Button buttonAddUser;
    private System.Windows.Forms.ComboBox comboBoxInstances;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ListBox listBoxRoles;
    private System.Windows.Forms.Button buttonExit;
    private System.Windows.Forms.ListBox listBoxMenu;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Button buttonAddKBUser;
  }
}

