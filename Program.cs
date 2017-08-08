using System;
using System.Windows.Forms;

namespace NewAdUser {

  internal static class Program {

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main() {
      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new Form1());
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
      throw (Exception)e.ExceptionObject;
    }
  }
}