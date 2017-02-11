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
    public partial class AnalysesForm : Form
    {
        private List<TimerResult> ResultList { get; set; }

        public AnalysesForm()
        {
            InitializeComponent();
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
                YValueType = ChartValueType.DateTime,
                ChartType = SeriesChartType.Line,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 10
            };

            this.resultsChart.Series.Add(series1);

            if (ResultList.Count > 0)
            {
                foreach (TimerResult result in ResultList)
                {
                    var yValue = DateTime.MinValue + result.GetEndResult();
                    series1.Points.AddXY(result.DateTime, yValue);
                }

                double max = Double.MinValue;
                double min = Double.MaxValue;

                //double leftLimit = resultsChart.ChartAreas[0].AxisX.Minimum;
                //double rightLimit = resultsChart.ChartAreas[0].AxisX.Maximum;

                for (int s = 0; s < resultsChart.Series.Count; s++)
                {
                    foreach (DataPoint dp in resultsChart.Series[s].Points)
                    {
                        //if (dp.XValue >= leftLimit && dp.XValue <= rightLimit)
                        //{
                        min = Math.Min(min, dp.YValues[0]);
                        max = Math.Max(max, dp.YValues[0]);
                        //}
                    }
                }

                resultsChart.ChartAreas[0].AxisY.Minimum = min;
                resultsChart.ChartAreas[0].AxisY.Maximum = max;

                resultsChart.ChartAreas[0].AxisY.LabelStyle.Format = "mm:ss.ffff";
                resultsChart.ChartAreas[0].AxisX.LabelStyle.Format = "dd.MM.yyyy hh:mm:ss";

                resultsChart.Invalidate();
            }
            else
            {
                series1.Points.Add();
                series1.Points[0].IsEmpty = true;
            }
        }

        private void ResultsChart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            switch (e.HitTestResult.ChartElementType)
            {
                case ChartElementType.DataPoint:
                    var timerResult = ResultList[e.HitTestResult.PointIndex];

                    var xValue = timerResult.DateTime;
                    var yValue = timerResult.Result;

                    e.Text = string.Format("{0} - {1}", new[] { xValue.ToString("dd.MM.yyyy"), yValue.ToString(@"mm\:ss\.ffff") });

                    break;
            }
        }
    }
}
