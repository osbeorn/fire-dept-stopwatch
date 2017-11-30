namespace FireDeptStopwatch.Forms
{
    partial class PreferencesForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.preparationTimeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.inputPenaltiesCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.recordSplitTimesCheckBox = new System.Windows.Forms.CheckBox();
            this.countriesComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.videosFolderSelectButton = new System.Windows.Forms.Button();
            this.videosFolderTextBox = new System.Windows.Forms.TextBox();
            this.addCameraButton = new System.Windows.Forms.Button();
            this.cameraUrlsListBox = new System.Windows.Forms.ListBox();
            this.recordVideosCheckBox = new System.Windows.Forms.CheckBox();
            this.videosFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.preparationTimeNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Čas za pripravo:";
            // 
            // preparationTimeNumericUpDown
            // 
            this.preparationTimeNumericUpDown.Location = new System.Drawing.Point(95, 19);
            this.preparationTimeNumericUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.preparationTimeNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.preparationTimeNumericUpDown.Name = "preparationTimeNumericUpDown";
            this.preparationTimeNumericUpDown.Size = new System.Drawing.Size(216, 20);
            this.preparationTimeNumericUpDown.TabIndex = 1;
            this.preparationTimeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(107, 363);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 2;
            this.confirmButton.Text = "Potrdi";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(188, 363);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Prekliči";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(317, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "min";
            // 
            // inputPenaltiesCheckBox
            // 
            this.inputPenaltiesCheckBox.AutoSize = true;
            this.inputPenaltiesCheckBox.Location = new System.Drawing.Point(9, 45);
            this.inputPenaltiesCheckBox.Name = "inputPenaltiesCheckBox";
            this.inputPenaltiesCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.inputPenaltiesCheckBox.Size = new System.Drawing.Size(125, 17);
            this.inputPenaltiesCheckBox.TabIndex = 6;
            this.inputPenaltiesCheckBox.Text = "Vnos kazenskih točk";
            this.inputPenaltiesCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.recordSplitTimesCheckBox);
            this.groupBox1.Controls.Add(this.countriesComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.preparationTimeNumericUpDown);
            this.groupBox1.Controls.Add(this.inputPenaltiesCheckBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 126);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Splošno";
            // 
            // recordSplitTimesCheckBox
            // 
            this.recordSplitTimesCheckBox.AutoSize = true;
            this.recordSplitTimesCheckBox.Location = new System.Drawing.Point(9, 68);
            this.recordSplitTimesCheckBox.Name = "recordSplitTimesCheckBox";
            this.recordSplitTimesCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recordSplitTimesCheckBox.Size = new System.Drawing.Size(146, 17);
            this.recordSplitTimesCheckBox.TabIndex = 9;
            this.recordSplitTimesCheckBox.Text = "Beleženje vmesnih časov";
            this.recordSplitTimesCheckBox.UseVisualStyleBackColor = true;
            // 
            // countriesComboBox
            // 
            this.countriesComboBox.FormattingEnabled = true;
            this.countriesComboBox.Location = new System.Drawing.Point(95, 91);
            this.countriesComboBox.Name = "countriesComboBox";
            this.countriesComboBox.Size = new System.Drawing.Size(245, 21);
            this.countriesComboBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Država:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.videosFolderSelectButton);
            this.groupBox2.Controls.Add(this.videosFolderTextBox);
            this.groupBox2.Controls.Add(this.addCameraButton);
            this.groupBox2.Controls.Add(this.cameraUrlsListBox);
            this.groupBox2.Controls.Add(this.recordVideosCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(346, 213);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Snemanje";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Posnetki";
            // 
            // videosFolderSelectButton
            // 
            this.videosFolderSelectButton.Location = new System.Drawing.Point(315, 41);
            this.videosFolderSelectButton.Name = "videosFolderSelectButton";
            this.videosFolderSelectButton.Size = new System.Drawing.Size(25, 22);
            this.videosFolderSelectButton.TabIndex = 11;
            this.videosFolderSelectButton.Text = "...";
            this.videosFolderSelectButton.UseVisualStyleBackColor = true;
            this.videosFolderSelectButton.Click += new System.EventHandler(this.VideosFolderSelectButton_Click);
            // 
            // videosFolderTextBox
            // 
            this.videosFolderTextBox.Location = new System.Drawing.Point(95, 42);
            this.videosFolderTextBox.Name = "videosFolderTextBox";
            this.videosFolderTextBox.ReadOnly = true;
            this.videosFolderTextBox.Size = new System.Drawing.Size(220, 20);
            this.videosFolderTextBox.TabIndex = 10;
            // 
            // addCameraButton
            // 
            this.addCameraButton.Location = new System.Drawing.Point(253, 183);
            this.addCameraButton.Name = "addCameraButton";
            this.addCameraButton.Size = new System.Drawing.Size(87, 23);
            this.addCameraButton.TabIndex = 9;
            this.addCameraButton.Text = "Dodaj kamero";
            this.addCameraButton.UseVisualStyleBackColor = true;
            this.addCameraButton.Click += new System.EventHandler(this.AddCameraButton_Click);
            // 
            // cameraUrlsListBox
            // 
            this.cameraUrlsListBox.FormattingEnabled = true;
            this.cameraUrlsListBox.Location = new System.Drawing.Point(6, 69);
            this.cameraUrlsListBox.Name = "cameraUrlsListBox";
            this.cameraUrlsListBox.Size = new System.Drawing.Size(334, 108);
            this.cameraUrlsListBox.TabIndex = 8;
            this.cameraUrlsListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CameraUrlsListBox_KeyUp);
            this.cameraUrlsListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraUrlsListBox_MouseDown);
            // 
            // recordVideosCheckBox
            // 
            this.recordVideosCheckBox.AutoSize = true;
            this.recordVideosCheckBox.Location = new System.Drawing.Point(9, 19);
            this.recordVideosCheckBox.Name = "recordVideosCheckBox";
            this.recordVideosCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recordVideosCheckBox.Size = new System.Drawing.Size(84, 17);
            this.recordVideosCheckBox.TabIndex = 7;
            this.recordVideosCheckBox.Text = "Omogočeno";
            this.recordVideosCheckBox.UseVisualStyleBackColor = true;
            // 
            // PreferencesForm
            // 
            this.AcceptButton = this.confirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(370, 398);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nastavitve";
            ((System.ComponentModel.ISupportInitialize)(this.preparationTimeNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown preparationTimeNumericUpDown;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox inputPenaltiesCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox countriesComboBox;
        private System.Windows.Forms.CheckBox recordSplitTimesCheckBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox recordVideosCheckBox;
        private System.Windows.Forms.ListBox cameraUrlsListBox;
        private System.Windows.Forms.Button addCameraButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button videosFolderSelectButton;
        private System.Windows.Forms.TextBox videosFolderTextBox;
        private System.Windows.Forms.FolderBrowserDialog videosFolderBrowserDialog;
    }
}