using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Диплом
{
    public partial class Form11 : Form
    {
        public string newFilePath { get; set; }
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(newFilePath))
            {
                try
                {
                    Process.Start(newFilePath); // Открытие документа с помощью ассоциированной программы
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии документа: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Документ не был сохранен.");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
