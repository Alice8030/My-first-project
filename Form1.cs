using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Диплом
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*for (int i = 0; i < 10; i++)
            {
               label1.Top  -= 1;
            }*/

            for (int i = 0; i < 10; i++)
            {
                Opacity += 0.005d;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
