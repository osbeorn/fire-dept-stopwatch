namespace FireDeptStopwatch.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.stopwatchTimer = new System.Windows.Forms.Timer(this.components);
            this.lineupTimer = new System.Windows.Forms.Timer(this.components);
            this.mainPanel = new System.Windows.Forms.Panel();
            this.logoPanel = new System.Windows.Forms.Panel();
            this.webcamStatusPictureBox = new System.Windows.Forms.PictureBox();
            this.preparationLabel = new System.Windows.Forms.Label();
            this.lineupLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.stopwatchLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.preparationButton = new System.Windows.Forms.Button();
            this.resultListGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteAllResultsButton = new System.Windows.Forms.Button();
            this.deleteResultButton = new System.Windows.Forms.Button();
            this.resultsListBox = new System.Windows.Forms.ListBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.preparationTimer = new System.Windows.Forms.Timer(this.components);
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel.SuspendLayout();
            this.logoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webcamStatusPictureBox)).BeginInit();
            this.resultListGroupBox.SuspendLayout();
            this.mainMenuStrip.SuspendLayout();
            this.resultsContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopwatchTimer
            // 
            this.stopwatchTimer.Interval = 1;
            this.stopwatchTimer.Tick += new System.EventHandler(this.StopwatchTimer_Tick);
            // 
            // lineupTimer
            // 
            this.lineupTimer.Interval = 1000;
            this.lineupTimer.Tick += new System.EventHandler(this.LineupTimer_Tick);
            // 
            // mainPanel
            // 
            this.mainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainPanel.Controls.Add(this.logoPanel);
            this.mainPanel.Controls.Add(this.preparationButton);
            this.mainPanel.Controls.Add(this.resultListGroupBox);
            this.mainPanel.Controls.Add(this.resetButton);
            this.mainPanel.Controls.Add(this.startButton);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 24);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(728, 457);
            this.mainPanel.TabIndex = 1;
            // 
            // logoPanel
            // 
            this.logoPanel.BackgroundImage = global::FireDeptStopwatch.Properties.Resources.logo_background;
            this.logoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPanel.Controls.Add(this.webcamStatusPictureBox);
            this.logoPanel.Controls.Add(this.preparationLabel);
            this.logoPanel.Controls.Add(this.lineupLabel);
            this.logoPanel.Controls.Add(this.label2);
            this.logoPanel.Controls.Add(this.stopwatchLabel);
            this.logoPanel.Controls.Add(this.label1);
            this.logoPanel.Location = new System.Drawing.Point(221, 3);
            this.logoPanel.Name = "logoPanel";
            this.logoPanel.Size = new System.Drawing.Size(504, 400);
            this.logoPanel.TabIndex = 6;
            // 
            // webcamStatusPictureBox
            // 
            this.webcamStatusPictureBox.Image = global::FireDeptStopwatch.Properties.Resources.webcam_nok;
            this.webcamStatusPictureBox.Location = new System.Drawing.Point(461, 3);
            this.webcamStatusPictureBox.Name = "webcamStatusPictureBox";
            this.webcamStatusPictureBox.Size = new System.Drawing.Size(40, 40);
            this.webcamStatusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.webcamStatusPictureBox.TabIndex = 6;
            this.webcamStatusPictureBox.TabStop = false;
            // 
            // preparationLabel
            // 
            this.preparationLabel.AutoSize = true;
            this.preparationLabel.BackColor = System.Drawing.Color.Transparent;
            this.preparationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.preparationLabel.Location = new System.Drawing.Point(372, 88);
            this.preparationLabel.Name = "preparationLabel";
            this.preparationLabel.Size = new System.Drawing.Size(41, 44);
            this.preparationLabel.TabIndex = 5;
            this.preparationLabel.Text = "0";
            // 
            // lineupLabel
            // 
            this.lineupLabel.AutoSize = true;
            this.lineupLabel.BackColor = System.Drawing.Color.Transparent;
            this.lineupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lineupLabel.Location = new System.Drawing.Point(372, 274);
            this.lineupLabel.Name = "lineupLabel";
            this.lineupLabel.Size = new System.Drawing.Size(41, 44);
            this.lineupLabel.TabIndex = 1;
            this.lineupLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(15, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(351, 44);
            this.label2.TabIndex = 3;
            this.label2.Text = "Priprava orodja: ";
            // 
            // stopwatchLabel
            // 
            this.stopwatchLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.stopwatchLabel.BackColor = System.Drawing.Color.Transparent;
            this.stopwatchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stopwatchLabel.Location = new System.Drawing.Point(7, 156);
            this.stopwatchLabel.Name = "stopwatchLabel";
            this.stopwatchLabel.Size = new System.Drawing.Size(488, 93);
            this.stopwatchLabel.TabIndex = 0;
            this.stopwatchLabel.Text = "00:00:0000";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(15, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "Končna postavitev: ";
            // 
            // preparationButton
            // 
            this.preparationButton.Location = new System.Drawing.Point(342, 410);
            this.preparationButton.Name = "preparationButton";
            this.preparationButton.Size = new System.Drawing.Size(86, 35);
            this.preparationButton.TabIndex = 4;
            this.preparationButton.Text = "Priprava orodja";
            this.preparationButton.UseVisualStyleBackColor = true;
            this.preparationButton.Click += new System.EventHandler(this.PreparationButton_Click);
            // 
            // resultListGroupBox
            // 
            this.resultListGroupBox.Controls.Add(this.deleteAllResultsButton);
            this.resultListGroupBox.Controls.Add(this.deleteResultButton);
            this.resultListGroupBox.Controls.Add(this.resultsListBox);
            this.resultListGroupBox.Location = new System.Drawing.Point(-1, 3);
            this.resultListGroupBox.Name = "resultListGroupBox";
            this.resultListGroupBox.Size = new System.Drawing.Size(216, 456);
            this.resultListGroupBox.TabIndex = 0;
            this.resultListGroupBox.TabStop = false;
            this.resultListGroupBox.Text = "Rezultati";
            // 
            // deleteAllResultsButton
            // 
            this.deleteAllResultsButton.Location = new System.Drawing.Point(112, 407);
            this.deleteAllResultsButton.Name = "deleteAllResultsButton";
            this.deleteAllResultsButton.Size = new System.Drawing.Size(75, 35);
            this.deleteAllResultsButton.TabIndex = 2;
            this.deleteAllResultsButton.Text = "Briši vse";
            this.deleteAllResultsButton.UseVisualStyleBackColor = true;
            this.deleteAllResultsButton.Click += new System.EventHandler(this.DeleteAllResultsButton_Click);
            // 
            // deleteResultButton
            // 
            this.deleteResultButton.Enabled = false;
            this.deleteResultButton.Location = new System.Drawing.Point(31, 407);
            this.deleteResultButton.Name = "deleteResultButton";
            this.deleteResultButton.Size = new System.Drawing.Size(75, 35);
            this.deleteResultButton.TabIndex = 1;
            this.deleteResultButton.Text = "Briši";
            this.deleteResultButton.UseVisualStyleBackColor = true;
            this.deleteResultButton.Click += new System.EventHandler(this.DeleteResultButton_Click);
            // 
            // resultsListBox
            // 
            this.resultsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.resultsListBox.FormattingEnabled = true;
            this.resultsListBox.ItemHeight = 20;
            this.resultsListBox.Location = new System.Drawing.Point(6, 16);
            this.resultsListBox.Name = "resultsListBox";
            this.resultsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.resultsListBox.Size = new System.Drawing.Size(204, 384);
            this.resultsListBox.TabIndex = 0;
            this.resultsListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ResultsListBox_KeyUp);
            this.resultsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ResultsListBox_MouseDown);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.resetButton.Enabled = false;
            this.resetButton.Location = new System.Drawing.Point(515, 410);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 35);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // startButton
            // 
            this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.startButton.Location = new System.Drawing.Point(434, 410);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 35);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // preparationTimer
            // 
            this.preparationTimer.Interval = 1000;
            this.preparationTimer.Tick += new System.EventHandler(this.PreparationTimer_Tick);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(106)))), ((int)(((byte)(255)))), ((int)(((byte)(149)))));
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.analysisToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(728, 24);
            this.mainMenuStrip.TabIndex = 6;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.fileToolStripMenuItem.Text = "&Datoteka";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "&Izhod";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.editToolStripMenuItem.Text = "&Urejanje";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.preferencesToolStripMenuItem.Text = "&Nastavitve";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.PreferencesToolStripMenuItem_Click);
            // 
            // analysisToolStripMenuItem
            // 
            this.analysisToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graphsToolStripMenuItem});
            this.analysisToolStripMenuItem.Name = "analysisToolStripMenuItem";
            this.analysisToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.analysisToolStripMenuItem.Text = "&Analize";
            // 
            // graphsToolStripMenuItem
            // 
            this.graphsToolStripMenuItem.Name = "graphsToolStripMenuItem";
            this.graphsToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
            this.graphsToolStripMenuItem.Text = "&Grafi";
            this.graphsToolStripMenuItem.Click += new System.EventHandler(this.GraphsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.helpToolStripMenuItem.Text = "Pomoč";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.aboutToolStripMenuItem.Text = "O programu";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // resultsContextMenuStrip
            // 
            this.resultsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editResultToolStripMenuItem});
            this.resultsContextMenuStrip.Name = "resultsContextMenuStrip";
            this.resultsContextMenuStrip.ShowImageMargin = false;
            this.resultsContextMenuStrip.Size = new System.Drawing.Size(78, 26);
            // 
            // editResultToolStripMenuItem
            // 
            this.editResultToolStripMenuItem.Name = "editResultToolStripMenuItem";
            this.editResultToolStripMenuItem.Size = new System.Drawing.Size(77, 22);
            this.editResultToolStripMenuItem.Text = "Uredi";
            this.editResultToolStripMenuItem.Click += new System.EventHandler(this.EditResultToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 481);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.mainMenuStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SSV štoparica (PGD Zagradec pri Grosupljem)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.mainPanel.ResumeLayout(false);
            this.logoPanel.ResumeLayout(false);
            this.logoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webcamStatusPictureBox)).EndInit();
            this.resultListGroupBox.ResumeLayout(false);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.resultsContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer stopwatchTimer;
        private System.Windows.Forms.Timer lineupTimer;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.GroupBox resultListGroupBox;
        private System.Windows.Forms.ListBox resultsListBox;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lineupLabel;
        private System.Windows.Forms.Label stopwatchLabel;
        private System.Windows.Forms.Button deleteAllResultsButton;
        private System.Windows.Forms.Button deleteResultButton;
        private System.Windows.Forms.Button preparationButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label preparationLabel;
        private System.Windows.Forms.Timer preparationTimer;
        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphsToolStripMenuItem;
        private System.Windows.Forms.Panel logoPanel;
        private System.Windows.Forms.ContextMenuStrip resultsContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox webcamStatusPictureBox;
    }
}

