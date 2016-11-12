using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class PreferencesForm : Form
    {
        private MainForm parent;

        public PreferencesForm()
        {
            InitializeComponent();
        }

        public void InitializeComponents(MainForm parent)
        {
            this.parent = parent;

            preparationTimeNumericUpDown.Value = parent.GetPreparationTime();
            inputPenaltiesCheckBox.Checked = parent.GetInputPenalties();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["preparationTime"].Value = ((int)(preparationTimeNumericUpDown.Value * 60)).ToString();
            config.AppSettings.Settings["inputPenalties"].Value = inputPenaltiesCheckBox.Checked.ToString(); ;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

            parent.SetPreparationTime((int) preparationTimeNumericUpDown.Value);
            parent.SetInputPenalties(inputPenaltiesCheckBox.Checked);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
