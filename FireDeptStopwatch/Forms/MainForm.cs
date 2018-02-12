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
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using FireDeptStopwatch.Helpers;
using System.Deployment.Application;

namespace FireDeptStopwatch.Forms
{
    public partial class MainForm : Form/*, IMessageFilter*/
    {
        private DateTime start;
        private DateTime current;
        private TimeSpan diff;

        private List<SplitTimeResult> splitTimes;
        private DateTime? lastSplitTime = null;
        private SplitTimesForm splitTimesForm;

        private List<TimerResult> resultList;
        private IKeyboardMouseEvents globalHook;

        private SoundPlayer player;

        private int lineupCounter;
        private int preparationCounter;

        private int preparationTime;
        private bool inputPenalties;
        private bool recordSplitTimes;
        private CountryCode country;
        private bool recording;
        //private string videosFolder;
        private List<CameraInfo> cameras;
        private bool endLineup;

        private List<VideoRecorder> videoRecorders;

        //private RawInputDevices deviceHandler;
        private CoreAudioController audioController;
        private List<String> mutedSessions;

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

        public bool GetRecordSplitTimes()
        {
            return recordSplitTimes;
        }

        public void SetRecordSplitTimes(bool recordSplitTimes)
        {
            this.recordSplitTimes = recordSplitTimes;
        }

        public CountryCode GetCountry()
        {
            return country;
        }

        public void SetCountry(CountryCode country)
        {
            this.country = country;
        }

        public bool GetRecordVideos()
        {
            return recording;
        }

        public void SetRecordVideos(bool recording)
        {
            this.recording = recording;

            this.webcamStatusPictureBox.Visible = this.recording;
        }

        //public string GetVideosFolder()
        //{
        //    return videosFolder;
        //}

        //public void SetVideosFolder(string videosFolder)
        //{
        //    this.videosFolder = videosFolder;
        //}

        public List<CameraInfo> GetCameras()
        {
            return cameras;
        }

        public void SetCameras(List<CameraInfo> cameraUrls)
        {
            this.cameras = cameraUrls;
        }

        public bool GetEndLineup()
        {
            return endLineup;
        }

        public void SetEndLineup(bool endLineup)
        {
            this.endLineup = endLineup;
        }

        private void InitializeComponents()
        {
            //Application.AddMessageFilter(this);

            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");

            splitTimes = new List<SplitTimeResult>();

            resultList = new List<TimerResult>();

            globalHook = Hook.GlobalEvents();
            globalHook.MouseDownExt += GlobalHook_MouseDownExt;
            globalHook.KeyDown += GlobalHook_KeyDown;

            //deviceHandler = new RawInputDevices(Handle);

            audioController = new CoreAudioController();
            mutedSessions = new List<string>();
        }

        private void PlaySound(UnmanagedMemoryStream sound, bool sync)
        {
            player = new SoundPlayer(sound);
            if (sync)
                player.PlaySync();
            else
                player.Play();
        }

        private async void StartTimer()
        {
            startButton.Enabled = false;
            preparationButton.Enabled = false;

            ClearSplitTimes();

            MuteApplications();

            StartVideoRecorders();

            stopwatchLabel.Text = new TimeSpan().ToString(@"mm\:ss\.ffff");

            switch (country)
            {
                case CountryCode.AT:
                    await Task.Factory.StartNew(() => PlaySound(Properties.Resources.ssv_startno_povelje_AT, true));
                    break;
                case CountryCode.HR:
                    await Task.Factory.StartNew(() => PlaySound(Properties.Resources.ssv_startno_povelje_HR, true));
                    break;
                case CountryCode.SI:
                default:
                    await Task.Factory.StartNew(() => PlaySound(Properties.Resources.ssv_startno_povelje_SI, true));
                    break;
            }

            start = DateTime.Now;
            lastSplitTime = null;

            resetButton.Enabled = true;

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

            if (player != null)
            {
                player.Stop();
            }

            UnmuteApplications();

            ClearSplitTimes();
        }

        private void ClearSplitTimes()
        {
            if (splitTimes != null)
            {
                splitTimes = new List<SplitTimeResult>();
            }

            if (recordSplitTimes && splitTimesForm != null)
            {
                splitTimesForm.ClearSplitTimes();
            }

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
                )
            };

            stopwatchTimer.Stop();

            var twelveAndHalfSecs = new TimeSpan(0, 0, 0, 12, 500);
            var twelveSecs = new TimeSpan(0, 0, 12);
            var thirteenSecs = new TimeSpan(0, 0, 13);
            var fourteenSecs = new TimeSpan(0, 0, 14);
            var fifteenSecs = new TimeSpan(0, 0, 15);

            if (diff >= twelveSecs && diff < twelveAndHalfSecs)
            {
                PlaySound(Properties.Resources.ssv_zakljucek_1_alt_sok, false);
            }
            else if (diff >= twelveAndHalfSecs && diff < thirteenSecs)
            {
                PlaySound(Properties.Resources.ssv_zakljucek_1_alt_sparta, false);
            }
            else if (diff >= thirteenSecs && diff < fourteenSecs)
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

        private void LogSplitTime()
        {
            TimeSpan fromStart = current - start;
            TimeSpan fromPrevious;
            if (!lastSplitTime.HasValue)
                fromPrevious = current - start;
            else
                fromPrevious = current - this.lastSplitTime.Value;

            var splitTimeResult = new SplitTimeResult
            {
                FromPrevious = fromPrevious,
                FromStart = fromStart
            };
            splitTimes.Add(splitTimeResult);

            lastSplitTime = current;

            if (splitTimesForm != null)
                splitTimesForm.AddSplitTime(splitTimeResult);
        }

        private void SaveResults()
        {
            if (Debugger.IsAttached)
            {
                try
                {
                    using (var stream = File.Open(Path.Combine("Data", "results.bin"), FileMode.Create))
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
                try
                {
                    using (var stream = File.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FireDeptStopwatch", "Data", "results.bin"), FileMode.Create))
                    {
                        BinaryFormatter bin = new BinaryFormatter();
                        bin.Serialize(stream, resultList);
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
                //preparationButton,
                //startButton,
                resetButton
            };

            var controlClickPoint = mainPanel.PointToClient(clickPoint);
            foreach (var control in controls)
            {
                if (control.Bounds.Contains(controlClickPoint))
                    return true;
            }

            return false;
        }

        private void DeleteResult()
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

        private void MuteApplications()
        {
            mutedSessions = new List<string>();

            var sessions = audioController.DefaultPlaybackDevice.GetCapability<IAudioSessionController>();
            foreach (var session in sessions)
            {
                if (session.IsSystemSession || session.IsMuted || (session.ExecutablePath != null && session.ExecutablePath.Contains("FireDeptStopwatch")))
                    continue;

                session.SetMuteAsync(true);
                mutedSessions.Add(session.Id);
            }
        }

        private void UnmuteApplications()
        {
            var sessions = audioController.DefaultPlaybackDevice.GetCapability<IAudioSessionController>();
            foreach (var session in sessions)
            {
                if (mutedSessions.Contains(session.Id))
                    session.SetMuteAsync(false);
            }
        }

        private void StartVideoRecorders()
        {
            if (recording)
            {
                videoRecorders.ForEach(
                    vr =>
                    {
                        vr.StartRecording();
                    }
                );
            }
        }

        private void StopVideoRecorders(TimerResult result)
        {
            if (recording)
            {
                videoRecorders.ForEach(
                    vr =>
                    {
                        vr.StopRecording(result);
                    }
                );
            }
        }

        private void ShowSplitTimesForm()
        {
            if (splitTimesForm == null)
            {
                splitTimesForm = new SplitTimesForm();
                splitTimesForm.Location = new Point(this.Location.X + this.Bounds.Width, this.Location.Y);
                splitTimesForm.Show();
            }
        }

        private void HideSplitTimesForm()
        {
            if (splitTimesForm != null)
            {
                splitTimesForm.Close();
                splitTimesForm = null;
            }
        }

        private List<CameraInfo> DelimitedStringToCameraInfoList(string delimitedString)
        {
            if (delimitedString == null || delimitedString == "")
            {
                return new List<CameraInfo>();
            }

            var cameras = new List<CameraInfo>();
            foreach (var cameraString in delimitedString.Split(';')) {
                var camera = new CameraInfo();

                if (cameraString.Contains("@"))
                {
                    camera.Url = cameraString.Substring(cameraString.IndexOf("@") + 1);

                    var credentials = cameraString.Substring(0, cameraString.IndexOf("@")).Replace("http://", "");
                    camera.User = credentials.Split(':')[0];
                    camera.Password = credentials.Split(':')[1];
                }
                else
                {
                    camera.Url = cameraString;
                }

                cameras.Add(camera);
            }

            return cameras;
        }

        private List<string> DelimitedStringToStringList(string delimitedString)
        {
            if (delimitedString == null || delimitedString == "")
            {
                return new List<string>();
            }

            return new List<string>(delimitedString.Split(';'));
        }

        #region Event handlers

        private void StartButton_Click(object sender, EventArgs e)
        {
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
            if (IsClickOnControl(e.Location))
            {
                return;
            }

            if (stopwatchTimer.Enabled && e.Button.Equals(MouseButtons.Left))
            {
                EndTimerAndLogResult();
            }
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (recordSplitTimes && stopwatchTimer.Enabled && e.KeyCode == Keys.Space)
            {
                LogSplitTime();
                e.Handled = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //deviceHandler = new RawInputDevices(this.Handle);

            string appDataFolder;
            if (Debugger.IsAttached)
            {
                appDataFolder = String.Empty;
            }
            else
            {
                appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FireDeptStopwatch");
            }

            var targetPath = Path.Combine(appDataFolder, "Data");
            Directory.CreateDirectory(targetPath);

            try
            {
                using (var stream = File.Open(Path.Combine(appDataFolder, "Data", "results.bin"), FileMode.OpenOrCreate))
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

            ExeConfigurationFileMap configurationFileMap = new ExeConfigurationFileMap {
                ExeConfigFilename = Path.Combine(appDataFolder, "FireDeptStopwatch.config")
            };
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configurationFileMap, ConfigurationUserLevel.None);

            preparationTime = Int32.Parse(configuration.AppSettings.Settings["preparationTime"].Value);
            inputPenalties = Boolean.Parse(configuration.AppSettings.Settings["inputPenalties"].Value);
            recordSplitTimes = Boolean.Parse(configuration.AppSettings.Settings["recordSplitTimes"].Value);
            country = (CountryCode) Enum.Parse(typeof(CountryCode), configuration.AppSettings.Settings["country"].Value);
            recording = Boolean.Parse(configuration.AppSettings.Settings["recordVideos"].Value);

            //videosFolder = configuration.AppSettings.Settings["videosFolder"].Value;
            targetPath = Path.Combine(appDataFolder, "TempRecordings");
            Directory.CreateDirectory(targetPath);

            targetPath = Path.Combine(appDataFolder, "Recordings");
            Directory.CreateDirectory(targetPath);

            cameras = DelimitedStringToCameraInfoList(configuration.AppSettings.Settings["cameras"].Value);

            endLineup = Boolean.Parse(configuration.AppSettings.Settings["endLineup"].Value);

            videoRecorders = new List<VideoRecorder>();
            for (var i = 0; i < cameras.Count; i++)
            {
                var videoRecorder = new VideoRecorder(cameras[i]);
                videoRecorder.OnConnect += OnVideoCameraConnect;
                videoRecorders.Add(videoRecorder);
            }

            if (recordSplitTimes)
            {
                ShowSplitTimesForm();
            }

            this.webcamStatusPictureBox.Visible = recording;
        }

        private void OnVideoCameraConnect(object sender, EventArgs e)
        {
            webcamStatusPictureBox.Image = FireDeptStopwatch.Properties.Resources.webcam_ok;
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
                PlaySound(Properties.Resources.ssv_priprava_orodja_zakljucek, false);
                lineupTimer.Stop();

                startButton.Enabled = true;
                preparationButton.Enabled = true;

                TimerResult timerResult = new TimerResult
                {
                    Result = current - start,
                    DateTime = start
                };
                if (splitTimes != null && splitTimes.Count > 0)
                    timerResult.SplitTimes = new List<SplitTimeResult>(splitTimes);

                StopVideoRecorders(timerResult);

                UnmuteApplications();

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
            DeleteResult();
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

        private void ResultListBox_SelectedIndexChanged(object sender, EventArgs e)
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

            PlaySound(Properties.Resources.ssv_priprava_orodja_zacetek, false);

            preparationTimer.Start();
        }

        private void PreparationTimer_Tick(object sender, EventArgs e)
        {
            if (preparationCounter == 0)
            {
                PlaySound(Properties.Resources.ssv_priprava_orodja_zakljucek, false);
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
            var result = preferencesForm.ShowDialog(this);

            if (result == DialogResult.OK) {
                // show / hide split times form
                if (recordSplitTimes)
                {
                    ShowSplitTimesForm();
                }
                else
                {
                    HideSplitTimesForm();
                }
            }
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
            else if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks == 2) // double click
                {
                    if (recordSplitTimes)
                    {
                        resultsListBox.SelectedIndex = resultsListBox.IndexFromPoint(e.Location);
                        if (resultsListBox.SelectedIndex != -1)
                        {
                            var selectedResult = resultsListBox.SelectedItem as TimerResult;
                            if (selectedResult.HasSplitTimes())
                            {
                                if (splitTimesForm == null)
                                {
                                    ShowSplitTimesForm();
                                }

                                splitTimesForm.SetSplitTimes(selectedResult.SplitTimes);
                            }
                        }
                    }
                }
            }
        }

        private void ResultsListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && resultsListBox.SelectedItems.Count > 0)
            {
                DeleteResult();
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

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (splitTimesForm != null && splitTimesForm.Visible)
                splitTimesForm.Location = new Point(this.Location.X + this.Bounds.Width, this.Location.Y);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SSV štoparica\n\n© 2016-2018 Benjamin Kastelic & PGD Zagradec pri Grosupljem", "O programu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("updater.exe", "/checknow");
        }

        private void ResultsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            var index = resultsListBox.IndexFromPoint(e.Location);
            if (index >= 0 && index < resultsListBox.Items.Count)
            {
                var item = resultsListBox.Items[index] as TimerResult;
                resultsToolTip.SetToolTip(resultsListBox, item.ToLongString());
            }
            else
            {
                resultsToolTip.SetToolTip(resultsListBox, String.Empty);
            }
        }

        #endregion
    }
}
