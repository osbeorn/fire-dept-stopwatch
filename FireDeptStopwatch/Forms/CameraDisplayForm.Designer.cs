namespace FireDeptStopwatch.Forms
{
    partial class CameraDisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraDisplayForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.turnRightButton = new System.Windows.Forms.Button();
            this.turnLeftButton = new System.Windows.Forms.Button();
            this.turnDownButton = new System.Windows.Forms.Button();
            this.turnUpButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.turnRightButton);
            this.groupBox1.Controls.Add(this.turnLeftButton);
            this.groupBox1.Controls.Add(this.turnDownButton);
            this.groupBox1.Controls.Add(this.turnUpButton);
            this.groupBox1.Location = new System.Drawing.Point(738, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 99);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nastavitve";
            // 
            // turnRightButton
            // 
            this.turnRightButton.Location = new System.Drawing.Point(74, 42);
            this.turnRightButton.Name = "turnRightButton";
            this.turnRightButton.Size = new System.Drawing.Size(25, 25);
            this.turnRightButton.TabIndex = 5;
            this.turnRightButton.Text = "▶";
            this.turnRightButton.UseVisualStyleBackColor = true;
            this.turnRightButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TurnRightButton_MouseDown);
            this.turnRightButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TurnRightButton_MouseUp);
            // 
            // turnLeftButton
            // 
            this.turnLeftButton.Location = new System.Drawing.Point(12, 42);
            this.turnLeftButton.Name = "turnLeftButton";
            this.turnLeftButton.Size = new System.Drawing.Size(25, 25);
            this.turnLeftButton.TabIndex = 4;
            this.turnLeftButton.Text = "◀";
            this.turnLeftButton.UseVisualStyleBackColor = true;
            this.turnLeftButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TurnLeftButton_MouseDown);
            this.turnLeftButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TurnLeftButton_MouseUp);
            // 
            // turnDownButton
            // 
            this.turnDownButton.Location = new System.Drawing.Point(43, 57);
            this.turnDownButton.Name = "turnDownButton";
            this.turnDownButton.Size = new System.Drawing.Size(25, 25);
            this.turnDownButton.TabIndex = 3;
            this.turnDownButton.Text = "▼";
            this.turnDownButton.UseVisualStyleBackColor = true;
            this.turnDownButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TurnDownButton_MouseDown);
            this.turnDownButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TurnDownButton_MouseUp);
            // 
            // turnUpButton
            // 
            this.turnUpButton.Location = new System.Drawing.Point(43, 26);
            this.turnUpButton.Name = "turnUpButton";
            this.turnUpButton.Size = new System.Drawing.Size(25, 25);
            this.turnUpButton.TabIndex = 2;
            this.turnUpButton.Text = "▲";
            this.turnUpButton.UseVisualStyleBackColor = true;
            this.turnUpButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TurnUpButton_MouseDown);
            this.turnUpButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TurnUpButton_MouseUp);
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Image = global::FireDeptStopwatch.Properties.Resources.loader;
            this.pictureBox.InitialImage = global::FireDeptStopwatch.Properties.Resources.loader;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(720, 576);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.WaitOnLoad = true;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(775, 565);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Zapri";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // CameraDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 600);
            this.ControlBox = false;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kamera";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button turnRightButton;
        private System.Windows.Forms.Button turnLeftButton;
        private System.Windows.Forms.Button turnDownButton;
        private System.Windows.Forms.Button turnUpButton;
        private System.Windows.Forms.Button closeButton;
    }
}