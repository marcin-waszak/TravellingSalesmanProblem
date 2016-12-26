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

namespace TravellingSalesmanProblem
{
    public partial class MainForm : Form
    {
        private int x_;
        private int y_;
        private BindingList<Town> locations_;

        public MainForm()
        {
            InitializeComponent();

            x_ = 10;
            y_ = 100;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Aqua, x_, y_, 80, 80);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++x_;
            Invalidate();
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
            StreamReader my_reader = new StreamReader(openFileDialog1.FileName);
            locations_ = new BindingList<Town>();

            while (!my_reader.EndOfStream)
            {
                var line = my_reader.ReadLine();
                var values = line.Split(';');

                locations_.Add(new Town(values));
            }

            my_reader.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
