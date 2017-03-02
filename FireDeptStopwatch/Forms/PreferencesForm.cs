using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
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

        bool PairDevice()
        {
            using (var discoverForm = new SelectBluetoothDeviceDialog())
            {
                if (discoverForm.ShowDialog(this) != DialogResult.OK)
                {
                    // no device selected
                    return false;
                }

                BluetoothDeviceInfo deviceInfo = discoverForm.SelectedDevice;

                if (!deviceInfo.Authenticated) // previously paired?
                {
                    // TODO: show a dialog with a PIN/discover the device PIN
                    if (!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress, "0000"))
                    {
                        // not previously paired and attempt to pair failed
                        return false;
                    }
                }

                // device should now be paired with the OS so make a connection to it asynchronously
                var client = new BluetoothClient();
                client.BeginConnect(deviceInfo.DeviceAddress, BluetoothService.SerialPort, this.BluetoothClientConnectCallback, client);

                return true;
            }
        }

        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            var client = (BluetoothClient) result.AsyncState;
            client.EndConnect(result);

            // get the client's stream and do whatever reading/writing you want to do.
            // if you want to maintain the connection then calls to Read() on the client's stream should block when awaiting data from the device

            // when you're done reading/writing and want to close the connection or the device servers the connection control flow will resume here and you need to tidy up

            client.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PairDevice();
        }
    }
}
