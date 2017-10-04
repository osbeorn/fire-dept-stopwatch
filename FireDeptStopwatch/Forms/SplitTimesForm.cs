using FireDeptStopwatch.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class SplitTimesForm : Form
    {
        [DllImport("user32.dll")]
        private extern static IntPtr SetActiveWindow(IntPtr handle);

        private MainForm parent;
        private List<SplitTimeResult> splitTimes;

        public SplitTimesForm()
        {
            InitializeComponent();
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_ACTIVATE = 6;
            const int WA_INACTIVE = 0;
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                //case WM_ACTIVATE: // prevent from activation
                //    if (((int) message.WParam & 0xFFFF) != WA_INACTIVE)
                //    {
                //        if (message.LParam != IntPtr.Zero)
                //        {
                //            SetActiveWindow(message.LParam);
                //        }
                //        else
                //        {
                //            // could not find sender, just in-activate it.
                //            SetActiveWindow(IntPtr.Zero);
                //        }
                //    }
                //    break;

                case WM_SYSCOMMAND: // prevent the form from being moved
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

        public void InitializeComponents(MainForm parent)
        {
            this.parent = parent;
        }

        public void SetSplitTimes(List<SplitTimeResult> splitTimes)
        {
            this.splitTimes = splitTimes;
            splitTimesListBox.DataSource = this.splitTimes;
        }

        public void AddSplitTime(SplitTimeResult splitTime)
        {
            splitTimesListBox.Items.Add(splitTime);
        }

        public void ClearSplitTimes()
        {
            splitTimes = null;
            splitTimesListBox.DataSource = null;
            splitTimesListBox.Items.Clear();
        }
    }
}
