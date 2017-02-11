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
        private TimeSpan diff;

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

        public List<TimerResult> GetResultsList()
        {
            return resultList;
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

            //Cursor.Position = new Point(
            //    Screen.PrimaryScreen.Bounds.Width / 2,
            //    Screen.PrimaryScreen.Bounds.Height / 2
            //);
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
            List<KeyValuePair<UnmanagedMemoryStream, double>> sounds = new List<KeyValuePair<UnmanagedMemoryStream, double>>
            {
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_air_horn, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_cuckoo_clock, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_freeze, 0.95d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_ship_bell, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_train_whistle, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_ufo, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_bike_horn, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_party_horn, 0.885714d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_get_to_da_choppa, 0.95d/9.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1, 0.1d
                ),
            };

            stopwatchTimer.Stop();

            var fifteenSecs = new TimeSpan(0, 0, 15);
            var fourteenSecs = new TimeSpan(0, 0, 14);
            var thirteenSecs = new TimeSpan(0, 0, 13);

            if (diff >= thirteenSecs && diff < fourteenSecs)
            {
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_hallelujah_2, false);
            }
            else if (diff >= fourteenSecs && diff < fifteenSecs)
            {
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_zakljucek_1_alt_na_golici_10s_cut, false);
            }
            else
            {
                Random r = new Random();
                double diceRoll = r.NextDouble();

                double cumulative = 0.0d;
                for (int i = 0; i < sounds.Count; i++)
                {
                    cumulative += sounds[i].Value;
                    if (diceRoll < cumulative)
                    {
                        PlaySound(sounds[i].Key, false);
                        break;
                    }
                }
            }

            lineupCounter = 11;
            lineupTimer.Start();
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

            var controlClickPoint = mainPanel.PointToClient(clickPoint);
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

            diff = current - start;
            stopwatchLabel.Text = diff.ToString(@"mm\:ss\.ffff");
        }

        private void GlobalHook_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            //if (IsClickOnControl(e.Location))
            //{
            //    return;
            //}


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
                    resultList.Sort((x, y) => y.DateTime.CompareTo(x.DateTime));

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
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_priprava_orodja_zakljucek, false);
                lineupTimer.Stop();

                startButton.Enabled = true;
                preparationButton.Enabled = true;

                TimerResult timerResult = new TimerResult();
                timerResult.Result = current - start;
                timerResult.DateTime = start;

                if (inputPenalties)
                {
                    var penaltiesForm = new PenaltiesForm();
                    if (penaltiesForm.ShowDialog(this) == DialogResult.OK)
                    {
                        timerResult.Penalties = penaltiesForm.ReturnValue;
                    }
                }
                else
                {
                    timerResult.Penalties = 0;
                }

                resultList.Insert(0, timerResult);
                listBox1.Items.Insert(0, timerResult);

                SaveResults();
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
            resetButton.Enabled = true;

            preparationCounter = preparationTime;

            PlaySound(FireDeptStopwatch.Properties.Resources.ssv_priprava_orodja_zacetek, false);

            preparationTimer.Start();
        }

        private void PreparationTimer_Tick(object sender, EventArgs e)
        {
            if (preparationCounter == 0)
            {
                PlaySound(FireDeptStopwatch.Properties.Resources.ssv_priprava_orodja_zakljucek, false);
                preparationTimer.Stop();

                preparationButton.Enabled = true;
                startButton.Enabled = true;
                resetButton.Enabled = false;
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

        private void GraphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnalysesForm graphsForm = new AnalysesForm();
            graphsForm.InitializeComponents(this);
            graphsForm.Show();
        }

        #endregion

        #region Overrides

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    var rc = new Rectangle(this.ClientSize.Width - backgroundImage.Width,
        //        this.ClientSize.Height - backgroundImage.Height,
        //        backgroundImage.Width, backgroundImage.Height);
        //    e.Graphics.DrawImage(backgroundImage, rc);
        //}

        #endregion
    }
}
