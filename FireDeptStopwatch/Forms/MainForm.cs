using FireDeptStopwatch.Classes;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class MainForm : Form/*, IMessageFilter*/
    {
        private DateTime start;
        private DateTime current;
        private List<TimerResult> resultList;
        private IKeyboardMouseEvents globalHook;

        private int lineupCounter;
        private int preparationCounter;

        private int preparationTime;
        private bool inputPenalties;

        private bool resetTriggered { get; set; }

        //RawInputDevices deviceHandler;

        public MainForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        public int GetPreparationTime()
        {
            return preparationTime / 60;
        }

        public void SetPreparationTime(int preparationTime)
        {
            this.preparationTime = preparationTime * 60;
        }

        public bool GetInputPenalties()
        {
            return inputPenalties;
        }

        public void SetInputPenalties(bool inputPenalties)
        {
            this.inputPenalties = inputPenalties;
        }

        private void InitializeComponents()
        {
            //Application.AddMessageFilter(this);

            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");

            resultList = new List<TimerResult>();

            globalHook = Hook.GlobalEvents();
            globalHook.MouseDownExt += GlobalHook_MouseDownExt;

            //deviceHandler = new RawInputDevices(Handle);
        }

        private void PlaySound(UnmanagedMemoryStream sound, bool sync)
        {
            using (var player = new SoundPlayer(sound))
            {
                if (sync)
                    player.PlaySync();
                else
                    player.Play();
            }
        }

        private async void StartTimer()
        {
            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");
            resetButton.Enabled = false;

            await Task.Factory.StartNew(() => PlaySound(FireDeptStopwatch.Properties.Resources.ssv_startno_povelje_trimmed, true));

            resetButton.Enabled = true;

            start = DateTime.Now;
            stopwatchTimer.Start();

            Cursor.Position = new Point(
                Screen.PrimaryScreen.Bounds.Width / 2,
                Screen.PrimaryScreen.Bounds.Height / 2
            );
        }

        private void ResetTimers()
        {
            resetButton.Enabled = false;

            preparationTimer.Stop();
            preparationButton.Enabled = true;
            preparationLabel.Text = "0";

            stopwatchTimer.Stop();
            startButton.Enabled = true;
            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");

            lineupTimer.Stop();
            lineupLabel.Text = "0";
        }

        private void EndTimerAndLogResult()
        {
            PlaySound(FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1, false);

            stopwatchTimer.Stop();

            lineupCounter = 11;
            lineupTimer.Start();

            TimerResult timerResult = new TimerResult();
            timerResult.result = current - start;
            timerResult.dateTime = start;

            resultList.Insert(0, timerResult);

            listBox1.Items.Insert(0, timerResult);

            SaveResults();
        }

        private void SaveResults()
        {
            try
            {
                using (Stream stream = File.Open("Data/results.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, resultList);
                }
            }
            catch (IOException)
            {
                // do nothing
            }
        }

        //public bool PreFilterMessage(ref Message message)
        //{
        //    int hardId = deviceHandler.GetDeviceID(message);

        //    if (hardId > 0)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Device ID : " + hardId.ToString());
        //        //Return true here if you want to supress the mouse click
        //        //bear in mind that mouse down and up messages will still pass through, so you will need to filter these out and return true also.
        //    }

        //    return false;
        //}

        private bool IsClickOnControl(Point clickPoint)
        {
            List<Control> controls = new List<Control>()
            {
                preparationButton,
                startButton,
                resetButton
            };

            var controlClickPoint = panel2.PointToClient(clickPoint);
            foreach (Control control in controls)
            {
                //var controlBounds = panel2.RectangleToScreen(control.Bounds);

                if (control.Bounds.Contains(controlClickPoint))
                    return true;
            }

            return false;
        }

        #region Event handlers

        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            preparationButton.Enabled = false;

            StartTimer();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetTimers();
        }

        private void StopwatchTimer_Tick(object sender, EventArgs e)
        {
            current = DateTime.Now;

            var diff = current - start;
            stopwatchLabel.Text = diff.ToString(@"mm\:ss\.ffff");
        }

        private void GlobalHook_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (IsClickOnControl(e.Location))
            {
                System.Diagnostics.Debug.WriteLine("Click on control.");
                return;
            }


            if (stopwatchTimer.Enabled && e.Button.Equals(MouseButtons.Left))
            {
                EndTimerAndLogResult();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //deviceHandler = new RawInputDevices(this.Handle);

            try
            {
                using (Stream stream = File.Open("Data/results.bin", FileMode.Open))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    resultList = (List<TimerResult>) bin.Deserialize(stream);
                    resultList.Sort((x, y) => y.dateTime.CompareTo(x.dateTime));

                    foreach (TimerResult result in resultList)
                    {
                        listBox1.Items.Add(result);
                    }
                }
            }
            catch (Exception)
            {
                resultList = new List<TimerResult>();
            }

            preparationTime = Int32.Parse(ConfigurationManager.AppSettings["preparationTime"]);
            inputPenalties = Boolean.Parse(ConfigurationManager.AppSettings["inputPenalties"]);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Application.RemoveMessageFilter(this);

            SaveResults();
        }

        private void LineupTimer_Tick(object sender, EventArgs e)
        {
            if (lineupCounter == 0)
            {
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_zakljucek_2, false);
                lineupTimer.Stop();

                startButton.Enabled = true;
                preparationButton.Enabled = true;
            }
            else
            {
                lineupCounter--;
                lineupLabel.Text = lineupCounter.ToString();
            }
        }

        private void DeleteResultButton_Click(object sender, EventArgs e)
        {
            string message = "Ali res želite brisati izbrane rezultate?";
            string caption = "Briši";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (!result.Equals(DialogResult.Yes))
                return;

            var selectedItems = listBox1.SelectedItems;

            if (selectedItems.Count == 0)
                return;

            for (var i = selectedItems.Count - 1; i >= 0; i--)
            {
                resultList.Remove(selectedItems[i] as TimerResult);
                listBox1.Items.Remove(selectedItems[i]);
            }

            SaveResults();
        }

        private void DeleteAllResultsButton_Click(object sender, EventArgs e)
        {
            string message = "Ali res želite brisati vse rezultate?";
            string caption = "Briši vse";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            DialogResult result = MessageBox.Show(message, caption, buttons);

            if (!result.Equals(DialogResult.Yes))
                return;

            resultList.Clear();
            listBox1.Items.Clear();

            SaveResults();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
                deleteResultButton.Enabled = true;
            else
                deleteResultButton.Enabled = false;
        }

        private void PreparationButton_Click(object sender, EventArgs e)
        {
            preparationButton.Enabled = false;
            startButton.Enabled = false;

            preparationCounter = preparationTime;

            preparationTimer.Start();
        }

        private void PreparationTimer_Tick(object sender, EventArgs e)
        {
            if (preparationCounter == 0)
            {
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_zakljucek_2, false);
                preparationTimer.Stop();

                preparationButton.Enabled = true;
                startButton.Enabled = true;
            }
            else
            {
                preparationCounter--;
                preparationLabel.Text = preparationCounter.ToString();
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesForm preferencesForm = new PreferencesForm();
            preferencesForm.InitializeComponents(this);
            preferencesForm.ShowDialog(this);
        }

        #endregion
    }
}
