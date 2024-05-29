using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MO_test9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button_mediameasure_Click(object sender, EventArgs e)
        {

            Function function = new Function();
            double h_max = double.Parse(textBox_Hmax.Text);
            double dh = double.Parse(textBox_dH.Text);

            function.Faraday_measure(h_max,dh);

        }

        private void button_nonmedia_Click(object sender, EventArgs e)
        {

            Function function = new Function();
            function.Nonmedia_measure();

        }
    }
}
