using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Диплом
{
    public partial class Form15 : Form
    {
        public Form mainForm;
        public Form15()
        {
            InitializeComponent();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            mainForm.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form13 form13 = new Form13();
            form13.mainForm = mainForm;
            form13.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form14 form14 = new Form14();
            form14.mainForm = mainForm;
            form14.Show();
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

        private void Form15_Load(object sender, EventArgs e)
        {
            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT Contract.Number AS 'Номер договора', Contract.Name_obj AS 'Наименование договора', Contract.Date AS 'Дата заключения', alttabn.Address AS 'Адрес', alttabn.Work_name AS 'Наименование работ', CONCAT(Client.Surname, ' ', Client.first_name, ' ', Client.Second_name) AS 'Заказчик' FROM `Contract` JOIN alttabn ON `Contract`.alttabn_id = alttabn.id JOIN Client ON `Contract`.Client_id = Client.id ";
                       
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

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            textBox1.KeyPress += textBox1_KeyPress;
            // Фильтр для поиска
            string filter = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                filter = $"(`Заказчик` LIKE '%{searchText}%' OR `Адрес` LIKE '%{searchText}%' OR CONVERT(`Номер договора`, 'System.String') LIKE '%{searchText}%')";

            }

            // Примените фильтр к источнику данных DataGridView (например, DataTable или BindingSource)
            // Замените "dataGridView1" на имя вашего DataGridView
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Соединение с базой данных
                string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();


                    //// Получение начальной и конечной даты из элементов управления DateTimePicker
                    //string startDate = dateTimePicker1.Value.ToString();
                    //DateTime endDate = dateTimePicker2.Value;

                    //// Преобразование дат в строки в нужном формате
                    //string formattedStartDate = startDate.ToString("yyyy-MM-dd");
                    //string formattedEndDate = endDate.ToString("yyyy-MM-dd");








                    string filterSql = "SELECT Contract.Number AS 'Номер договора', Contract.Name_obj AS 'Наименование договора', Contract.Date AS 'Дата заключения', alttabn.Address AS 'Адрес', alttabn.Work_name AS 'Наименование работ', Client.Surname AS 'Заказчик' " +
                       "FROM `Contract` " +
                       "JOIN alttabn ON `Contract`.alttabn_id = alttabn.id " +
                       "JOIN Client ON `Contract`.Client_id = Client.id " +
                       "WHERE Contract.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";

                    MySqlCommand filterCommand = new MySqlCommand(filterSql, connection);
                    //filterCommand.Parameters.AddWithValue("@startDate", formattedStartDate);
                    //filterCommand.Parameters.AddWithValue("@endDate", formattedEndDate);
                    MySqlDataReader filterDataReader = filterCommand.ExecuteReader();

                    DataTable filteredDataTable = new DataTable();
                    filteredDataTable.Load(filterDataReader);

                    // Закрытие ридера после фильтрации данных
                    filterDataReader.Close();

                    // Обновление источника данных DataGridView с отфильтрованными данными
                    dataGridView1.DataSource = filteredDataTable;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при выполнении SQL-запроса: " + ex.Message);
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT Contract.Number AS 'Номер договора', Contract.Name_obj AS 'Наименование договора', Contract.Date AS 'Дата заключения', alttabn.Address AS 'Адрес', alttabn.Work_name AS 'Наименование работ', CONCAT(Client.Surname, ' ', Client.first_name, ' ', Client.Second_name) AS 'Заказчик' FROM `Contract` JOIN alttabn ON `Contract`.alttabn_id = alttabn.id JOIN Client ON `Contract`.Client_id = Client.id ";

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

            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form20 form20 = new Form20();
            form20.mainForm = mainForm;
            form20.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns["Номер договора"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Наименование договора"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Дата заключения"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Адрес"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Наименование работ"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Заказчик"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения

             
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
