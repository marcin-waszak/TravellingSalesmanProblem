using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravellingSalesmanProblem
{
    public partial class MainForm : Form
    {
        private Program.AlgorithmType _algorithm_type;
        private int _mi;
        private int _lambda;
        private int _n;

        private CitiesCollection _cities;
        private CitiesCollection _draw_cities;

        private const float DotSize = 6.0f;

        private struct DrawPoint
        {
            public string Name { get; private set; }
            public float X { get; private set; }
            public float Y { get; private set; }

            public DrawPoint(string name, float x, float y)
            {
                Name = name;
                X = x;
                Y = y;
            }
        }

        private List<DrawPoint> _draw_points;
        private List<int> _draw_edge_points;

        // Statusbar
        StatusBar mainStatusBar;
        StatusBarPanel statusPanel_1;
        StatusBarPanel statusPanel_2;

        public MainForm(ref CitiesCollection cities)
        {
            _cities = cities;
            _draw_cities = new CitiesCollection();
            _draw_points = new List<DrawPoint>();
            _draw_edge_points = new List<int>();

            InitializeComponent();

            // Initialize Statusbar
            mainStatusBar = new StatusBar();
            statusPanel_1 = new StatusBarPanel();
            statusPanel_2 = new StatusBarPanel();

            // Set first panel properties and add to StatusBar
            statusPanel_1.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel_1.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel_1);

            // Set second panel properties and add to StatusBar
            statusPanel_2.BorderStyle = StatusBarPanelBorderStyle.Raised;
            statusPanel_2.Width = 100;
            statusPanel_2.Text = "Progress 50%";
            //statusPanel_2.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel_2);

            mainStatusBar.SizingGrip = false;
            mainStatusBar.ShowPanels = true;

            //In the end, we add StatusBar to the Form.
            Controls.Add(mainStatusBar);

            // Vertical dotted line bugfix
            AlgorithmPlusRadio.Select();
        }

        private void ReadSettings()
        {
            if (AlgorithmPlusRadio.Checked)
                _algorithm_type = Program.AlgorithmType.MiPlusLambda;
            else if (AlgorithmCommaRadio.Checked)
                _algorithm_type = Program.AlgorithmType.MiCommaLambda;

            _mi = Convert.ToInt32(numericUpDown1.Value);
            _lambda = Convert.ToInt32(numericUpDown2.Value);
            _n = Convert.ToInt32(numericUpDown3.Value);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            ScaleList();

            if (_draw_points.Count < 1)
                return;

            PointF[] points = new PointF[_draw_edge_points.Count];

            for (int i = 0; i < _draw_edge_points.Count; ++i)
            {
                float x = _draw_points[_draw_edge_points[i]].X;
                float y = _draw_points[_draw_edge_points[i]].Y;
                points[i] = new PointF(x, y);
            }

            // Draw lines onto the Panel1
            Pen blackPen = new Pen(Color.Black, 1);

            if (enableAntialiasingToolStripMenuItem.Checked)
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            else
                e.Graphics.SmoothingMode = SmoothingMode.None;

            e.Graphics.DrawLines(blackPen, points);

            // Draw points onto the Panel1
            for (int i = 0; i < _draw_points.Count; ++i)
            {
                float x = _draw_points[i].X - DotSize / 2.0f;
                float y = _draw_points[i].Y - DotSize / 2.0f;
                e.Graphics.FillRectangle(Brushes.Red, x, y, DotSize, DotSize);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "The Travelling Salesman Problem with genetic algorithm approach." +
                "\n\nApplication licensed under GNU GPL. Copyright 2016." +
                "\n\nDamian Bułak" +
                "\nMarcin Waszak" +
                "\nAleksander Białobrzeski",
                "About",
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ParseFile();
        }

        private void ParseFile()
        {
            StreamReader my_reader = new StreamReader(openFileDialog1.FileName);
            _cities = new CitiesCollection();

            while (!my_reader.EndOfStream)
            {
                var line = my_reader.ReadLine();
                var values = line.Split(';');

                _cities.Add(new City(values));
            }

            my_reader.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            RunAlgorithm();
        }

        private async void RunAlgorithm()
        {
            ReadSettings();
            ChangeStart();
            _draw_edge_points.Clear();

            var resultTour = Task<Tour>.Factory.StartNew(() => Program.Algorithm(_algorithm_type, _mi, _lambda, _n, _cities.Towns));

            await resultTour;

            int? firstIdx = null;
            foreach (var city in resultTour.Result.Cities)
            {
                var idx = _cities.Towns.FindIndex(town => town.Name.Equals(city.Name));
                firstIdx = firstIdx ?? idx;
                _draw_edge_points.Add(idx);
            }
            Debug.Assert(firstIdx != null, "firstIdx != null");
            _draw_edge_points.Add(firstIdx.Value);

            CreateList();
            ChangeFinish(resultTour.Result);
            splitContainer1.Panel1.Invalidate();
        }

        private void ScaleList()
        {
            float min_longitude = _draw_cities.MinLongitude;
            float min_latitude = _draw_cities.MinLatitude;

            float delta_longitude = _draw_cities.MaxLongitude - min_longitude;
            float delta_latitude = _draw_cities.MaxLatitude - min_latitude;
            float points_ratio = delta_longitude / delta_latitude;

            float panel_width = splitContainer1.Panel1.Width;
            float panel_height = splitContainer1.Panel1.Height;
            float panel_ratio = panel_width / panel_height;

            float scale = 1.0f;

            if (points_ratio > panel_ratio)
                scale = (panel_width) / delta_longitude;
            else
                scale = (panel_height) / delta_latitude;

            _draw_points.Clear();

            for (int i = 0; i < _draw_cities.Count; ++i)
            {
                float x = _draw_cities[i].Longitude;
                float y = _draw_cities[i].Latitude;

                x *= scale;
                y *= scale;

                string name = _draw_cities[i].Name;
                _draw_points.Add(new DrawPoint(name, x, y));
            }
        }

        private void CreateList()
        {
            _draw_cities.Clear();

            for (int i = 0; i < _n && i < _cities.Count; ++i)
                _draw_cities.Add(_cities[i].Name, _cities[i].Latitude, _cities[i].Longitude);

            _draw_cities.FlipLatitude();
            _draw_cities.CancelOffset();
            _draw_cities.ScaleLatitude(1.6f); // 1.6 ~ 1/cos(52 deg), latitude correction
        }

        public void ChangeStart()
        {
            label4.Text = "Status: Working...";
            button1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
        }

        public void ChangeFinish(Tour result)
        {
            label4.Text = "Status: Finished";
            button1.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;

            statusPanel_1.Text = "Total tour distance " + result.GetDistance() + " km";
        }

        private void enableAntialiasingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableAntialiasingToolStripMenuItem.Checked)
                enableAntialiasingToolStripMenuItem.Checked = false;
            else
                enableAntialiasingToolStripMenuItem.Checked = true;

            splitContainer1.Panel1.Invalidate();
        }
        
    }
}
