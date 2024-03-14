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
    public partial class Form14 : Form
    {
        // Добавьте свойство для хранения информации о происхождении открытия
        public bool OpenedFromForm5 { get; set; }

        public Form mainForm;
        public Form14()
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

        private void Form14_Load(object sender, EventArgs e)
        {
            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT id AS '№', Class_type AS 'Классификация', Name_obj AS 'Наименование', Unit AS 'Единица измерения', Price AS 'Цена' FROM material_equipment";
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

            if (OpenedFromForm5)
            {
                // Показать кнопки
                button1.Visible = true;
                button2.Visible = true;
            }
            else
            {
                // Скрыть кнопки
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text;
            textBox1.KeyPress += textBox1_KeyPress;
            // Создайте фильтр для поиска по фамилии, имени и отчеству
            string filter = $"`Наименование` LIKE '%{searchText}%'";

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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_`~@#$^&[{}]'<>|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox2.Text;
            textBox2.KeyPress += textBox2_KeyPress;
            // Создайте фильтр для поиска по фамилии, имени и отчеству
            string filter = $"`Классификация` LIKE '%{searchText}%'";

            // Примените фильтр к источнику данных DataGridView (например, DataTable или BindingSource)
            // Замените "dataGridView1" на имя вашего DataGridView
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = filter;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.|+";

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

        private void button2_Click(object sender, EventArgs e)
        {
            // Добавление данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT COUNT(*) FROM material_equipment WHERE id = @id";
                string insertQuery = "INSERT INTO material_equipment (Class_type, Name_obj, Unit, Price) VALUES (@class_type, @name_obj, @unit, @price)";
                
                MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);


                for (int rowIndex = 0; rowIndex < dataGridView1.Rows.Count; rowIndex++)
                {
                    DataGridViewCell cell = dataGridView1.Rows[rowIndex].Cells[0];

                    if (cell.Value != null)
                    {
                        string id = cell.Value.ToString();

                        //string id = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

                        selectCommand.Parameters.Clear();
                        selectCommand.Parameters.AddWithValue("@id", id);

                        int count = Convert.ToInt32(selectCommand.ExecuteScalar());

                        if (count == 0)
                        {
                            insertCommand.Parameters.Clear();
                            //insertCommand.Parameters.AddWithValue("@id", id);
                            insertCommand.Parameters.AddWithValue("@class_type", dataGridView1.Rows[rowIndex].Cells[1].Value);
                            insertCommand.Parameters.AddWithValue("@name_obj", dataGridView1.Rows[rowIndex].Cells[2].Value);
                            insertCommand.Parameters.AddWithValue("@unit", dataGridView1.Rows[rowIndex].Cells[3].Value);
                            insertCommand.Parameters.AddWithValue("@price", dataGridView1.Rows[rowIndex].Cells[4].Value);

                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    
                }


                    MessageBox.Show("Изменения внесены");
                connection.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns["№"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            dataGridView1.Columns["№"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
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
                        string classType = Convert.ToString(row.Cells["Классификация"].Value);
                        string nameObj = Convert.ToString(row.Cells["Наименование"].Value);
                        string unit = Convert.ToString(row.Cells["Единица измерения"].Value);
                        string priceStr = Convert.ToString(row.Cells["Цена"].Value);

                        // Преобразование строки в тип decimal
                        decimal price;
                        if (!decimal.TryParse(priceStr, out price))
                        {
                            continue;
                        }

                        // Запрос на обновление записи в базе данных
                        string query = "UPDATE material_equipment SET Class_type = @ClassType, Name_obj = @NameObj, Unit = @Unit, Price = @Price WHERE id = @Id";

                        MySqlCommand command = new MySqlCommand(query, connection);

                        command.Connection = connection;
                        command.CommandText = query;

                        // Параметры запроса
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@ClassType", classType);
                        command.Parameters.AddWithValue("@NameObj", nameObj);
                        command.Parameters.AddWithValue("@Unit", unit);
                        command.Parameters.AddWithValue("@Price", price);
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
