using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Диплом
{
    public partial class Form3 : Form
    {
        public int ButtonNumber { get; private set; }
        public int staffID;

        public Form3(int buttonNumber)
        {
            InitializeComponent();
            ButtonNumber = buttonNumber;

        }

        /* private void loginButton_Click(object sender, EventArgs e)
         {
             if (CheckCredentials())
             {
                 DialogResult = DialogResult.OK;
                 Close();
             }
             else
             {
                 MessageBox.Show("Invalid username or password.");
             }
         }*/

        /* private bool CheckCredentials()
         {
             // Проверка логина и пароля
             return true;
         }*/
        public static void CloseAllReaders(MySqlConnection connection)
        {
            var openReaders = new List<MySqlDataReader>();

            // Retrieve all open readers from the connection
            foreach (var field in typeof(MySqlConnection).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
            {
                if (field.FieldType == typeof(MySqlDataReader))
                {
                    var reader = (MySqlDataReader)field.GetValue(connection);
                    if (reader != null && !reader.IsClosed)
                    {
                        openReaders.Add(reader);
                    }
                }
            }

            // Close all open readers
            foreach (var reader in openReaders)
            {
                reader.Close();
            }
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


        // Функция для проверки хэша пароля

        private bool VerifyHashedPassword(string enteredPassword, string storedHash)
        {
            // Generate the hash of the plain text
            string hashedPlainText = HashPassword(enteredPassword);

            // Compare the generated hash with the provided hashed password
            return string.Equals(hashedPlainText, storedHash, StringComparison.OrdinalIgnoreCase);
        }

        //private bool VerifyHashedPassword(string enteredPassword, string storedHash)
        //{
        //    byte[] hashBytes = Convert.FromBase64String(storedHash);

        //    using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, hashBytes, 10000))
        //    {
        //        byte[] enteredHash = pbkdf2.GetBytes(32);

        //        // Сравниваем хэш введенного пароля с хэшом, хранящимся в базе данных
        //        for (int i = 0; i < 32; i++)
        //        {
        //            if (enteredHash[i] != hashBytes[i])
        //                return false;
        //        }
        //    }

        //    return true;
        //}

        private void Back_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.KeyPress += textBox1_KeyPress;
            textBox2.KeyPress += textBox2_KeyPress;


            String loginUser = textBox1.Text;
            String passUser = textBox2.Text;

            string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice";
            //MySqlConnection connection1 = new MySqlConnection(connstring);
            using (MySqlConnection connection = new MySqlConnection(connstring))
            {
                try
                {
                    connection.Open();
                    //connection1.Open();

                    string selectQuery = "SELECT * FROM `authorization` WHERE `Login` = @login";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@login", loginUser);

                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string passwordHashFromDB = reader.GetString("PasswordHash");

                                if (VerifyHashedPassword(passUser, passwordHashFromDB))
                                {
                                    int staffId = reader.GetInt32("Staff_id");

                                    reader.Close();

                                    string loginQuery = "SELECT Post FROM staff WHERE id = @staffId";
                                    using (MySqlCommand loginCommand = new MySqlCommand(loginQuery, connection))
                                    {
                                        loginCommand.Parameters.AddWithValue("@staffId", staffId);
                                        string staffPost = loginCommand.ExecuteScalar()?.ToString();

                                        if (!String.IsNullOrEmpty(staffPost) && staffPost == "Директор" && ButtonNumber == 1)
                                        {
                                            DialogResult = DialogResult.OK;
                                            staffID = staffId;

                                            // Скрываем Form2 и Form3
                                            this.Hide(); // скрываем Form3
                                            Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
                                            form2.Hide(); // скрываем Form2

                                            Close();
                                        }
                                        else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Инженер ПТО" && ButtonNumber == 2)
                                        {
                                            DialogResult = DialogResult.OK;
                                            staffID = staffId;

                                            // Скрываем Form2 и Form3
                                            this.Hide(); // скрываем Form3
                                            Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
                                            form2.Hide(); // скрываем Form2

                                            Close();
                                        }
                                        else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Бухгалтер" && ButtonNumber == 3)
                                        {
                                            DialogResult = DialogResult.OK;
                                            staffID = staffId;

                                            // Скрываем Form2 и Form3
                                            this.Hide(); // скрываем Form3
                                            Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
                                            form2.Hide(); // скрываем Form2

                                            Close();
                                        }
                                        else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Системный администратор" && ButtonNumber == 4)
                                        {
                                            DialogResult = DialogResult.OK;
                                            staffID = staffId;

                                            // Скрываем Form2 и Form3
                                            this.Hide(); // скрываем Form3
                                            Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
                                            form2.Hide(); // скрываем Form2

                                            Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Доступ запрещен", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Не правильный логин или пароль", " ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No such data", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
                }
            }

            //string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice"; //параметры для подключения к БД
            //MySqlConnection connection = new MySqlConnection(); //объект для подключения
            //connection.ConnectionString = connstring;


            //try
            //{
            //    connection.Open(); // открытие подключения к БД

            //    string selectQuery = "SELECT * FROM `authorization` WHERE `Login` = @login";
            //    MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
            //    selectCommand.Parameters.AddWithValue("@login", loginUser);

            //    MySqlDataReader reader = selectCommand.ExecuteReader();

            //    if (reader.Read())
            //    {
            //        string passwordHashFromDB = reader.GetString("PasswordHash");

            //        //string getText = HashPassword(passUser);

            //        //MessageBox.Show("DB: " + passwordHashFromDB);
            //        //MessageBox.Show("Text: " + getText);

            //        // Проверка хэша пароля
            //        if (VerifyHashedPassword(passUser, passwordHashFromDB))
            //        {
            //            int staffId = reader.GetInt32("Staff_id");

            //            // Здесь можете выполнить дополнительные операции после успешной авторизации

            //            string loginQuery = @"SELECT s.Post FROM staff WHERE s.id = @staffId";
            //            MySqlCommand loginCommand = new MySqlCommand(loginQuery, connection);
            //            loginCommand.Parameters.AddWithValue("@staffId", staffId);
            //            string staffPost = loginCommand.ExecuteScalar()?.ToString();

            //            if (!String.IsNullOrEmpty(staffPost) && staffPost == "Директор" && ButtonNumber == 1)
            //            {
            //                DialogResult = DialogResult.OK;
            //                staffID = staffId;

            //                // Скрываем Form2 и Form3
            //                this.Hide(); // скрываем Form3
            //                Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
            //                form2.Hide(); // скрываем Form2

            //                Close();
            //            }
            //            else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Инженер ПТО" && ButtonNumber == 2)
            //            {
            //                DialogResult = DialogResult.OK;
            //                staffID = staffId;

            //                // Скрываем Form2 и Form3
            //                this.Hide(); // скрываем Form3
            //                Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
            //                form2.Hide(); // скрываем Form2

            //                Close();
            //            }
            //            else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Бухгалтер" && ButtonNumber == 3)
            //            {
            //                DialogResult = DialogResult.OK;
            //                staffID = staffId;

            //                // Скрываем Form2 и Form3
            //                this.Hide(); // скрываем Form3
            //                Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
            //                form2.Hide(); // скрываем Form2

            //                Close();
            //            }
            //            else if (!String.IsNullOrEmpty(staffPost) && staffPost == "Системный администратор" && ButtonNumber == 4)
            //            {
            //                DialogResult = DialogResult.OK;
            //                staffID = staffId;

            //                // Скрываем Form2 и Form3
            //                this.Hide(); // скрываем Form3
            //                Form2 form2 = (Form2)this.Owner; // получаем ссылку на Form2 через Owner
            //                form2.Hide(); // скрываем Form2

            //                Close();
            //            }
            //            else
            //            {
            //                MessageBox.Show("Доступ запрещен");
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Не правильный логин или пароль", " ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("No such data", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    }

            //    reader.Close(); // закрытие reader

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Ошибка при подключении к базе данных: " + ex.Message);
            //}
            //finally
            //{
            //    connection.Close(); // закрытие подключения к БД
            //}
        }

        private void textBox2_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()=/\"`~#$^&[{}]'<>,|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void textBox1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "№;:?=-/\"`~[{}]'<>,|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
