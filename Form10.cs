using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Диплом
{
    public partial class Form10 : Form
    {
        public string newFilePath { get; set; }
        public string Label3Text { get; set; }
        public string Label1Text { get; set; }
        public string TextBoxValue { get; set; }
        public Form10()
        {
            InitializeComponent();

            // Отложенная установка значений контролов при загрузке формы
            Load += Form10_Load;

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

        private void Form10_Load(object sender, EventArgs e)
        {

            // Установка значения LabelText в label5
            label5.Text = Label3Text;
            // Установка значения TextBoxValue в label7
            label7.Text = TextBoxValue;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
