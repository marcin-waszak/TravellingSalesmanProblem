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
        private BindingList<Town> towns_;

        private Program.Algoritms algorithm_;
        private int mi_;
        private int lambda_;
        private int n_;

        public MainForm(ref BindingList<Town> towns)
        {
            towns_ = towns;

            InitializeComponent();

            x_ = 10;
            y_ = 100;
        }

        private void ReadSettings()
        {
            if (AlgorithmPlusRadio.Checked)
                algorithm_ = Program.Algoritms.MiPlusLambda;
            else if (AlgorithmCommaRadio.Checked)
                algorithm_ = Program.Algoritms.MiCommaLambda;

            mi_ = Convert.ToInt32(numericUpDown1.Value);
            lambda_ = Convert.ToInt32(numericUpDown2.Value);
            n_ = Convert.ToInt32(numericUpDown3.Value);
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
            ParseFile();
        }

        private void ParseFile()
        {
            StreamReader my_reader = new StreamReader(openFileDialog1.FileName);
            towns_ = new BindingList<Town>();

            while (!my_reader.EndOfStream)
            {
                var line = my_reader.ReadLine();
                var values = line.Split(';');

                towns_.Add(new Town(values));
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
    }
}
