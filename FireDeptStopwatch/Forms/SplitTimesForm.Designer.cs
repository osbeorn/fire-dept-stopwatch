namespace FireDeptStopwatch.Forms
{
    partial class SplitTimesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitTimesForm));
            this.splitTimesListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // splitTimesListBox
            // 
            this.splitTimesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.splitTimesListBox.FormattingEnabled = true;
            this.splitTimesListBox.IntegralHeight = false;
            this.splitTimesListBox.ItemHeight = 20;
            this.splitTimesListBox.Location = new System.Drawing.Point(12, 12);
            this.splitTimesListBox.Name = "splitTimesListBox";
            this.splitTimesListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.splitTimesListBox.Size = new System.Drawing.Size(206, 457);
            this.splitTimesListBox.TabIndex = 0;
            // 
            // SplitTimesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 481);
            this.Controls.Add(this.splitTimesListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplitTimesForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Vmesni časi";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox splitTimesListBox;
    }
}