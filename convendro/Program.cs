using System;
using System.Collections.Generic;
using System.Windows.Forms;
using libconvendro;

namespace convendro
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Set arguments.
            CommandLineSession.Arguments = args;
            bool runui = true;

            if (runui) {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }
    }
}
