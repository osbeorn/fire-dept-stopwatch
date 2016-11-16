namespace FireDeptStopwatch.Forms
{
    partial class PenaltiesForm
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
            this.penaltiesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.minusFiveButton = new System.Windows.Forms.Button();
            this.minusTenButton = new System.Windows.Forms.Button();
            this.minusTwentyButton = new System.Windows.Forms.Button();
            this.plusFiveButton = new System.Windows.Forms.Button();
            this.plusTenButton = new System.Windows.Forms.Button();
            this.plusTwentyButton = new System.Windows.Forms.Button();
            this.confirmButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.penaltiesNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // penaltiesNumericUpDown
            // 
            this.penaltiesNumericUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.penaltiesNumericUpDown.Location = new System.Drawing.Point(105, 12);
            this.penaltiesNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.penaltiesNumericUpDown.Name = "penaltiesNumericUpDown";
            this.penaltiesNumericUpDown.Size = new System.Drawing.Size(173, 20);
            this.penaltiesNumericUpDown.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kazenske točke:";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(170, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 46);
            this.label2.TabIndex = 2;
            this.label2.Text = "5";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(159, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 46);
            this.label3.TabIndex = 3;
            this.label3.Text = "10";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(159, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 46);
            this.label4.TabIndex = 4;
            this.label4.Text = "20";
            // 
            // minusFiveButton
            // 
            this.minusFiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.minusFiveButton.Location = new System.Drawing.Point(100, 68);
            this.minusFiveButton.Name = "minusFiveButton";
            this.minusFiveButton.Size = new System.Drawing.Size(53, 34);
            this.minusFiveButton.TabIndex = 5;
            this.minusFiveButton.Text = "-";
            this.minusFiveButton.UseVisualStyleBackColor = true;
            this.minusFiveButton.Click += new System.EventHandler(this.MinusFiveButton_Click);
            // 
            // minusTenButton
            // 
            this.minusTenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.minusTenButton.Location = new System.Drawing.Point(100, 125);
            this.minusTenButton.Name = "minusTenButton";
            this.minusTenButton.Size = new System.Drawing.Size(53, 34);
            this.minusTenButton.TabIndex = 11;
            this.minusTenButton.Text = "-";
            this.minusTenButton.UseVisualStyleBackColor = true;
            this.minusTenButton.Click += new System.EventHandler(this.MinusTenButton_Click);
            // 
            // minusTwentyButton
            // 
            this.minusTwentyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.minusTwentyButton.Location = new System.Drawing.Point(100, 185);
            this.minusTwentyButton.Name = "minusTwentyButton";
            this.minusTwentyButton.Size = new System.Drawing.Size(53, 34);
            this.minusTwentyButton.TabIndex = 12;
            this.minusTwentyButton.Text = "-";
            this.minusTwentyButton.UseVisualStyleBackColor = true;
            this.minusTwentyButton.Click += new System.EventHandler(this.MinusTwentyButton_Click);
            // 
            // plusFiveButton
            // 
            this.plusFiveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.plusFiveButton.Location = new System.Drawing.Point(229, 68);
            this.plusFiveButton.Name = "plusFiveButton";
            this.plusFiveButton.Size = new System.Drawing.Size(53, 34);
            this.plusFiveButton.TabIndex = 13;
            this.plusFiveButton.Text = "+";
            this.plusFiveButton.UseVisualStyleBackColor = true;
            this.plusFiveButton.Click += new System.EventHandler(this.PlusFiveButton_Click);
            // 
            // plusTenButton
            // 
            this.plusTenButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.plusTenButton.Location = new System.Drawing.Point(229, 125);
            this.plusTenButton.Name = "plusTenButton";
            this.plusTenButton.Size = new System.Drawing.Size(53, 34);
            this.plusTenButton.TabIndex = 14;
            this.plusTenButton.Text = "+";
            this.plusTenButton.UseVisualStyleBackColor = true;
            this.plusTenButton.Click += new System.EventHandler(this.PlusTenButton_Click);
            // 
            // plusTwentyButton
            // 
            this.plusTwentyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.plusTwentyButton.Location = new System.Drawing.Point(229, 185);
            this.plusTwentyButton.Name = "plusTwentyButton";
            this.plusTwentyButton.Size = new System.Drawing.Size(53, 34);
            this.plusTwentyButton.TabIndex = 15;
            this.plusTwentyButton.Text = "+";
            this.plusTwentyButton.UseVisualStyleBackColor = true;
            this.plusTwentyButton.Click += new System.EventHandler(this.PlusTwentyButton_Click);
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(150, 257);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(75, 23);
            this.confirmButton.TabIndex = 16;
            this.confirmButton.Text = "Potrdi";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // PenaltiesForm
            // 
            this.AcceptButton = this.confirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 292);
            this.ControlBox = false;
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.plusTwentyButton);
            this.Controls.Add(this.plusTenButton);
            this.Controls.Add(this.plusFiveButton);
            this.Controls.Add(this.minusTwentyButton);
            this.Controls.Add(this.minusTenButton);
            this.Controls.Add(this.minusFiveButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.penaltiesNumericUpDown);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PenaltiesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vnos kazenskih točk";
            ((System.ComponentModel.ISupportInitialize)(this.penaltiesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown penaltiesNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button minusFiveButton;
        private System.Windows.Forms.Button minusTenButton;
        private System.Windows.Forms.Button minusTwentyButton;
        private System.Windows.Forms.Button plusFiveButton;
        private System.Windows.Forms.Button plusTenButton;
        private System.Windows.Forms.Button plusTwentyButton;
        private System.Windows.Forms.Button confirmButton;
    }
}