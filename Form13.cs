using Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Диплом
{
    public partial class Form13 : Form
    {
        // Добавьте свойство для хранения информации о происхождении открытия
        public bool OpenedFromForm6 { get; set; }

        public Form mainForm;

        public Form13()
        {
            InitializeComponent();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT id AS '№', Surname AS 'Фамилия', first_name AS 'Имя', Second_name AS 'Отчество', Series_passport AS 'Серия паспорта', Number_passport AS 'Номер паспорта', Issued_by AS 'Кем выдан', Date_issue AS 'Когда выдан', Address AS 'Адрес', Phone AS 'Номер телефона' FROM client";
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

            if (OpenedFromForm6)
            {
                // Показать кнопки
                button1.Visible = true;
            }
            else
            {
                // Скрыть кнопки
                button1.Visible = false;
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
            string filter = $"`Фамилия` LIKE '%{searchText}%' OR `Имя` LIKE '%{searchText}%' OR `Отчество` LIKE '%{searchText}%' OR `Номер телефона` LIKE '%{searchText}%'";

            // Примените фильтр к источнику данных DataGridView (например, DataTable или BindingSource)
            // Замените "dataGridView1" на имя вашего DataGridView
            (dataGridView1.DataSource as System.Data.DataTable).DefaultView.RowFilter = filter;
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
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hide();
            Form20 form20 = new Form20();
            form20.mainForm = mainForm;
            form20.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Редактирование данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";


            

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    

                    // Обход каждой строки в DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Получение значений из DataGridView
                        int id = Convert.ToInt32(row.Cells["№"].Value);
                        string surname = Convert.ToString(row.Cells["Фамилия"].Value);
                        string firstName = Convert.ToString(row.Cells["Имя"].Value);
                        string secondName = Convert.ToString(row.Cells["Отчество"].Value);
                        string seriesPassport = Convert.ToString(row.Cells["Серия паспорта"].Value);
                        string numberPassport = Convert.ToString(row.Cells["Номер паспорта"].Value);
                        string issuedBy = Convert.ToString(row.Cells["Кем выдан"].Value);
                        DateTime dateIssue = Convert.ToDateTime(row.Cells["Когда выдан"].Value);
                        string address = Convert.ToString(row.Cells["Адрес"].Value);
                        string phone = Convert.ToString(row.Cells["Номер телефона"].Value);

                        // Запрос на обновление записи в базе данных
                        string query = "UPDATE client SET Surname = @Surname, first_name = @FirstName, Second_name = @SecondName, " +
                                       "Series_passport = @SeriesPassport, Number_passport = @NumberPassport, Issued_by = @IssuedBy, " +
                                       "Date_issue = @DateIssue, Address = @Address, Phone = @Phone WHERE id = @Id";

                        MySqlCommand command = new MySqlCommand(query, connection);

                        command.Connection = connection;
                        command.CommandText = query;

                        // Параметры запроса
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Surname", surname);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@SecondName", secondName);
                        command.Parameters.AddWithValue("@SeriesPassport", seriesPassport);
                        command.Parameters.AddWithValue("@NumberPassport", numberPassport);
                        command.Parameters.AddWithValue("@IssuedBy", issuedBy);
                        command.Parameters.AddWithValue("@DateIssue", dateIssue.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@Id", id);

                        // Выполнение запроса на обновление
                        command.ExecuteNonQuery();
                    }


                    

                    MessageBox.Show("Изменения внесены");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }

            }
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
