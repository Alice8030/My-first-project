using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Диплом
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*for (int i = 0; i < 10; i++)
            {
                Opacity += 0.005d;
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Form3 f = new Form3();
            f.ShowDialog();*/

            OpenLoginForm(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenLoginForm(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenLoginForm(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenLoginForm(4);
        }

        private void OpenLoginForm(int buttonNumber)
        {
            Form3 loginForm = new Form3(buttonNumber);
            loginForm.Owner = this; // Я  ВСТАВИЛА ЭТОТ КОД
            loginForm.ShowDialog();
            if (loginForm.DialogResult == DialogResult.OK)
            {
                OpenTargetForm(loginForm.ButtonNumber, loginForm.staffID);
            }
        }

        private void OpenTargetForm(int buttonNumber, int staffID)
        {
            switch (buttonNumber)
            {
                case 1:
                    Form4 form4 = new Form4();
                    form4.mainForm = this;
                    form4.Show();
                    break;
                case 2:
                    Form5 form5 = new Form5(staffID);
                    form5.mainForm = this;
                    form5.Show();
                    break;
                case 3:
                    Form6 form6 = new Form6(staffID);
                    form6.mainForm = this;
                    form6.Show();
                    break;
                case 4:
                    Form7 form7 = new Form7();
                    form7.Show();
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
