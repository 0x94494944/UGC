using System;
using System.Windows.Forms;

namespace ProjectUGC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            OnProgramStart.Initialize("ProjectUGC", "300505", "w1SSeahiZOMIU1K7oxNRJSALgCMtVLxermM", "1.0");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
        }
    }
}
