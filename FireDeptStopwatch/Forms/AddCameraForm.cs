using FireDeptStopwatch.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class AddCameraForm : Form
    {
        public CameraInfo ReturnValue { get; set; }

        public AddCameraForm()
        {
            InitializeComponent();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (this.urlTextBox.Text == null || this.urlTextBox.Text == "")
            {
                return;
            }

            this.ReturnValue = new CameraInfo
            {
                Url = this.urlTextBox.Text,
                User = this.userTextBox.Text,
                Password = this.passwordTextBox.Text
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
