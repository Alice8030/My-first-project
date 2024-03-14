using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Drawing.Text;
using System.Xml.Linq;

namespace Диплом
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        public string ID_User
        {
            get
            {
                return label2.Text;
            }
            set
            {
                label2.Text = value;
            }
        }

        private void linkLabel2_MouseEnter(object sender, EventArgs e)
        {

        }

        private void linkLabel2_MouseLeave(object sender, EventArgs e)
        {

        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel2
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.SystemDefault;

            linkLabel3.Visible = false;
            linkLabel4.Visible = false;

            linkLabel5.Visible = true;
            linkLabel6.Visible = true;

            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            textBox1.Visible = false;
            textBox4.Visible = false;
            comboBox1.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            label9.Visible = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel1
            linkLabel1.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;

            linkLabel3.Visible = true;
            linkLabel4.Visible = true;

            linkLabel5.Visible = false;
            linkLabel6.Visible = false;

            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel2
            linkLabel3.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;

            label4.Visible = false;
            textBox4.Visible = false;
            comboBox1.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;
            button2.Visible = false;

            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;

            label3.Visible = true;
            textBox1.Visible = true;
            button1.Visible = true;
            label9.Visible = true;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel2
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.SystemDefault;

            label3.Visible = false;
            textBox1.Visible = false;
            button1.Visible = false;
            label9.Visible = false;

            label4.Visible = true;
            textBox4.Visible = true;
            comboBox1.Visible = true;
            textBox6.Visible = true;
            textBox7.Visible = true;
            button2.Visible = true;

            label5.Visible = true;
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox1_KeyPress;

            string loginUser = textBox1.Text;
            ChangePassword(loginUser);
        }

        private void ChangePassword(string login)
        {
            string newPassword = GeneratePassword(8); // Генерация нового пароля
            string newHashedPassword = HashPassword(newPassword); // Создание нового хеша для пароля

            string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice"; // Параметры подключения к БД

            using (MySqlConnection connection = new MySqlConnection(connstring))
            {
                try
                {
                    connection.Open(); // Открытие подключения к БД

                    // Поиск текущего хеша пароля по логину
                    string selectQuery = "SELECT PasswordHash FROM authorization WHERE Login = @login";
                    MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@login", login);
                    string currentHashedPassword = selectCommand.ExecuteScalar()?.ToString();

                    if (!string.IsNullOrEmpty(currentHashedPassword))
                    {
                        // Обновление хеша пароля на новый хеш
                        string updateQuery = "UPDATE authorization SET PasswordHash = @newHashedPassword WHERE Login = @login";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@newHashedPassword", newHashedPassword);
                        updateCommand.Parameters.AddWithValue("@login", login);
                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Пароль успешно изменен. Новый пароль: " + newPassword);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти пользователя с указанным логином", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.KeyPress += textBox4_KeyPress;
            textBox6.KeyPress += textBox6_KeyPress;
            textBox7.KeyPress += textBox7_KeyPress;

            string newPassword = GeneratePassword(8); // Генерация пароля длиной 8 символов
            string hashedPassword = HashPassword(newPassword);

            string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice";
            MySqlConnection connection = new MySqlConnection(connstring);
            connection.Open();

            // Команда для вставки данных в таблицу staff
            string fullName = textBox4.Text;

            // Разделение полного имени на отдельные части
            string[] nameParts = fullName.Split(' ');
            string surname = nameParts[0]; // Фамилия
            string firstName = nameParts[1]; // Имя
            string secondName = nameParts[2]; // Отчество

            string post = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);

            // Команда для вставки данных в таблицу staff
            string insertStaffQuery = "INSERT INTO staff (Surname, first_name, Second_name, Post, Phone) VALUES (@surname, @firstName, @secondName, @post, @phone)";
            MySqlCommand insertStaffCommand = new MySqlCommand(insertStaffQuery, connection);
            insertStaffCommand.Parameters.AddWithValue("@surname", surname);
            insertStaffCommand.Parameters.AddWithValue("@firstName", firstName);
            insertStaffCommand.Parameters.AddWithValue("@secondName", secondName);
            insertStaffCommand.Parameters.AddWithValue("@post", post);
            insertStaffCommand.Parameters.AddWithValue("@phone", textBox6.Text);


            // Команда для вставки данных в таблицу authorization
            string insertAuthorizationQuery = "INSERT INTO authorization (Login, PasswordHash, Staff_id) VALUES (@login, @passwordHash, @staffId)";
            MySqlCommand insertAuthorizationCommand = new MySqlCommand(insertAuthorizationQuery, connection);
            insertAuthorizationCommand.Parameters.AddWithValue("@login", textBox7.Text);
            insertAuthorizationCommand.Parameters.AddWithValue("@passwordHash", hashedPassword); // Предполагая, что у вас есть переменная "hashedPassword", содержащая хеш пароля
            insertAuthorizationCommand.Parameters.AddWithValue("@staffId", 0); // Здесь нужно указать правильное значение ID сотрудника

            insertStaffCommand.ExecuteNonQuery(); // Вставка данных в таблицу staff

            // Получите автоматически сгенерированный ID сотрудника
            int staffId = (int)insertStaffCommand.LastInsertedId;
            insertAuthorizationCommand.Parameters["@staffId"].Value = staffId; // Обновление параметра staffId в команде для вставки данных в таблицу authorization
            insertAuthorizationCommand.ExecuteNonQuery(); // Вставка данных в таблицу authorization

            connection.Close(); // Закрытие подключения к БД

            Form12 form12 = new Form12(); // Создание нового экземпляра Form12
            form12.Show();

        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Convert the plain text to bytes
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(password);

                // Compute the hash value of the plain text bytes
                byte[] hashBytes = sha256.ComputeHash(plainTextBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_.+";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            MessageBox.Show(password.ToString());
            return password.ToString();

        }

        //private string HashPassword(string password)
        //{
        //    // Генерируем соль
        //    byte[] salt;
        //    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

        //    // Создаем хеш пароля с солью
        //    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        //    byte[] hash = pbkdf2.GetBytes(20);

        //    // Комбинируем соль и хеш в один массив
        //    byte[] hashBytes = new byte[36];
        //    Array.Copy(salt, 0, hashBytes, 0, 16);
        //    Array.Copy(hash, 0, hashBytes, 16, 20);

        //    // Преобразуем в строку для сохранения в базе данных
        //    string hashedPassword = Convert.ToBase64String(hashBytes);

        //    return hashedPassword;
        //}


        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel2
            linkLabel5.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
            button3.Visible = true;
            button5.Visible = false;

            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT id, Surname AS 'Фамилия', first_name AS 'Имя', Second_name AS 'Отчество', Post AS 'Должность', Phone AS 'Номер телефона' FROM staff";
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

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel2
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.SystemDefault;
            dataGridView1.Visible = false;
            dataGridView2.Visible = true;
            button3.Visible = false;
            button5.Visible = true;

            // Соединение с базой данных
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для выборки данных
                string sql = "SELECT Login, PasswordHash, Staff_id AS 'id сотрудника' FROM authorization";
                MySqlCommand command = new MySqlCommand(sql, connection);

                // Создание адаптера данных для выполнения команды и заполнения набора данных
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                // Создание набора данных для хранения результатов запроса
                DataSet dataset = new DataSet();

                // Заполнение набора данных данными из базы данных
                adapter.Fill(dataset);

                // Назначение набора данных в качестве источника данных для DataGridView
                dataGridView2.DataSource = dataset.Tables[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        // Получаем значения из DataGridView
                        string id = row.Cells["id"].Value.ToString();
                        string surname = row.Cells["Фамилия"].Value.ToString();
                        string first_name = row.Cells["Имя"].Value.ToString();
                        string second_name = row.Cells["Отчество"].Value.ToString();
                        string post = row.Cells["Должность"].Value.ToString();
                        string phone = row.Cells["Номер телефона"].Value.ToString();


                        // Создаем команду SQL для обновления записи
                        string updateQuery = $"UPDATE staff SET Surname = '{surname}', first_name = '{first_name}', Second_name = '{second_name}', Post = '{post}', Phone = '{phone}' WHERE ID = {id}";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Изменения внесены");
                connection.Close();
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()=/\"`~#$^&[{}]'<>,|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=/\"`~@#$^&[{}]'<>,.|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }


        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()=/\"`~#$^&[{}]'<>,|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form mainform = Application.OpenForms[0];
            this.Close();
            mainform.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns["id"].ReadOnly = true; // Устанавливаем столбец "Price" только для чтения
            dataGridView1.Columns["Фамилия"].ReadOnly = false; // Устанавливаем столбец "Quantity" доступным для редактирования
            dataGridView1.Columns["Имя"].ReadOnly = false; // Устанавливаем столбец "Sum" только для чтения
            dataGridView1.Columns["Отчество"].ReadOnly = false; // Устанавливаем столбец "Sum" только для чтения
            dataGridView1.Columns["Должность"].ReadOnly = false; // Устанавливаем столбец "Sum" только для чтения
            dataGridView1.Columns["Номер телефона"].ReadOnly = false; // Устанавливаем столбец "Sum" только для чтения



        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Columns["Login"].ReadOnly = false; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView2.Columns["PasswordHash"].ReadOnly = true; // Устанавливаем столбец "Name_obj" только для чтения
            dataGridView2.Columns["id сотрудника"].ReadOnly = true; // Устанавливаем столбец "Unit" только для чтения

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        // Получаем значения из DataGridView
                        string login = row.Cells["Login"].Value.ToString();
                        string staff_id = row.Cells["id сотрудника"].Value.ToString();


                        // Создаем команду SQL для обновления записи
                        string updateQuery = $"UPDATE authorization SET Login = '{login}' WHERE Staff_id = {staff_id}";

                        using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                    }
                }

                MessageBox.Show("Изменения внесены");
                connection.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        //    string deletedId = string.Empty; // Переменная для сохранения значения первого столбца удаленной строки

        //    if (dataGridView1.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
        //    {
        //        int rowIndex = dataGridView1.SelectedRows[0].Index; // Получаем индекс выбранной строки

        //        // Получаем значение первого столбца удаленной строки
        //        deletedId = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

        //        // Выполняем удаление строки
        //        dataGridView1.Rows.RemoveAt(rowIndex);

        //    }

        //    string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        // Удаление записей в staff
        //        string deleteQuery = "DELETE FROM staff WHERE id = @staff_Id";
        //        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
        //        {
        //            deleteCommand.Parameters.AddWithValue("@staff_Id", deletedId);
        //            deleteCommand.ExecuteNonQuery();
        //        }

        //    }
        }
    }
}
