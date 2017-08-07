using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Helpers {

  public partial class Splash : Form {
    public Thread MyThread;
    public bool LoadDefaults;
    private string _status;

    public string Status {
      get { return this._status; }
      set {
        this._status = value;
        if (this.Created) this.Invoke((MethodInvoker)delegate { this.label2.Text = this._status; });
      }
    }

    public Splash() {
      InitializeComponent();
      Color labelColor = Color.OrangeRed;
      this.label1.Parent = this.pictureBox1;
      this.label1.BackColor = Color.Transparent;
      this.label1.ForeColor = labelColor;

      this.label2.Parent = this.pictureBox1;
      this.label2.BackColor = Color.Transparent;
      this.label2.ForeColor = labelColor;

      Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      this.label3.Text = version.ToString();
      this.label3.Parent = this.pictureBox1;
      this.label3.BackColor = Color.Transparent;
      this.label3.ForeColor = labelColor;

      Status = String.Empty;
    }

    //private MainForm mainForm{ get; set; }

    public delegate void CloseDel();

    public static Splash ShowSplash(string Status1 = "Загрузка...") {
      Splash s = new Splash();
      s.label1.Text = Status1;
      //s.mainForm = mainForm;
      s.MyThread = new Thread(s._showSplash);
      s.MyThread.Start();
      while (!s.Visible) {
        Thread.Sleep(50);
      }
      return s;
    }

    private void _showSplash() {
      this.BringToFront();
      this.ShowDialog();
    }

    public void CloseSplash() {
      this.Invoke((MethodInvoker)delegate { this.Close(); });
    }

    private void ButtonCancel_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void Splash_FormClosing(object sender, FormClosingEventArgs e) {
    }
  }
}