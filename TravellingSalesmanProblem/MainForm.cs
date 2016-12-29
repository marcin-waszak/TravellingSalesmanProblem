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

        private TownCollection _towns;
        private TownCollection _draw_towns;

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

        private struct DrawEdgePoints
        {
            public int IndexA { get; private set; }

            public DrawEdgePoints(int index_a)
            {
                IndexA = index_a;
            }
        }

        private BindingList<DrawPoint> _draw_points;
        private BindingList<int> _draw_edge_points;

        public MainForm(ref TownCollection towns)
        {
            _towns = towns;
            _draw_towns = new TownCollection();
            _draw_points = new BindingList<DrawPoint>();
            _draw_edge_points = new BindingList<int>();

            InitializeComponent();

            ////
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

        private void timer1_Tick(object sender, EventArgs e)
        {

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
            _towns = new TownCollection();

            while (!my_reader.EndOfStream)
            {
                var line = my_reader.ReadLine();
                var values = line.Split(';');

                _towns.Add(new Town(values));
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
            float min_longitude = _draw_towns.MinLongitude;
            float min_latitude = _draw_towns.MinLatitude;

            float delta_longitude = _draw_towns.MaxLongitude - min_longitude;
            float delta_latitude = _draw_towns.MaxLatitude - min_latitude;
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

            for (int i = 0; i < _n && i < _draw_towns.Count; ++i)
            {
                float x = _draw_towns[i].Longitude - _draw_towns.MinLongitude;
                float y = _draw_towns[i].Latitude - _draw_towns.MinLatitude;

                x *= scale;
                y *= scale;

                string name = _draw_towns[i].Name;
                _draw_points.Add(new DrawPoint(name, x, y));
            }
        }

        private void CreateList()
        {
            _draw_towns.Clear();

            for (int i = 0; i < _n && i < _towns.Count; ++i)
                _draw_towns.Add(_towns[i].Name, _towns[i].Latitude, _towns[i].Longitude);

            _draw_towns.FlipLatitude();
            _draw_towns.CancelOffset();
            _draw_towns.ScaleLatitude(1.6f); // 1.6 ~ 1/cos(52 deg), latitude correction
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
