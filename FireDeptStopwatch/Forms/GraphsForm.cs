using FireDeptStopwatch.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FireDeptStopwatch.Forms
{
    public partial class GraphsForm : Form
    {
        private List<TimerResult> ResultList { get; set; }

        public GraphsForm()
        {
            InitializeComponent();
        }

        private double f(int i)
        {
            var f1 = 59894 - (8128 * i) + (262 * i * i) - (1.6 * i * i * i);
            return f1;
        }

        public void InitializeComponents(MainForm parent) {
            ResultList = parent.GetResultsList();

            resultsChart.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Series1",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                XValueType = ChartValueType.DateTime,
                YValueType = ChartValueType.Auto,
                ChartType = SeriesChartType.FastLine,
            };

            this.resultsChart.Series.Add(series1);

            foreach (TimerResult result in ResultList)
            {
                var yValue = DateTime.MinValue + result.GetEndResult();
                series1.Points.AddXY(result.DateTime, yValue);
            }

            resultsChart.Invalidate();
        }
    }
}
