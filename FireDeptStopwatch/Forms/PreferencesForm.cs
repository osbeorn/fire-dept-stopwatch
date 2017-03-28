using FireDeptStopwatch.Classes;
using System;
using System.Configuration;
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

            countriesComboBox.ValueMember = "Code";
            countriesComboBox.DisplayMember = "Name";
            countriesComboBox.DataSource = Country.GetList();
            countriesComboBox.SelectedValue = parent.GetCountry();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["preparationTime"].Value = ((int)(preparationTimeNumericUpDown.Value * 60)).ToString();
            config.AppSettings.Settings["inputPenalties"].Value = inputPenaltiesCheckBox.Checked.ToString();
            config.AppSettings.Settings["country"].Value = countriesComboBox.SelectedValue.ToString();
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

            parent.SetPreparationTime((int) preparationTimeNumericUpDown.Value);
            parent.SetInputPenalties(inputPenaltiesCheckBox.Checked);
            parent.SetCountry((CountryCode) countriesComboBox.SelectedValue);

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
