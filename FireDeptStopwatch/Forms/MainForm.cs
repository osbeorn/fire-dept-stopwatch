using FireDeptStopwatch.Classes;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.IO.IsolatedStorage;
using System.Media;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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

            //PrepareDataFile();

            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");

            resultList = new List<TimerResult>();

            globalHook = Hook.GlobalEvents();
            globalHook.MouseDownExt += GlobalHook_MouseDownExt;

            //deviceHandler = new RawInputDevices(Handle);
        }

        private void PrepareDataFile()
        {
            using (var appScope = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream("results.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, appScope))
                {
                    // do nothing
                }
            }
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

            await Task.Factory.StartNew(() => PlaySound(Properties.Resources.ssv_startno_povelje, true));

            resetButton.Enabled = true;

            start = DateTime.Now;
            stopwatchTimer.Start();
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
            var sounds = new List<KeyValuePair<UnmanagedMemoryStream, double>>
            {
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_air_horn, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_cuckoo_clock, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_freeze, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_ship_bell, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_train_whistle, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_ufo, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_bike_horn, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_party_horn, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_get_to_da_choppa, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_get_down, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_its_a_cookbook, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1_alt_its_over_9000, 1.0d/12.0d
                ),
                new KeyValuePair<UnmanagedMemoryStream, double>(
                    Properties.Resources.ssv_zakljucek_1, 1.0d/12.0d
                ),
            };

            stopwatchTimer.Stop();

            var fifteenSecs = new TimeSpan(0, 0, 15);
            var fourteenSecs = new TimeSpan(0, 0, 14);
            var thirteenSecs = new TimeSpan(0, 0, 13);

            if (diff >= thirteenSecs && diff < fourteenSecs)
            {
                PlaySound(Properties.Resources.ssv_zakljucek_1_alt_hallelujah, false);
            }
            else if (diff >= fourteenSecs && diff < fifteenSecs)
            {
                PlaySound(Properties.Resources.ssv_zakljucek_1_alt_na_golici_10s_cut, false);
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
            if (Debugger.IsAttached)
            {
                // project file for debugging
                try
                {
                    using (var stream = File.Open("Data/results.bin", FileMode.Create))
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
            else
            {
                // isolated storage
                try
                {
                    using (var appScope = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var stream = new IsolatedStorageFileStream("results.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, appScope))
                        {
                            BinaryFormatter bin = new BinaryFormatter();
                            bin.Serialize(stream, resultList);
                        }
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }
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
            var controls = new List<Control>()
            {
                preparationButton,
                startButton,
                resetButton
            };

            var controlClickPoint = mainPanel.PointToClient(clickPoint);
            foreach (var control in controls)
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

            if (Debugger.IsAttached)
            {
                // project file for debugging
                try
                {
                    using (var stream = File.Open("Data/results.bin", FileMode.Open))
                    {
                        var binaryFormatter = new BinaryFormatter();
                        resultList = (List<TimerResult>)binaryFormatter.Deserialize(stream);
                        resultList.Sort((x, y) => y.DateTime.CompareTo(x.DateTime));

                        foreach (var result in resultList)
                        {
                            resultsListBox.Items.Add(result);
                        }
                    }
                }
                catch (Exception)
                {
                    resultList = new List<TimerResult>();
                }
            }
            else
            {
                // isolated storage
                try
                {
                    using (var appScope = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var stream = new IsolatedStorageFileStream("results.bin", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, appScope))
                        {
                            var binaryFormatter = new BinaryFormatter();
                            resultList = (List<TimerResult>) binaryFormatter.Deserialize(stream);
                            resultList.Sort((x, y) => y.DateTime.CompareTo(x.DateTime));

                            foreach (var result in resultList)
                            {
                                resultsListBox.Items.Add(result);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    resultList = new List<TimerResult>();
                }
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
                        stopwatchLabel.Text = timerResult.GetEndResult().ToString(@"mm\:ss\.ffff");
                    }
                }
                else
                {
                    timerResult.Penalties = 0;
                }

                resultList.Insert(0, timerResult);
                resultsListBox.Items.Insert(0, timerResult);

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

            var selectedItems = resultsListBox.SelectedItems;

            if (selectedItems.Count == 0)
                return;

            for (var i = selectedItems.Count - 1; i >= 0; i--)
            {
                resultList.Remove(selectedItems[i] as TimerResult);
                resultsListBox.Items.Remove(selectedItems[i]);
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
            resultsListBox.Items.Clear();

            SaveResults();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resultsListBox.SelectedItems.Count > 0)
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

        private void ResultsListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                resultsListBox.SelectedIndex = resultsListBox.IndexFromPoint(e.Location);
                if (resultsListBox.SelectedIndex != -1)
                {
                    resultsContextMenuStrip.Tag = resultsListBox.SelectedItem;
                    resultsContextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void EditResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var timerResult = resultsContextMenuStrip.Tag as TimerResult;

            var penaltiesForm = new PenaltiesForm(timerResult.Penalties);
            if (penaltiesForm.ShowDialog(this) == DialogResult.OK)
            {
                timerResult.Penalties = penaltiesForm.ReturnValue;

                // workaround for updating an item in a listbox
                var index = resultsListBox.SelectedIndex;
                resultsListBox.Items.RemoveAt(resultsListBox.SelectedIndex);
                resultsListBox.Items.Insert(index, timerResult);
            }
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
