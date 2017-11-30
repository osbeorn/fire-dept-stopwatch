using FireDeptStopwatch.Classes;
using FireDeptStopwatch.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class PreferencesForm : Form
    {
        private MainForm parent;

        private bool recordVideos;
        private List<CameraInfo> cameras;

        public PreferencesForm()
        {
            InitializeComponent();
        }

        public void InitializeComponents(MainForm parent)
        {
            this.parent = parent;

            preparationTimeNumericUpDown.Value = parent.GetPreparationTime();

            inputPenaltiesCheckBox.Checked = parent.GetInputPenalties();
            recordSplitTimesCheckBox.Checked = parent.GetRecordSplitTimes();

            countriesComboBox.ValueMember = "Code";
            countriesComboBox.DisplayMember = "Name";
            countriesComboBox.DataSource = Country.GetList();
            countriesComboBox.SelectedValue = parent.GetCountry();

            recordVideos = parent.GetRecordVideos();
            recordVideosCheckBox.Checked = recordVideos;

            //videosFolderTextBox.Text = parent.GetVideosFolder();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                videosFolderTextBox.Text = Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "Recordings");
            }
            else
            {
                videosFolderTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Recordings");
            }

            cameras = parent.GetCameras();
            foreach (var camera in cameras)
            {
                cameraUrlsListBox.Items.Add(camera);
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["preparationTime"].Value = ((int)(preparationTimeNumericUpDown.Value * 60)).ToString();
            config.AppSettings.Settings["inputPenalties"].Value = inputPenaltiesCheckBox.Checked.ToString();
            config.AppSettings.Settings["recordSplitTimes"].Value = recordSplitTimesCheckBox.Checked.ToString();
            config.AppSettings.Settings["country"].Value = countriesComboBox.SelectedValue.ToString();
            config.AppSettings.Settings["recordVideos"].Value = recordVideosCheckBox.Checked.ToString();
            //config.AppSettings.Settings["videosFolder"].Value = videosFolderTextBox.Text;
            config.AppSettings.Settings["cameras"].Value = CameraInfoListToDelimitedString(cameras);
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

            parent.SetPreparationTime((int) preparationTimeNumericUpDown.Value);
            parent.SetInputPenalties(inputPenaltiesCheckBox.Checked);
            parent.SetRecordSplitTimes(recordSplitTimesCheckBox.Checked);
            parent.SetCountry((CountryCode) countriesComboBox.SelectedValue);
            parent.SetRecordVideos(recordVideosCheckBox.Checked);
            //parent.SetVideosFolder(videosFolderTextBox.Text);
            parent.SetCameras(cameras);

            DialogResult = DialogResult.OK;
            Close();
        }

        private string CameraInfoListToDelimitedString(List<CameraInfo> cameras)
        {
            if (cameras == null)
            {
                return "";
            }

            var cameraUrls = new List<string>();
            foreach (var camera in cameras)
            {
                cameraUrls.Add(camera.ToString());
            }

            return String.Join(";", cameraUrls);
        }

        private string StringListToDelimitedString(List<string> stringList)
        {
            if (stringList == null)
            {
                return "";
            }

            return String.Join(";", stringList);
        }

        private void AddCameraButton_Click(object sender, EventArgs e)
        {
            var addCameraForm = new AddCameraForm();
            if (addCameraForm.ShowDialog(this) == DialogResult.OK)
            {
                var cameraInfo = addCameraForm.ReturnValue;

                var streamUri = CameraHelper.GetStreamUri(cameraInfo);
                if (streamUri == null)
                {
                    MessageBox.Show("Ni mogoče vzpostaviti povezave s kamero.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cameras.Add(cameraInfo);

                cameraUrlsListBox.Items.Add(cameraInfo);
                cameraUrlsListBox.Update();
            }
        }

        private void CameraUrlsListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2) // double click
                {
                    cameraUrlsListBox.SelectedIndex = cameraUrlsListBox.IndexFromPoint(e.Location);
                    if (cameraUrlsListBox.SelectedIndex != -1)
                    {
                        var selectedResult = cameraUrlsListBox.SelectedItem as CameraInfo;
                        if (selectedResult != null)
                        {
                            new CameraDisplayForm(selectedResult).Show();
                        }
                    }
                }
            }
        }

        private void CameraUrlsListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && cameraUrlsListBox.SelectedItems.Count > 0)
            {
                string message = "Ali res želite brisati izbrane kamere?";
                string caption = "Briši";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;

                DialogResult result = MessageBox.Show(message, caption, buttons);
                if (!result.Equals(DialogResult.Yes))
                    return;

                var selectedItems = cameraUrlsListBox.SelectedItems;
                if (selectedItems.Count == 0)
                    return;

                for (var i = selectedItems.Count - 1; i >= 0; i--)
                {
                    cameras.Remove(selectedItems[i] as CameraInfo);
                    cameraUrlsListBox.Items.Remove(selectedItems[i]);
                }
            }
        }

        private void VideosFolderSelectButton_Click(object sender, EventArgs e)
        {
            //if (videosFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            //{
            //    videosFolderTextBox.Text = videosFolderBrowserDialog.SelectedPath;
            //}

            string targetPath;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                targetPath = Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "Recordings");
            }
            else
            {
                targetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Recordings");
            }

            Process.Start(targetPath);
        }
    }
}
