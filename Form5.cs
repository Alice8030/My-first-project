using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace Диплом
{
    public partial class Form5 : Form
    {

        public Form mainForm;
        private int objectId;
        public int staffId;
        public Form5(int staff_ID)
        {
            InitializeComponent();
            this.staffId = staff_ID;

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

        private void button1_Click(object sender, EventArgs e)
        {
            Address.KeyPress += Address_KeyPress;
            Work_name.KeyPress += Work_name_KeyPress;

            if (!String.IsNullOrEmpty(CSR.Text) && !String.IsNullOrEmpty(Address.Text) && !String.IsNullOrEmpty(Work_name.Text))
            {
                // Заполнение новой записью таблицы alttabn
                string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice"; //параметры для подключения к БД
                using (MySqlConnection connection = new MySqlConnection(connstring)) //объект для подключения
                {
                    connection.Open(); //открытие подключения к БД

                    string queryS = "SELECT * FROM alttabn WHERE Address = @Address AND CSR = @CSR AND Work_name = @Work_name";
                    MySqlCommand cmd = new MySqlCommand(queryS, connection);
                    cmd.Parameters.AddWithValue("@Address", Address.Text);
                    cmd.Parameters.AddWithValue("@CSR", CSR.Text);
                    cmd.Parameters.AddWithValue("@Work_name", Work_name.Text);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        reader.Close();
                        // получаем id последней добавленной записи
                        string queryMAX = "select max(id) from alttabn";
                        MySqlCommand cmd1 = new MySqlCommand(queryMAX, connection);
                        MySqlDataReader readerMAX = cmd1.ExecuteReader();
                        readerMAX.Read();
                        objectId = Convert.ToInt32(readerMAX.GetValue(0)) + 1;
                        readerMAX.Close();
                        string queryI = "INSERT INTO alttabn (id, Address, CSR, Work_name) values (@id, @Address, @CSR, @Work_name)";

                        MySqlCommand command = new MySqlCommand(queryI, connection);
                        command.Parameters.AddWithValue("@id", objectId);
                        command.Parameters.AddWithValue("@Address", Address.Text);
                        command.Parameters.AddWithValue("@CSR", CSR.Text);
                        command.Parameters.AddWithValue("@Work_name", Work_name.Text);

                        command.ExecuteNonQuery(); // добавляем запись в базу данных
                    }
                    else
                    {
                        reader.Read();
                        objectId = Convert.ToInt32(reader.GetValue(0));
                        reader.Close();
                    }

                    connection.Close();

                }

                this.Hide(); // скрываем Form5
                Form8 form8 = new Form8(objectId, staffId); // Создаем новый экземпляр Form8
                form8.label1.Text = Address.Text;
                form8.label3.Text = Work_name.Text;
                //Передаем objectId в Form8
                form8.Show(); // Открываем Form8
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }


        }



        private void button2_Click(object sender, EventArgs e)
        {
            CSR.KeyPress += CSR_KeyPress;
            //Поиск объекта в таблице object по CSR
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();

            // Получаем значение из TextBox4
            string csr = CSR.Text;


            // Формируем запрос к базе данных
            cmd.CommandText = "SELECT Address, Work_name FROM alttabn WHERE CSR=@csr";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@csr", csr);

            // Задаем соединение для объекта cmd
            cmd.Connection = connection;

            // Открываем соединение с базой данных и выполняем запрос
            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Если запись найдена, выводим значение из столбца Address TextBox5
                Address.Text = reader["Address"].ToString();
            }
            else
            {
                // Если запись не найдена, выводим сообщение
                MessageBox.Show("Такой записи не существует", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Address.Clear();
            }
            // Закрываем соединение и освобождаем ресурсы
            reader.Close();

            // Закрываем соединение с базой данных
            connection.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form13 form13 = new Form13();
            form13.mainForm = this;
            form13.Show();

            // Установить нижнее подчеркивание для linkLabel2
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form14 form14 = new Form14();
            form14.OpenedFromForm5 = true; // Передаем информацию о происхождении открытия
            form14.mainForm = this;
            form14.Show();

            // Установить нижнее подчеркивание для linkLabel3
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form15 form15 = new Form15();
            form15.mainForm = this;
            form15.Show();

            // Установить нижнее подчеркивание для linkLabel4
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form16 form16 = new Form16();
            form16.mainForm = this;
            form16.Show();

            // Установить нижнее подчеркивание для linkLabel5
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form17 form17 = new Form17();
            form17.OpenedFromForm5 = true; // Передаем информацию о происхождении открытия
            form17.mainForm = this;
            form17.Show();

            // Установить нижнее подчеркивание для linkLabel6
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            //this.Close();
            //if (mainForm == null)
            //{
            //    MessageBox.Show("NULL");
            //}
            //else
            //{
            //    MessageBox.Show("NOT NULL");
            //}
            //mainForm.Show(); // Открываем форму
            Form mainform = Application.OpenForms[0];
            this.Close();
            mainform.Show();

            // Установить нижнее подчеркивание для linkLabel7
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        private void CSR_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!;%:?*()_=/\"`~@#$^&[{}]'<>,.|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Address_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Work_name_KeyPress(object sender, KeyPressEventArgs e)
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

        private void CSR_TextChanged(object sender, EventArgs e)
        {
            // Проверяем, содержит ли текстовое поле текст
            if (!string.IsNullOrWhiteSpace(CSR.Text))
            {
                // Если текстовое поле содержит текст, делаем кнопку активной
                button2.Enabled = true;
            }
            else
            {
                // Если текстовое поле пустое, делаем кнопку неактивной
                button2.Enabled = false;
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            CSR.TextChanged += CSR_TextChanged;
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form20 form20 = new Form20();
            form20.OpenedFromForm5 = true; // Передаем информацию о происхождении открытия
            form20.mainForm = mainForm;
            form20.Show();

            // Установить нижнее подчеркивание для linkLabel8
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.SystemDefault;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Установить нижнее подчеркивание для linkLabel1
            linkLabel1.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel3.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel4.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel5.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel6.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel7.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
        }
    }
}
