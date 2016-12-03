using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FireDeptStopwatch.Forms
{
    public partial class PenaltiesForm : Form
    {
        public int ReturnValue { get; set; }

        public PenaltiesForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.penaltiesNumericUpDown.Focus();
            this.penaltiesNumericUpDown.Select(0, this.penaltiesNumericUpDown.Text.Length);
        }

        private void AddPenalties(int penalties)
        {
            if (this.penaltiesNumericUpDown.Value + penalties < 0)
            {
                this.penaltiesNumericUpDown.Value = 0;
                return;
            }

            if (this.penaltiesNumericUpDown.Value + penalties > this.penaltiesNumericUpDown.Maximum)
            {
                this.penaltiesNumericUpDown.Value = this.penaltiesNumericUpDown.Maximum;
                return;
            }

            this.penaltiesNumericUpDown.Value += penalties;
        }

        #region Event handlers

        private void MinusFiveButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(-5);
        }

        private void PlusFiveButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(5);
        }

        private void MinusTenButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(-10);
        }

        private void PlusTenButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(10);
        }

        private void MinusTwentyButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(-20);
        }

        private void PlusTwentyButton_Click(object sender, EventArgs e)
        {
            this.AddPenalties(20);
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            this.ReturnValue = (int) this.penaltiesNumericUpDown.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion Event handlers
    }
}
