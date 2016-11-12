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
            this.panel2 = new System.Windows.Forms.Panel();
            this.preparationLabel = new System.Windows.Forms.Label();
            this.preparationButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.resultListGroupBox = new System.Windows.Forms.GroupBox();
            this.deleteAllResultsButton = new System.Windows.Forms.Button();
            this.deleteResultButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lineupLabel = new System.Windows.Forms.Label();
            this.stopwatchLabel = new System.Windows.Forms.Label();
            this.preparationTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            this.resultListGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
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
            // panel2
            // 
            this.panel2.Controls.Add(this.preparationLabel);
            this.panel2.Controls.Add(this.preparationButton);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.resultListGroupBox);
            this.panel2.Controls.Add(this.resetButton);
            this.panel2.Controls.Add(this.startButton);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lineupLabel);
            this.panel2.Controls.Add(this.stopwatchLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(728, 457);
            this.panel2.TabIndex = 1;
            // 
            // preparationLabel
            // 
            this.preparationLabel.AutoSize = true;
            this.preparationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.preparationLabel.Location = new System.Drawing.Point(583, 88);
            this.preparationLabel.Name = "preparationLabel";
            this.preparationLabel.Size = new System.Drawing.Size(41, 44);
            this.preparationLabel.TabIndex = 5;
            this.preparationLabel.Text = "0";
            // 
            // preparationButton
            // 
            this.preparationButton.Location = new System.Drawing.Point(342, 422);
            this.preparationButton.Name = "preparationButton";
            this.preparationButton.Size = new System.Drawing.Size(86, 23);
            this.preparationButton.TabIndex = 4;
            this.preparationButton.Text = "Priprava orodja";
            this.preparationButton.UseVisualStyleBackColor = true;
            this.preparationButton.Click += new System.EventHandler(this.PreparationButton_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(226, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(351, 44);
            this.label2.TabIndex = 3;
            this.label2.Text = "Priprava orodja: ";
            // 
            // resultListGroupBox
            // 
            this.resultListGroupBox.Controls.Add(this.deleteAllResultsButton);
            this.resultListGroupBox.Controls.Add(this.deleteResultButton);
            this.resultListGroupBox.Controls.Add(this.listBox1);
            this.resultListGroupBox.Location = new System.Drawing.Point(-1, 3);
            this.resultListGroupBox.Name = "resultListGroupBox";
            this.resultListGroupBox.Size = new System.Drawing.Size(216, 456);
            this.resultListGroupBox.TabIndex = 0;
            this.resultListGroupBox.TabStop = false;
            this.resultListGroupBox.Text = "Rezultati";
            // 
            // deleteAllResultsButton
            // 
            this.deleteAllResultsButton.Location = new System.Drawing.Point(112, 419);
            this.deleteAllResultsButton.Name = "deleteAllResultsButton";
            this.deleteAllResultsButton.Size = new System.Drawing.Size(75, 23);
            this.deleteAllResultsButton.TabIndex = 2;
            this.deleteAllResultsButton.Text = "Briši vse";
            this.deleteAllResultsButton.UseVisualStyleBackColor = true;
            this.deleteAllResultsButton.Click += new System.EventHandler(this.DeleteAllResultsButton_Click);
            // 
            // deleteResultButton
            // 
            this.deleteResultButton.Enabled = false;
            this.deleteResultButton.Location = new System.Drawing.Point(31, 419);
            this.deleteResultButton.Name = "deleteResultButton";
            this.deleteResultButton.Size = new System.Drawing.Size(75, 23);
            this.deleteResultButton.TabIndex = 1;
            this.deleteResultButton.Text = "Briši";
            this.deleteResultButton.UseVisualStyleBackColor = true;
            this.deleteResultButton.Click += new System.EventHandler(this.DeleteResultButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(6, 16);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(204, 388);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // resetButton
            // 
            this.resetButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.resetButton.Enabled = false;
            this.resetButton.Location = new System.Drawing.Point(515, 422);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 23);
            this.resetButton.TabIndex = 1;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // startButton
            // 
            this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.startButton.Location = new System.Drawing.Point(434, 422);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(226, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(351, 44);
            this.label1.TabIndex = 2;
            this.label1.Text = "Končna postavitev: ";
            // 
            // lineupLabel
            // 
            this.lineupLabel.AutoSize = true;
            this.lineupLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lineupLabel.Location = new System.Drawing.Point(583, 274);
            this.lineupLabel.Name = "lineupLabel";
            this.lineupLabel.Size = new System.Drawing.Size(41, 44);
            this.lineupLabel.TabIndex = 1;
            this.lineupLabel.Text = "0";
            // 
            // stopwatchLabel
            // 
            this.stopwatchLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.stopwatchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 60F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.stopwatchLabel.Location = new System.Drawing.Point(215, 157);
            this.stopwatchLabel.Name = "stopwatchLabel";
            this.stopwatchLabel.Size = new System.Drawing.Size(504, 93);
            this.stopwatchLabel.TabIndex = 0;
            this.stopwatchLabel.Text = "label1";
            // 
            // preparationTimer
            // 
            this.preparationTimer.Interval = 1000;
            this.preparationTimer.Tick += new System.EventHandler(this.PreparationTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(728, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 481);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SSV štoparica";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.resultListGroupBox.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer stopwatchTimer;
        private System.Windows.Forms.Timer lineupTimer;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox resultListGroupBox;
        private System.Windows.Forms.ListBox listBox1;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
    }
}

