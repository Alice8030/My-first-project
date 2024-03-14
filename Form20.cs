using MySql.Data.MySqlClient;
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
    public partial class Form20 : Form
    {
        // Добавьте свойство для хранения информации о происхождении открытия
        public bool OpenedFromForm5 { get; set; }
        public bool OpenedFromForm6 { get; set; }


        public Form mainForm;

        public Form20()
        {
            InitializeComponent();
        }

        private void Form20_Load(object sender, EventArgs e)
        {
            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT id AS '№', CSR AS 'Св-во о гос.регистрации', Address AS 'Адрес' FROM alttabn";
                MySqlCommand command = new MySqlCommand(sql, connection);

                // Создание адаптера данных для выполнения команды и заполнения набора данных
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                // Создание набора данных для хранения результатов запроса
                DataSet dataset = new DataSet();

                // Заполнение набора данных данными из базы данных
                adapter.Fill(dataset);

                // Назначение набора данных в качестве источника данных для DataGridView
                dataGridView1.DataSource = dataset.Tables[0];
            }

            if (OpenedFromForm5 || OpenedFromForm6)
            {
                // Показать кнопки
                button2.Visible = true;
            }
            else
            {
                // Скрыть кнопки
                button2.Visible = false;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            mainForm.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form14 form14 = new Form14();
            form14.mainForm = mainForm;
            form14.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form15 form15 = new Form15();
            form15.mainForm = mainForm;
            form15.Show();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form16 form16 = new Form16();
            form16.mainForm = mainForm;
            form16.Show();
            
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form17 form17 = new Form17();
            form17.mainForm = mainForm;
            form17.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            textBox1.KeyPress += textBox1_KeyPress;

            // Фильтр для поиска по фамилии, имени и отчеству
            string filter = $"`Св-во о гос.регистрации` LIKE '%{searchText}%' OR `Адрес` LIKE '%{searchText}%'";

            // Примените фильтр к источнику данных DataGridView (например, DataTable или BindingSource)
            // Замените "dataGridView1" на имя вашего DataGridView
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        public void HideLinkLabel()
        {
            linkLabel1.Visible = false;
            linkLabel2.Visible = false;
            linkLabel3.Visible = false;
            linkLabel4.Visible = false;
            linkLabel5.Visible = false;
            linkLabel6.Visible = false;
            linkLabel8.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Редактирование данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Обход каждой строки в DataGridView
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Получение значений из DataGridView
                    int id = Convert.ToInt32(row.Cells["№"].Value);
                    string csr = Convert.ToString(row.Cells["Св-во о гос.регистрации"].Value);
                    string address = Convert.ToString(row.Cells["Адрес"].Value);



                    // Запрос на обновление записи в базе данных
                    string query = "UPDATE alttabn SET Address = @Address, CSR = @CSR WHERE id = @Id";

                    MySqlCommand command = new MySqlCommand(query, connection);

                    command.Connection = connection;
                    command.CommandText = query;

                    // Параметры запроса
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@CSR", csr);
                    command.Parameters.AddWithValue("@Id", id);

                    // Выполнение запроса на обновление
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Изменения внесены");
                connection.Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form13 form13 = new Form13();
            form13.mainForm = mainForm;
            form13.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns["№"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
