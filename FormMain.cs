using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;

namespace GraphWinFormCSharp
{
    public partial class FormMain : Form
    {
        private int[] m_PlotData = new int[256];
        private Point m_mousePoint;

        public FormMain()
        {
            InitializeComponent();

            menuStrip.MouseDown += new MouseEventHandler(OnMouseDownMenuStrip);
            menuStrip.MouseMove += new MouseEventHandler(OnMouseMoveMenuStrip);

            plotView.Model = new OxyPlot.PlotModel { PlotType = OxyPlot.PlotType.XY };

            InitGraph();
        }

        private void OnMouseDownMenuStrip(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                m_mousePoint = new Point(e.X, e.Y);
            }

            return;
        }

        private void OnMouseMoveMenuStrip(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Left += e.X - m_mousePoint.X;
                this.Top += e.Y - m_mousePoint.Y;
            }

            return;
        }

        private void InitGraph()
        {
            InitPlotData();

            Draw();

            return;
        }

        private void Draw()
        {
            var dataList = new List<DataPoint>();
            for (int nIdx = 0; nIdx < m_PlotData.Length; nIdx++)
            {
                var dataPoint = new DataPoint(nIdx, m_PlotData[nIdx]);
                dataList.Add(dataPoint);
            }

            var series = new LineSeries
            {
                ItemsSource = dataList,
            };
            var plotModel = new PlotModel();
            plotModel.Series.Add(series);

            plotView.Model.Series.Clear();
            plotView.Model = plotModel;

            return;
        }

        private void InitPlotData()
        {
            for (int nIdx = 0; nIdx < m_PlotData.Length; nIdx++)
            {
                m_PlotData[nIdx] = 0;
            }

            return;
        }

        private void DrawPlotData()
        {
            ComOpenFileDialog openFileDlg = new ComOpenFileDialog();
            openFileDlg.Filter = "CSV|*.csv";
            openFileDlg.Title = "Open the file";
            if (openFileDlg.ShowDialog() == true)
            {
                ReadCsv(openFileDlg.FileName);
                Draw();
            }
            return;
        }

        private void OnClickMenu(object sender, EventArgs e)
        {
            string strHeader = sender.ToString();

            switch (strHeader)
            {
                case "CSV File Read(&O)":
                    {
                        DrawPlotData();
                    }
                    break;
                default:
                    break;
            }
        }

        private void ReadCsv(string _strFileName)
        {
            StreamReader stream = new StreamReader(_strFileName);
            {
                while (!stream.EndOfStream)
                {
                    string strLine = stream.ReadLine();
                    string[] strValues = strLine.Split(',');

                    int nIndex = int.Parse(strValues.GetValue(0).ToString());
                    int nValue = int.Parse(strValues.GetValue(1).ToString());
                    if (nIndex > 255)
                    {
                        break;
                    }
                    m_PlotData[nIndex] = nValue;
                }
            }

            return;
        }
    }
}