﻿using System;
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
        public const int MinIterations = 10;
        public const int MaxIterations = 10000000;
        public const double MinMutationRate = 0.0;
        public const double MaxMutationRate = 1.0;
        private const string FormText = "Travelling Salesman Problem";

        private AlgorithmType _algorithm_type;
        private int _mi;
        private int _lambda;
        private double _mutationRate;
        private int _n;
        private bool _elitism;
        private int _iterations;

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

        public MainForm(ref CitiesCollection cities)
        {
            Text = FormText;

            _cities = cities;
            _draw_cities = new CitiesCollection();
            _draw_points = new List<DrawPoint>();
            _draw_edge_points = new List<int>();

            InitializeComponent();

            // Vertical dotted line bugfix
            AlgorithmPlusRadio.Select();
        }

        private void ReadSettings()
        {
            if (AlgorithmPlusRadio.Checked)
                _algorithm_type = AlgorithmType.MiPlusLambda;
            else if (AlgorithmCommaRadio.Checked)
                _algorithm_type = AlgorithmType.MiCommaLambda;

            _mi = Convert.ToInt32(numericUpDown1.Value);
            _lambda = Convert.ToInt32(numericUpDown2.Value);
            _mutationRate = Convert.ToDouble(mutationNumericUpDown.Value);
            _n = Convert.ToInt32(numericUpDown3.Value);
            _elitism = elitismCheckBox.Checked;
            _iterations = Convert.ToInt32(iterationsNumericUpDown.Value);
        }

        private void tabPage1_Paint(object sender, PaintEventArgs e)
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

                Brush brush;

                if (i != 0)
                    brush = Brushes.Red;
                else
                    brush = Brushes.Blue;

                e.Graphics.FillRectangle(brush, x, y, DotSize, DotSize);
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
            string filename = openFileDialog1.FileName;
            _cities = new CitiesCollection();
            Utilities.ParseFile(filename, _cities);
            Text = Path.GetFileName(filename) + " - " + FormText;
            toolStripStatusLabel1.Text = "Loaded " + _cities.Count + " cities from file";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadSettings();

            if (_cities.Count == 0)
            {
                MessageBox.Show("Cannot start the algorithm, because cities list is empty.\nLoad proper locations file first!", "Cities list empty",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (_lambda < 1 || _mi < 1 || _n < 1)
            {
                MessageBox.Show("Cannot start the algorithm, because λ, μ and n parameters have to be greater than zero!", "Bad parameters",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (_lambda <= _mi)
            {
                MessageBox.Show("Cannot start the algorithm, because λ has to be greater than μ!", "Bad parameters",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (_iterations < MinIterations || _iterations > MaxIterations)
            {
                MessageBox.Show($"Cannot start the algorithm, because steps number is incorrect! Must be in [${MinIterations}, ${MaxIterations}]", "Bad parameters",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (_mutationRate < MinMutationRate || _mutationRate > MaxMutationRate)
            {
                MessageBox.Show($"Cannot start the algorithm, because mutation rate is incorrect! Must be in [${MinMutationRate}, ${MaxMutationRate}]", "Bad parameters",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            RunAlgorithm();
        }

        private async void RunAlgorithm()
        {
            ChangeStart();

            var progress_indicator = new Progress<int>(SetProgress);
            var result_tour = await Program.Algorithm(_algorithm_type, _mi, _lambda, _mutationRate,
                _elitism, _iterations, _n, _cities.Towns, progress_indicator);

            _draw_edge_points.Clear();
            listView1.Items.Clear();

            int i = 0;
            int? firstIdx = null;
            foreach (var city in result_tour.Cities)
            {
                var idx = _cities.Towns.FindIndex(town => town.Name.Equals(city.Name));
                firstIdx = firstIdx ?? idx;
                _draw_edge_points.Add(idx);

                ++i;
                ListViewItem item = new ListViewItem(i.ToString());
                item.SubItems.Add(city.Name);
                item.SubItems.Add(city.Longitude.ToString());
                item.SubItems.Add(city.Latitude.ToString());

                if (i % 2 == 0)
                    item.BackColor = Color.FromArgb(240, 240, 240);

                listView1.Items.Add(item);
            }
            Debug.Assert(firstIdx != null, "firstIdx != null");
            _draw_edge_points.Add(firstIdx.Value);

            CreateList();
            ChangeFinish(result_tour);
            tabPage1.Invalidate();
        }

        private void ScaleList()
        {
            float min_longitude = _draw_cities.MinLongitude;
            float min_latitude = _draw_cities.MinLatitude;

            float delta_longitude = _draw_cities.MaxLongitude - min_longitude;
            float delta_latitude = _draw_cities.MaxLatitude - min_latitude;
            float points_ratio = delta_longitude / delta_latitude;

            float panel_width = tabPage1.Width;
            float panel_height = tabPage1.Height;
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

        public void SetProgress(int progress)
        {
            toolStripProgressBar1.Value = Math.Min(progress + 2, 100);
            if (progress > 0)
                toolStripProgressBar1.Value = progress + 1;

            label4.Text = "Status: Working (" + progress + "%)";
        }

        public void ChangeStart()
        {
            label4.Text = "Status: Working...";
            button1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            SetProgress(0);
            toolStripProgressBar1.Visible = true;
        }

        public void ChangeFinish(Tour result)
        {
            label4.Text = "Status: Finished";
            button1.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Text = "Total tour distance " + result.GetDistance() + " km";
        }

        private void enableAntialiasingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableAntialiasingToolStripMenuItem.Checked)
                enableAntialiasingToolStripMenuItem.Checked = false;
            else
                enableAntialiasingToolStripMenuItem.Checked = true;

            tabPage1.Invalidate();
        }
    }
}
