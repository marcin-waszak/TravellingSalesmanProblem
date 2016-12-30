using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private Program.Algoritms _algorithm;
        private int _m_;
        private int _lambda;
        private int _n;

        private CitiesCollection _cities;
        private CitiesCollection _drawCities;

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

        public MainForm(ref CitiesCollection cities)
        {
            _cities = cities;
            _drawCities = new CitiesCollection();
            _draw_points = new List<DrawPoint>();
            _draw_edge_points = new List<int>();

            InitializeComponent();

            // Vertical dotted line bugfix
            AlgorithmPlusRadio.Select();

            // Example points to connect
            _draw_edge_points.Add(0);
            _draw_edge_points.Add(1);
            _draw_edge_points.Add(2);
            _draw_edge_points.Add(33);
            _draw_edge_points.Add(0);
        }

        private void ReadSettings()
        {
            if (AlgorithmPlusRadio.Checked)
                _algorithm = Program.Algoritms.MiPlusLambda;
            else if (AlgorithmCommaRadio.Checked)
                _algorithm = Program.Algoritms.MiCommaLambda;

            _m_ = Convert.ToInt32(numericUpDown1.Value);
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
            ReadSettings();
            ChangeStart();

            //Program.Algorithm();

            CreateList();

            splitContainer1.Panel1.Invalidate();
        }

        private void ScaleList()
        {
            float min_longitude = _drawCities.MinLongitude;
            float min_latitude = _drawCities.MinLatitude;

            float delta_longitude = _drawCities.MaxLongitude - min_longitude;
            float delta_latitude = _drawCities.MaxLatitude - min_latitude;
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

            for (int i = 0; i < _n && i < _drawCities.Count; ++i)
            {
                float x = _drawCities[i].Longitude - _drawCities.MinLongitude;
                float y = _drawCities[i].Latitude - _drawCities.MinLatitude;

                x *= scale;
                y *= scale;

                string name = _drawCities[i].Name;
                _draw_points.Add(new DrawPoint(name, x, y));
            }
        }

        private void CreateList()
        {
            _drawCities.Clear();

            for (int i = 0; i < _n && i < _cities.Count; ++i)
                _drawCities.Add(_cities[i].Name, _cities[i].Latitude, _cities[i].Longitude);

            _drawCities.FlipLatitude();
            _drawCities.CancelOffset();
            _drawCities.ScaleLatitude(1.6f); // 1.6 ~ 1/cos(52 deg), latitude correction
        }

        public void ChangeStart()
        {
            label4.Text = "Status: Working...";
            button1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
        }

        public void ChangeFinish()
        {
            label4.Text = "Status: Finished";
            button1.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
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
