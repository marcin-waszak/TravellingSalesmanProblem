using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
