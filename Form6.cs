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
using System.IO;
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Collections;
using System.Security.Cryptography;
using MySqlX.XDevAPI.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;
using Microsoft.Office.Interop.Word;
using MySqlX.XDevAPI;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;
using System.Runtime.ConstrainedExecution;
using System.Threading;

namespace Диплом
{
    public partial class Form6 : Form
    {
        public Form mainForm;
        public int staffID;
        private string selectedItem;

        public Form6(int staff_ID)
        {
            InitializeComponent();
            this.staffID = staff_ID;
        }

        public string ID_User
        {
            get
            {
                return label3.Text;
            }
            set
            {
                label3.Text = value;
            }
        }

        private bool flag = false;

        
        public void ReplaceWordInDocument(string filePath, string oldWord, string newWord)
        {
            // Create an instance of Word Application
            Word.Application wordApp = new Word.Application();

            // Open the document
            Word.Document document = wordApp.Documents.Open(filePath);

            // Set up Find and Replace parameters
            object findText = oldWord;
            object replaceWith = newWord;
            object replace = Word.WdReplace.wdReplaceAll;
            object matchCase = false;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;

            // Execute Find and Replace
            wordApp.Selection.Find.Execute(ref findText, ref matchCase,
            ref matchWholeWord, ref matchWildCards, ref matchSoundsLike,
            ref matchAllWordForms, Type.Missing, Type.Missing, Type.Missing,
            ref replaceWith, ref replace);

            // Save the updated document
            document.Save();

            // Close the document and release resources
            document.Close();
            wordApp.Quit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Series.KeyPress += Series_KeyPress;
            Number.KeyPress += Number_KeyPress;
            Issued_by.KeyPress += Issued_by_KeyPress;
            Date.KeyPress += Date_KeyPress;
            Address.KeyPress += Address_KeyPress;
            Phone.KeyPress += Phone_KeyPress;

            string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connstring))
                {
                    connection.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    
                    string fullName = Surname.Text;

                    // Разделение полного имени на отдельные части
                    string[] nameParts = fullName.Split(' ');

                    // Получаем значение из TextBox'ов
                    string surname = nameParts[0];
                    string first_name1 = nameParts[1];
                    string snd_name = nameParts[2];
                    string series = Series.Text;
                    string number = Number.Text;
                    string issued = Issued_by.Text;
                    string date_issued = Date.Text;
                    string address = Address.Text;
                    string phone = Phone.Text;


                    // Формируем запрос к базе данных
                    cmd.CommandText = "SELECT Surname, first_name, Second_name, Series_passport, Number_passport, Issued_by, Date_issue, Address, Phone FROM client WHERE (Surname=@surname AND first_name=@first_name AND Second_name=@snd_name AND Series_passport=@series AND Number_passport=@number AND Issued_by=@issued AND Date_issue=@date_issued AND Address=@address AND Phone=@phone)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@first_name", first_name1);
                    cmd.Parameters.AddWithValue("@snd_name", snd_name);
                    cmd.Parameters.AddWithValue("@series", series);
                    cmd.Parameters.AddWithValue("@number", number);
                    cmd.Parameters.AddWithValue("@issued", issued);
                    cmd.Parameters.AddWithValue("@date_issued", date_issued);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@phone", phone);

                    // Задаем соединение для объекта cmd
                    cmd.Connection = connection;

                    // Выполняем запрос
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        
                        // Если запись найдена, выводим сообщение
                        MessageBox.Show("Такой клиент уже существует", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    else
                    {
                        // Если запись не найдена...
                       

                        if (!String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Series.Text) && !String.IsNullOrEmpty(Number.Text) && !String.IsNullOrEmpty(Issued_by.Text) && !String.IsNullOrEmpty(Date.Text) && !String.IsNullOrEmpty(Address.Text) && !String.IsNullOrEmpty(Phone.Text))
                        {

                            string query = "INSERT INTO client (id, Surname, first_name, Second_name, Series_passport, Number_passport, Issued_by, Date_issue, Address, Phone) VALUES (NULL, @surname_cl, @first_name_cl, @snd_name_cl, @series_cl, @number_cl, @issued_cl, @date_issued_cl, @address_cl, @phone_cl)";
                            MySqlCommand command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@surname_cl", surname);
                            command.Parameters.AddWithValue("@first_name_cl", first_name1);
                            command.Parameters.AddWithValue("@snd_name_cl", snd_name);
                            command.Parameters.AddWithValue("@series_cl", Series.Text);
                            command.Parameters.AddWithValue("@number_cl", Number.Text);
                            command.Parameters.AddWithValue("@issued_cl", Issued_by.Text);
                            command.Parameters.AddWithValue("@date_issued_cl", Date.Text);
                            command.Parameters.AddWithValue("@address_cl", Address.Text);
                            command.Parameters.AddWithValue("@phone_cl", Phone.Text);

                            // Закрываем предыдущий DataReader
                            reader.Close();

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Клиент успешно добавлен", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Введите данные клиента", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }


                    }

                    // Закрываем соединение и освобождаем ресурсы
                    cmd.Dispose();

                    // Закрываем соединение с базой данных
                    connection.Close();










                }
            }
            catch (Exception ex)
            {
                Form18 form18 = new Form18();
                form18.label1.Text = ex.Message;
                MessageBox.Show("Введите данные клиента", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                form18.ShowDialog();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Surname.KeyPress += Surname_KeyPress;
            //Поиск данных клиента в client по ФИО
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            MySqlConnection connection = new MySqlConnection(connectionString);


            if (!String.IsNullOrEmpty(Surname.Text))
            {
                // Получаем значение из TextBox'а
                string fullName = Surname.Text;

                // Разделение полного имени на отдельные части
                string[] nameParts = fullName.Split(' ');
                string surname = nameParts[0];
                string first_name1 = nameParts[1];
                string snd_name = nameParts[2];





                // Формируем запрос к базе данных
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT Surname, first_name, Second_name, Series_passport, Number_passport, Issued_by, Date_issue, Address, Phone FROM client WHERE (Surname=@surname AND first_name=@first_name AND Second_name=@snd_name)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@first_name", first_name1);
                cmd.Parameters.AddWithValue("@snd_name", snd_name);

                // Задаем соединение для объекта cmd
                cmd.Connection = connection;

                // Открываем соединение с базой данных и выполняем запрос
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Если запись найдена, выводим значения из столбцов Address и Work_name в TextBox'ы
                    Series.Text = reader["Series_passport"].ToString();
                    Number.Text = reader["Number_passport"].ToString();
                    Issued_by.Text = reader["Issued_by"].ToString();
                    DateTime date = (DateTime)reader["Date_issue"];
                    Date.Text = date.ToString("yyyy-MM-dd");
                    Address.Text = reader["Address"].ToString();
                    Phone.Text = reader["Phone"].ToString();
                    flag = true;

                    button1.Enabled = false;
                }
                else
                {
                    // Если запись не найдена, выводим сообщение
                    MessageBox.Show("Такой записи не существует", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Series.Clear();
                    Number.Clear();
                    Issued_by.Clear();
                    Date.Clear();
                    Address.Clear();
                    Phone.Clear();

                    button1.Enabled = true;
                }
                // Закрываем соединение и освобождаем ресурсы
                reader.Close();

                // Закрываем соединение с базой данных
                connection.Close();
            }
            else
            {
                MessageBox.Show("Введите данные", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        



        private void Surname_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }


        private void Series_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void first_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void Second_Name_TextChanged(object sender, EventArgs e)
        {

        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form13 form13 = new Form13();
            form13.OpenedFromForm6 = true; // Передаем информацию о происхождении открытия
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
            this.Close();
            mainForm.Show();

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



        private async void button4_Click(object sender, EventArgs e)
        {
            // Показать Form19 и запустить задачу в фоновом режиме
            Form19 form19 = new Form19();
            form19.Shown += async (s, args) =>
            {
                // Выполнить операции в фоновом режиме
                await System.Threading.Tasks.Task.Run(() => ProcessData());

                // Закрыть Form19
                form19.Close();

                // Открыть Form11
                Form11 form11 = new Form11();
                form11.Show();
            };
            form19.Show();


        }

        private void ProcessData()
        {
            textBox1.KeyPress += textBox1_KeyPress;
            textBox3.KeyPress += textBox3_KeyPress;
            textBox4.KeyPress += textBox4_KeyPress;

            string connstring = "server=localhost;uid=root;pwd=alice.21;database=diplom_alice";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connstring))
                {
                    connection.Open();

                    // Договор на выполнение монтажных работ
                    if (selectedItem == "На выполнение монтажных работ")
                    {
                        string filePath = @"C:\Users\Алиса\Desktop\Диплом\Диплом\bin\Debug\document\contract1.docx";

                        if (!String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Series.Text) && !String.IsNullOrEmpty(Number.Text) && !String.IsNullOrEmpty(Issued_by.Text) && !String.IsNullOrEmpty(Date.Text) && !String.IsNullOrEmpty(Address.Text) && !String.IsNullOrEmpty(Phone.Text))
                        {

                            if (!String.IsNullOrEmpty(textBox1.Text))
                            {
                                //Создание новой записи в таблице Contract
                                int NumberEstimate = Convert.ToInt32(textBox1.Text);

                                // Проверка наличия записи с указанным номером в таблице "estimate"
                                string checkQuery = "SELECT COUNT(*) FROM estimate WHERE Number = @Number";
                                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                                checkCommand.Parameters.AddWithValue("@Number", NumberEstimate);
                                int count = Convert.ToInt32(checkCommand.ExecuteScalar());


                                // Получаем значение из TextBox'а
                                string fullName = Surname.Text;

                                // Разделение полного имени на отдельные части
                                string[] nameParts = fullName.Split(' ');
                                string surname = nameParts[0];
                                string first_name1 = nameParts[1];
                                string snd_name = nameParts[2];

                                if (count > 0)
                                {
                                    // Введенный номер существует в таблице "estimate"

                                    // Выполнение SQL-запроса для получения последней записи в столбце "Number"
                                    string query1 = "SELECT MAX(Number) FROM Contract";
                                    MySqlCommand cmd = new MySqlCommand(query1, connection);
                                    // Получение результата запроса
                                    int lastNumber = Convert.ToInt32(cmd.ExecuteScalar());

                                    // Прибавление 1 к последнему номеру
                                    int ContractNumber = lastNumber + 1;

                                    //Определение объекта
                                    string query2 = "SELECT alttabn_id FROM estimate WHERE Number = @Number";
                                    cmd.CommandText = query2;
                                    cmd.Parameters.AddWithValue("@Number", NumberEstimate);
                                    string objectId = cmd.ExecuteScalar()?.ToString();


                                    // Определение текущей даты и времени
                                    DateTime currentDate = DateTime.Now;

                                    

                                    //Определение клиента
                                    cmd.CommandText = "SELECT id FROM client WHERE (Surname=@surname AND first_name=@first_name AND Second_name=@snd_name)";
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@surname", surname);
                                    cmd.Parameters.AddWithValue("@first_name", first_name1);
                                    cmd.Parameters.AddWithValue("@snd_name", snd_name);
                                    object result2 = cmd.ExecuteScalar();
                                    int Client = Convert.ToInt32(result2);



                                    string query = "INSERT INTO contract (id, Number, Name_obj, Date, alttabn_id, Client_id, Staff_id) VALUES (NULL, @number, @name_work, @date, @object_id, @client, @staff)";
                                    MySqlCommand command = new MySqlCommand(query, connection);
                                    command.Parameters.AddWithValue("@number", ContractNumber);
                                    command.Parameters.AddWithValue("@name_work", selectedItem);
                                    command.Parameters.AddWithValue("@date", currentDate);
                                    command.Parameters.AddWithValue("@object_id", objectId);
                                    command.Parameters.AddWithValue("@client", Client);
                                    command.Parameters.AddWithValue("@staff", staffID);


                                    int rowsAffected = command.ExecuteNonQuery();

                                }
                                else
                                {
                                    // Введенный номер не существует в таблице "estimate"
                                    MessageBox.Show("Сметы с таким номером не существует", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }








                                string getContractData = "SELECT id, Number, Date, Name_obj FROM contract ORDER BY id DESC LIMIT 1";
                                string getClientData = @"SELECT CONCAT(c.Surname, ' ', c.first_name, ' ', c.Second_name) AS FullName,
                                    c.Series_passport, c.Number_passport, c.Issued_by,
                                    c.Date_issue, c.Address, c.Phone FROM client c WHERE c.id = (SELECT id
                                    FROM client WHERE Surname = @Surname AND first_name = @first_name AND Second_name = @second_name LIMIT 1)";
                                string getotalSumData = "SELECT SUM(Total_sum) from estimate WHERE Number=@number";
                                string getAlttabnData = "SELECT alttabn_id from estimate WHERE Number=@number";
                                string getWorkNameData = "SELECT Work_name FROM alttabn WHERE id=@alttabnId";




                                MySqlCommand getContractDataCmd = new MySqlCommand(getContractData, connection);
                                MySqlCommand getClientDataCmd = new MySqlCommand(getClientData, connection);
                                MySqlCommand getTotalSumCmd = new MySqlCommand(getotalSumData, connection);
                                MySqlCommand getAlttabnCmd = new MySqlCommand(getAlttabnData, connection);
                                MySqlCommand getWorkNameCmd = new MySqlCommand(getWorkNameData, connection);
                                getClientDataCmd.Parameters.AddWithValue("@Surname", surname);
                                getClientDataCmd.Parameters.AddWithValue("@first_name", first_name1);
                                getClientDataCmd.Parameters.AddWithValue("@second_name", snd_name);
                                getTotalSumCmd.Parameters.AddWithValue("@number", textBox1.Text);
                                getAlttabnCmd.Parameters.AddWithValue("@number", textBox1.Text);
                                decimal totalSum = Convert.ToDecimal(getTotalSumCmd.ExecuteScalar()?.ToString());
                                int alttabn_id = Convert.ToInt32(getAlttabnCmd.ExecuteScalar()?.ToString());
                                getWorkNameCmd.Parameters.AddWithValue("@alttabnId", alttabn_id);
                                string work_name = Convert.ToString(getWorkNameCmd.ExecuteScalar()?.ToString());

                                MySqlDataReader reader = null;

                                try
                                {

                                    //Execute the first query and retrieve the contract data
                                    using (MySqlDataReader contractReader = getContractDataCmd.ExecuteReader())
                                    {
                                        if (contractReader.Read())
                                        {
                                            //int contractId = contractReader.GetInt32("id");
                                            string contractNumber = contractReader.GetString("Number");
                                            DateTime contractDate = contractReader.GetDateTime("Date");
                                            string formattedDate = contractDate.ToString("dd.MM.yyyy");
                                            string contractNameObj = contractReader.GetString("Name_obj");

                                            contractReader.Close();
                                            // Execute the second query and retrieve the client data
                                            using (MySqlDataReader clientReader = getClientDataCmd.ExecuteReader())
                                            {
                                                if (clientReader.Read())
                                                {
                                                    string full_name = clientReader.GetString("FullName");
                                                    string seriesPassport = clientReader.GetString("Series_passport");
                                                    string numberPassport = clientReader.GetString("Number_passport");
                                                    string issuedBy = clientReader.GetString("Issued_by");
                                                    DateTime dateIssue = clientReader.GetDateTime("Date_issue");
                                                    string formattedDateIssue = dateIssue.ToString("dd.MM.yyyy");
                                                    string address = clientReader.GetString("Address");
                                                    string phone = clientReader.GetString("Phone");
                                                    clientReader.Close();

                                                    // Create a new file name
                                                    string newFileName = "Договор " + contractNumber + " от " + formattedDate;

                                                    // Duplicate the main file
                                                    string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + ".docx");
                                                    File.Copy(filePath, newFilePath);

                                                    Dictionary<string, string> hashMap = new Dictionary<string, string>()
                                        {
                                            {"{Number таблица Contract}", contractNumber},
                                            {"{Date таблица Contract}", formattedDate},
                                            {"{Surname + first_name + Second_name таблица Client}", full_name},
                                            {"{Work_name таблица alttabn}", work_name},
                                            {"{Series_passport таблица Client}", seriesPassport},
                                            {"{Number_passport таблица Client}", numberPassport},
                                            {"{Issued_by таблица Client}", issuedBy},
                                            {"{Date_issue таблица Client}", formattedDateIssue},
                                            {"{Address таблица Client}", address},
                                            {"{Phone таблица Client}", phone},
                                            {"{Total_sum таблица estimate}", totalSum.ToString()}
                                        };

                                                    foreach (KeyValuePair<string, string> pair in hashMap)
                                                    {
                                                        ReplaceWordInDocument(newFilePath, pair.Key, pair.Value);
                                                    }


                                                    // ВСТАВКА CONTRACT_ID В ТАБЛИЦЕ ESTIMATE



                                                    //string updateEstimateQuery = "UPDATE estimate SET Contract_id = @val WHERE id=@idEstimate";
                                                    string updateEstimateQuery = "UPDATE estimate SET Contract_id = (SELECT id FROM contract ORDER BY id DESC LIMIT 1) WHERE Number = @NumberEstimate";

                                                    MySqlCommand updateInEstimateCmd = new MySqlCommand(updateEstimateQuery, connection);
                                                    updateInEstimateCmd.Parameters.AddWithValue("@NumberEstimate", NumberEstimate);
                                                    updateInEstimateCmd.ExecuteNonQuery();

                                                    Form11 form11 = new Form11();
                                                    // Установка значения FilePath в Form11
                                                    form11.newFilePath = newFilePath;
                                                    form11.ShowDialog();

                                                }
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    reader?.Close();
                                    connection.Close();
                                }

                            }
                            else
                            {
                                MessageBox.Show("Введите номер сметы", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Заполните все поля", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    //Договор на годовое сервисное (техническое) обслуживание
                    else if (selectedItem == "На годовое сервисное (техническое) обслуживание")
                    {
                        label4.Visible = false;
                        textBox1.Visible = false;

                        string filePath = @"C:\Users\Алиса\Desktop\Диплом\Диплом\bin\Debug\document\contract2.docx";

                        if (!String.IsNullOrEmpty(Surname.Text) && !String.IsNullOrEmpty(Series.Text) && !String.IsNullOrEmpty(Number.Text) && !String.IsNullOrEmpty(Issued_by.Text) && !String.IsNullOrEmpty(Date.Text) && !String.IsNullOrEmpty(Address.Text) && !String.IsNullOrEmpty(Phone.Text))
                        {

                            //Создание новой записи в таблице Contract


                            // Выполнение SQL-запроса для получения последней записи в столбце "Number"
                            string query1 = "SELECT MAX(Number) FROM Contract";
                            MySqlCommand cmd = new MySqlCommand(query1, connection);
                            // Получение результата запроса
                            int lastNumber = Convert.ToInt32(cmd.ExecuteScalar());

                            // Прибавление 1 к последнему номеру
                            int ContractNumber = lastNumber + 1;

                            //Определение объекта
                            string object_address = textBox3.Text;
                            string query2 = "SELECT id FROM alttabn WHERE Address = @Address";
                            cmd.CommandText = query2;
                            cmd.Parameters.AddWithValue("@Address", object_address);
                            string objectId = cmd.ExecuteScalar()?.ToString();


                            // Определение текущей даты и времени
                            DateTime currentDate = DateTime.Now;

                            // Получаем значение из TextBox'а
                            string fullName = Surname.Text;

                            // Разделение полного имени на отдельные части
                            string[] nameParts = fullName.Split(' ');
                            string surname = nameParts[0];
                            string first_name1 = nameParts[1];
                            string snd_name = nameParts[2];

                            //Определение клиента
                            cmd.CommandText = "SELECT id FROM client WHERE (Surname=@surname AND first_name=@first_name AND Second_name=@snd_name)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@first_name", first_name1);
                            cmd.Parameters.AddWithValue("@snd_name", snd_name);
                            object result2 = cmd.ExecuteScalar();
                            int Client = Convert.ToInt32(result2);



                            string query = "INSERT INTO contract (id, Number, Name_obj, Date, alttabn_id, Client_id, Staff_id) VALUES (NULL, @number, @name_work, @date, @object_id, @client, @staff)";
                            MySqlCommand command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@number", ContractNumber);
                            command.Parameters.AddWithValue("@name_work", selectedItem);
                            command.Parameters.AddWithValue("@date", currentDate);
                            command.Parameters.AddWithValue("@object_id", objectId);
                            command.Parameters.AddWithValue("@client", Client);
                            command.Parameters.AddWithValue("@staff", staffID);


                            int rowsAffected = command.ExecuteNonQuery();






                            //Определение наименования котла 
                            string Name_kotla = textBox4.Text;

                            string getContractData = "SELECT Number, Date, Name_obj FROM contract ORDER BY id DESC LIMIT 1";
                            string getClientData = @"SELECT CONCAT(c.Surname, ' ', c.first_name, ' ', c.Second_name) AS FullName,
                                    c.Series_passport, c.Number_passport, c.Issued_by,
                                    c.Date_issue, c.Address, c.Phone FROM client c WHERE c.id = (SELECT id
                                    FROM client WHERE Surname = @Surname AND first_name = @first_name AND Second_name = @second_name LIMIT 1)";





                            MySqlCommand getContractDataCmd = new MySqlCommand(getContractData, connection);
                            MySqlCommand getClientDataCmd = new MySqlCommand(getClientData, connection);
                            getClientDataCmd.Parameters.AddWithValue("@Surname", surname);
                            getClientDataCmd.Parameters.AddWithValue("@first_name", first_name1);
                            getClientDataCmd.Parameters.AddWithValue("@second_name", snd_name);


                            MySqlDataReader reader = null;

                            try
                            {

                                //Execute the first query and retrieve the contract data
                                using (MySqlDataReader contractReader = getContractDataCmd.ExecuteReader())
                                {
                                    if (contractReader.Read())
                                    {
                                        //int contractId = contractReader.GetInt32("id");
                                        string contractNumber = contractReader.GetString("Number");

                                        DateTime contractDate = contractReader.GetDateTime("Date");
                                        // Добавление одного года к дате
                                        DateTime newDate = contractDate.AddYears(1);
                                        string formattedDate = contractDate.ToString("dd.MM.yyyy");
                                        // Преобразование новой даты обратно в строку в нужном формате
                                        string newformattedDate = newDate.ToString("dd.MM.yyyy");

                                        string contractNameObj = contractReader.GetString("Name_obj");
                                        contractReader.Close();
                                        // Execute the second query and retrieve the client data
                                        using (MySqlDataReader clientReader = getClientDataCmd.ExecuteReader())
                                        {
                                            if (clientReader.Read())
                                            {
                                                string full_name = clientReader.GetString("FullName");
                                                string seriesPassport = clientReader.GetString("Series_passport");
                                                string numberPassport = clientReader.GetString("Number_passport");
                                                string issuedBy = clientReader.GetString("Issued_by");
                                                DateTime dateIssue = clientReader.GetDateTime("Date_issue");
                                                string formattedDateIssue = dateIssue.ToString("dd.MM.yyyy");
                                                string address = clientReader.GetString("Address");
                                                string phone = clientReader.GetString("Phone");
                                                clientReader.Close();

                                                // Create a new file name
                                                string newFileName = "Договор " + contractNumber + " от " + formattedDate;

                                                // Duplicate the main file
                                                string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + ".docx");
                                                File.Copy(filePath, newFilePath);

                                                Dictionary<string, string> hashMap = new Dictionary<string, string>()
                                        {
                                            {"{Number таблица Contract}", contractNumber},
                                            {"{Date таблица Contract}", formattedDate},
                                            {"{Surname + first_name + Second_name таблица Client}", full_name},
                                            {"{Series_passport таблица Client}", seriesPassport},
                                            {"{Number_passport таблица Client}", numberPassport},
                                            {"{Issued_by таблица Client}", issuedBy},
                                            {"{Date_issue таблица Client}", formattedDateIssue},
                                            {"{Address таблица Client}", address},
                                            {"{Phone таблица Client}", phone},
                                            {"{Наименование котла}", Name_kotla},
                                            {"{Address}", object_address},
                                            {"{Date + Year таблица Contract}", newformattedDate}
                                        };

                                                foreach (KeyValuePair<string, string> pair in hashMap)
                                                {
                                                    ReplaceWordInDocument(newFilePath, pair.Key, pair.Value);
                                                }


                                                Form11 form11 = new Form11();
                                                // Установка значения FilePath в Form11
                                                form11.newFilePath = newFilePath;
                                                form11.ShowDialog();

                                            }
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                reader?.Close();
                                connection.Close();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Заполните все поля", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }




                    // Задержка для имитации загрузки
                    Thread.Sleep(60000);

                }
            }
            catch (Exception ex)
            {
                /*Form18 form18 = new Form18();
                form18.label1.Text = ex.Message;
                form18.ShowDialog();*/
                throw ex;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            Surname.Visible = true;
            Series.Visible = true;
            Number.Visible = true;
            Issued_by.Visible = true;
            Date.Visible = true;
            Address.Visible = true;
            Phone.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            linkLabel8.Visible = true;
            linkLabel9.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            label14.Visible = true;

            label4.Visible = false;
            label6.Visible = false;
            label9.Visible = false;
            textBox1.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = false;

            Surname.Text = string.Empty;
            Series.Text = string.Empty;
            Number.Text = string.Empty;
            Issued_by.Text = string.Empty;
            Date.Text = string.Empty;
            Address.Text = string.Empty;
            Phone.Text = string.Empty;
            textBox1.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;

        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label4.Visible = true;
            label6.Visible = false;
            label9.Visible = false;
            textBox1.Visible = true;
            textBox3.Visible = false;
            textBox4.Visible = false;
            button4.Visible = true;

            linkLabel8.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel9.LinkBehavior = LinkBehavior.HoverUnderline;

            selectedItem = "На выполнение монтажных работ";
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            label4.Visible = false;
            label6.Visible = true;
            label9.Visible = true;
            textBox1.Visible = false;
            textBox3.Visible = true;
            textBox4.Visible = true;
            button4.Visible = true;

            linkLabel8.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel9.LinkBehavior = LinkBehavior.SystemDefault;

            selectedItem = "На годовое сервисное (техническое) обслуживание";
        }

        private void Surname_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=/\"`~@#$^&[{}]'<>,.+|0123456789";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void first_name_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.+|0123456789";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Second_Name_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.+|0123456789";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Series_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.+|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.+|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Issued_by_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>+|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Date_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,+|";

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
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>+|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void Phone_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>|";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form20 form20 = new Form20();
            form20.OpenedFromForm6 = true; // Передаем информацию о происхождении открытия
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
