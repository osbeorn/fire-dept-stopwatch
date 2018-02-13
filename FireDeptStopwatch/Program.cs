using FireDeptStopwatch.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireDeptStopwatch
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{2963d2b2-e086-4db1-b4d2-9782dfe2d132}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("SSV štoparica je že zagnana.");
            }
        }
    }
}
