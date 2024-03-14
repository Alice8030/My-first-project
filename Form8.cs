using Word =  Microsoft.Office.Interop.Word;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Data.SqlClient;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Диплом
{
    public partial class Form8 : Form
    {
        public Form Form5;
        private int objectId;
        public int staffID;
        public Form8(int obj_id, int staff_ID)
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            this.staffID = staff_ID;


            objectId = obj_id;

            Form5 = new Form5(staff_ID);
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            //Заполнение узлов TreeView. Названия материалов берутся из таблицы material_equipment
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            //TreeView для материалов

            // Создаем новый объект MySqlCommand для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand("SELECT id, Name_obj, Class_type FROM material_equipment", connection);

            // Создаем объект MySqlDataReader для чтения результатов запроса
            MySqlDataReader reader1 = command.ExecuteReader();

            // Создаем словарь для хранения узлов категорий
            Dictionary<string, TreeNode> ClassNodes1 = new Dictionary<string, TreeNode>();

            // Проходим по всем строкам результата запроса
            while (reader1.Read())
            {
                // Получаем значения столбцов "id", "Name" и "Class"
                int itemId1 = reader1.GetInt32(0);
                string itemName1 = reader1.GetString(1);
                string ClassName1 = reader1.GetString(2);

                // Если категория уже есть в словаре, добавляем новый узел элемента к ее узлу
                if (ClassNodes1.ContainsKey(ClassName1))
                {
                    TreeNode itemNode = new TreeNode(itemName1);
                    itemNode.Tag = itemId1;
                    ClassNodes1[ClassName1].Nodes.Add(itemNode);
                }
                // Иначе создаем новый узел категории и добавляем в словарь
                else
                {
                    TreeNode ClassNode = new TreeNode(ClassName1);
                    TreeNode itemNode = new TreeNode(itemName1);
                    itemNode.Tag = itemId1;
                    ClassNode.Nodes.Add(itemNode);
                    ClassNodes1.Add(ClassName1, ClassNode);
                }
            }

            // Очищаем список узлов TreeView и добавляем новые узлы категорий
            treeView1.Nodes.Clear();
            foreach (TreeNode ClassNode in ClassNodes1.Values)
            {
                treeView1.Nodes.Add(ClassNode);
            }

            reader1.Close();


            //TreeView для услуг

            // Создаем новый объект MySqlCommand для выполнения SQL-запроса
            MySqlCommand cmd = new MySqlCommand("SELECT id, Name_obj, Class_type FROM service", connection);

            // Создаем объект MySqlDataReader для чтения результатов запроса
            MySqlDataReader reader2 = cmd.ExecuteReader();

            // Создаем словарь для хранения узлов категорий
            Dictionary<string, TreeNode> ClassNodes2 = new Dictionary<string, TreeNode>();

            // Проходим по всем строкам результата запроса
            while (reader2.Read())
            {
                // Получаем значения столбцов "id", "Name" и "Class"
                int itemId2 = reader2.GetInt32(0);
                string itemName2 = reader2.GetString(1);
                string ClassName2 = reader2.GetString(2);

                // Если категория уже есть в словаре, добавляем новый узел элемента к ее узлу
                if (ClassNodes2.ContainsKey(ClassName2))
                {
                    TreeNode itemNode = new TreeNode(itemName2);
                    itemNode.Tag = itemId2;
                    ClassNodes2[ClassName2].Nodes.Add(itemNode);
                }
                // Иначе создаем новый узел категории и добавляем в словарь
                else
                {
                    TreeNode ClassNode = new TreeNode(ClassName2);
                    TreeNode itemNode = new TreeNode(itemName2);
                    itemNode.Tag = itemId2;
                    ClassNode.Nodes.Add(itemNode);
                    ClassNodes2.Add(ClassName2, ClassNode);
                }
            }

            // Очищаем список узлов TreeView и добавляем новые узлы категорий
            treeView2.Nodes.Clear();
            foreach (TreeNode ClassNode in ClassNodes2.Values)
            {
                treeView2.Nodes.Add(ClassNode);
            }

            // Добавляем столбцы в DataGridView
            this.dataGridView1.Columns.Add("ID", "№");
            this.dataGridView1.Columns.Add("Name_obj", "Наименование");
            this.dataGridView1.Columns.Add("Unit", "Единицы измерения");
            this.dataGridView1.Columns.Add("Price", "Цена");
            this.dataGridView1.Columns.Add("Quantity", "Количество");
            this.dataGridView1.Columns.Add("Sum", "Сумма");

            // Настраиваем DataGridView
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AllowUserToAddRows = false;

            //Для подключения обработчика события RowsAdded к DataGridView
            this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);

            // Закрываем объекты чтения и подключения к базе данных
            reader2.Close();
            connection.Close();


        }


        // Объявляем словарь для хранения выбранных узлов
        private Dictionary<string, bool> selectedNodes = new Dictionary<string, bool>();


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Проверяем, был ли уже выбран данный узел
            if (selectedNodes.ContainsKey(e.Node.Text))
            {
                MessageBox.Show("Данный элемент уже выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            // Добавляем выбранный узел в словарь выбранных узлов
            selectedNodes[e.Node.Text] = true;


            //Отображаем Form9 для ввода количества выбранного узла
            Form9 form9 = new Form9();
            form9.ShowDialog();


            // Получаем введенное количество из Form9
            decimal quantity = form9.Quantity;
            string name = "0";
            string unit = "0";
            decimal price = 0;
            using (MySqlCommand command = new MySqlCommand("SELECT Name_obj, Unit, Price FROM material_equipment WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("@id", e.Node.Tag);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        name = reader.GetString(0);
                        unit = reader.GetString(1);
                        price = reader.GetDecimal(2);
                    }
                }
            }
            decimal sum = price * quantity;

            int rowCount = dataGridView1.Rows.Count;
            dataGridView1.Rows.Add(rowCount + 1, name, unit, price, quantity, sum);

            // Закрываем подключение к базе данных
            connection.Close();
        }

        private void treeView2_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();


            // Проверяем, был ли уже выбран данный узел
            if (selectedNodes.ContainsKey(e.Node.Text))
            {
                MessageBox.Show("Данный элемент уже выбран", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            // Добавляем выбранный узел в словарь выбранных узлов
            selectedNodes[e.Node.Text] = true;

            //Отображаем Form9 для ввода количества выбранного узла
            Form9 form9 = new Form9();
            form9.ShowDialog();

                // Получаем введенное количество из Form9
                decimal quantity = form9.Quantity;
                string name = "0";
                string unit = "0";
                decimal price = 0;
                using (MySqlCommand command = new MySqlCommand("SELECT Name_obj, Unit, Price FROM service WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", e.Node.Tag);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            name = reader.GetString(0);
                            unit = reader.GetString(1);
                            price = reader.GetDecimal(2);
                        }
                    }
                }
                decimal sum = price * quantity;

                int rowCount = dataGridView1.Rows.Count;
                dataGridView1.Rows.Add(rowCount + 1, name, unit, price, quantity, sum);
            

            // Закрываем подключение к базе данных
            connection.Close();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //Подсчитываем сумму всех строк столбца Sum в DataGridView1

            // Если столбец "Sum" еще не был добавлен, выходим из метода
            if (!dataGridView1.Columns.Contains("Sum"))
            {
                return;
            }

            decimal sum = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Если ячейка со столбцом "Sum" пуста или не содержит числовое значение, пропускаем строку
                if (row.Cells["Sum"].Value == null || !decimal.TryParse(row.Cells["Sum"].Value.ToString(), out decimal value))
                {
                    continue;
                }
                sum += value;
            }
            textBox1.Text = sum.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            treeView1.Visible = true;
            Button_SaveMaterial.Visible = true;

            treeView2.Visible = false;
            Button_SaveService.Visible = false;
            button3.Visible = false;

            linkLabel1.LinkBehavior = LinkBehavior.SystemDefault;
            linkLabel2.LinkBehavior = LinkBehavior.HoverUnderline;

            dataGridView1.Rows.Clear();
            textBox1.Clear();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            treeView2.Visible = true;
            Button_SaveService.Visible = true;

            treeView1.Visible = false;
            Button_SaveMaterial.Visible = false;
            button3.Visible = false;

            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel2.LinkBehavior = LinkBehavior.SystemDefault;

            dataGridView1.Rows.Clear();
            textBox1.Clear();
        }

        private void Button_SaveMaterial_Click(object sender, EventArgs e)
        {

            Form19 form19 = new Form19();

            //// ЗАПОЛНЕНИЕ ТАБЛИЦЫ ESTIMATE

            // Получение данных из TextBox1
            decimal totalSum = decimal.Parse(textBox1.Text);

            // Определение последнего добавленного ID в таблице estimate
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

            // Определение текущей даты и времени
            DateTime currentDate = DateTime.Now;

            // Определение текущего номера записи в таблице estimate
            int currentYear = currentDate.Year;
            string currentNumber = "1";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var command_check1 = new MySqlCommand("SELECT Number, Date, alttabn_id, Number_type FROM diplom_alice.estimate where alttabn_id = @objectid AND DATE(Date) = CURDATE() order by date desc limit 1; ", connection);
                var command_check2 = new MySqlCommand("SELECT COUNT(*) FROM diplom_alice.estimate where alttabn_id = @objectid AND DATE(Date) = CURDATE() order by date desc limit 1; ", connection);
                command_check1.Parameters.AddWithValue("@objectid", objectId);
                command_check2.Parameters.AddWithValue("@objectid", objectId);
                int count = Convert.ToInt32(command_check2.ExecuteScalar());
                MySqlDataReader reader_check = command_check1.ExecuteReader();
                reader_check.Read();
                //Если работы на текущую дату еще не были выполнены или если в базе данных нет записей на текущую дату
                if (!reader_check.HasRows || (reader_check.HasRows && Convert.ToDateTime(reader_check.GetValue(1)).Date != currentDate.Date) && count == 0)
                {
                    reader_check.Close();
                    var command = new MySqlCommand("SELECT MAX(Number) FROM estimate WHERE YEAR(Date) = @year", connection);
                    command.Parameters.AddWithValue("@year", currentYear);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int maxNumber = Convert.ToInt32(reader.GetValue(0));
                        currentNumber = (maxNumber + 1).ToString();
                    }
                }
                else if (reader_check.HasRows && Convert.ToDateTime(reader_check.GetValue(1)).Date == currentDate.Date && Convert.ToInt32(reader_check.GetValue(3)) == 2 && count == 1)
                {
                    int maxNumber = Convert.ToInt32(reader_check.GetValue(0));
                    currentNumber = (maxNumber).ToString();
                }
                else
                {
                    MessageBox.Show("Смета сегодня уже была создана", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }

            // Добавление новой записи в таблицу estimate
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO estimate (id, Number, Date, Total_sum, Staff_id, alttabn_id, Contract_id, Number_type) VALUES (NULL, @number, @date, @totalSum, @staffId, @objectId, @contractId, @number_type)", connection);
                command.Parameters.AddWithValue("@number", currentNumber);
                command.Parameters.AddWithValue("@date", currentDate);
                command.Parameters.AddWithValue("@totalSum", totalSum);
                command.Parameters.AddWithValue("@staffId", staffID);
                command.Parameters.AddWithValue("@objectId", objectId);
                command.Parameters.AddWithValue("@contractId", DBNull.Value);
                command.Parameters.AddWithValue("@number_type", "1");
                command.ExecuteNonQuery();

            }

            ////ЗАПОЛНЕНИЕ ТАБЛИЦЫ MATERIAL_EQUIPMENT_HAS_ESTIMATE

            SaveMaterialToDatabase();




            ////СОЗДАНИЕ СМЕТЫ В ВИДЕ ДОКУМЕНТА

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string filePath = @"C:\Users\Алиса\Desktop\Диплом\Диплом\bin\Debug\document\estimate1.docx";



                    string getClientData = "SELECT id, Number, Number_type, Date FROM estimate ORDER BY id DESC LIMIT 1";
                    string getStaffData = @"SELECT CONCAT(Surname, ' ', first_name, ' ', Second_name) AS FullName from staff WHERE id=@staff_Id";

                    MySqlCommand cmd = new MySqlCommand(getClientData, connection);
                    MySqlCommand getStaffDataCmd = new MySqlCommand(getStaffData, connection);
                    getStaffDataCmd.Parameters.AddWithValue("@staff_Id", staffID);


                    string staffFullName = string.Empty;

                    using (MySqlDataReader staffReader = getStaffDataCmd.ExecuteReader())
                    {
                        if (staffReader.Read())
                        {
                            staffFullName = staffReader.GetString("FullName");

                            string[] names = staffFullName.Split(' ');
                            string formattedName = string.Empty;

                            formattedName += names[1].Substring(0, 1) + "."; // Имя
                            formattedName += names[2].Substring(0, 1) + "."; // Отчество
                            formattedName += names[0]; // Фамилия


                            staffFullName = formattedName;
                        }
                        staffReader.Close();
                    }


                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        int number = reader.GetInt32("Number");
                        int numberType = reader.GetInt32("Number_type");
                        DateTime date = reader.GetDateTime("Date");
                        string formattedDate = date.ToString("dd.MM.yyyy");





                        string newFileName = "Смета " + number.ToString() + "." + numberType.ToString() + " от " + formattedDate;

                        // Duplicate the main file
                        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + ".docx");
                        File.Copy(filePath, newFilePath);


                        Dictionary<string, string> hashMap = new Dictionary<string, string>()
                                                        {
                                                            {"{id таблица estimate}", id.ToString()},
                                                            {"{number таблица estimate}", number.ToString() + "/" + numberType.ToString()},
                                                            {"{Date таблица estimate}", formattedDate},
                                                            {"{address таблица alttabn}", label1.Text},
                                                            {"{work_name таблица alttabn}", label3.Text},
                                                            {"{Surname + first_name + Second_name таблица staff}", staffFullName},
                                                            {"{total_sum таблица estimate}", totalSum.ToString()}
                                                        };

                        foreach (KeyValuePair<string, string> pair in hashMap)
                        {
                            ReplaceWordInDocument(newFilePath, pair.Key, pair.Value);
                        }



                        // Open the Word document
                        Word.Application wordApp = new Word.Application();
                        Document doc = wordApp.Documents.Open(newFilePath);

                        // Find the table in the document (assuming it's the first table)
                        Word.Table table = doc.Tables[1];

                        // Подсчитываем количество строк, которое нужно добавить
                        int rowsToAdd = dataGridView1.Rows.Count - 1;

                        // Добавляем необходимое количество строк в таблицу
                        for (int i = 0; i < rowsToAdd; i++)
                        {
                            table.Rows.Add(table.Rows[2]); // Копируем вторую строку и добавляем ее в конец таблицы
                        }


                        // Populate the table with data from DataGridView
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                table.Cell(i + 2, j + 1).Range.Text = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                        }



                        // Save and close the modified document
                        doc.Save();
                        doc.Close();





                        Form10 form10 = new Form10();
                        // Установка значения FilePath в Form10
                        form10.newFilePath = newFilePath;
                        // Установка значения Label3Text в Form10
                        form10.Label3Text = label3.Text;
                        // Установка значения Label1Text в Form10
                        form10.Label1Text = label1.Text;
                        // Установка значения TextBoxValue в Form10
                        form10.TextBoxValue = textBox1.Text;
                        form10.Show();
                    }

                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

            ////ВНЕСЕНИЕ ДАННЫХ В MATERIAL_EQUIPMENT_HAS_ESTIMATE
            private void SaveMaterialToDatabase()
            {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Получаем id Estimate, который был создан при сохранении на форме
                int estimateId = GetEstimateIdFromDatabase(connection); // Здесь необходимо реализовать получение id Estimate

                // Проходимся по каждой строке DataGridView и сохраняем данные
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Получаем данные из нужных столбцов
                    string materialEquipmentName = row.Cells["Name_obj"].Value.ToString();
                    decimal quantity = Convert.ToDecimal(row.Cells["Quantity"].Value);
                    decimal sum = Convert.ToDecimal(row.Cells["Sum"].Value);

                    // Получаем Material_Equipment_id по наименованию из таблицы material_equipment
                    int materialEquipmentId = GetMaterialEquipmentIdFromDatabase(connection, materialEquipmentName);

                    // Создаем SQL-запрос для вставки данных в таблицу material_equipment_has_estimate
                    string query = "INSERT INTO material_equipment_has_estimate (Material_Equipment_id, Estimate_id, Quantity, Summa) " +
                                   "VALUES (@materialEquipmentId, @estimateId, @quantity, @sum)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Задаем параметры для SQL-запроса
                        command.Parameters.AddWithValue("@materialEquipmentId", materialEquipmentId);
                        command.Parameters.AddWithValue("@estimateId", estimateId);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@sum", sum);

                        // Выполняем SQL-запрос
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        private int GetEstimateIdFromDatabase(MySqlConnection connection)
        {
            string query = "SELECT MAX(id) FROM estimate";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return 0; // Возвращаем 0, если не удалось получить id Estimate
        }

        private int GetMaterialEquipmentIdFromDatabase(MySqlConnection connection, string materialEquipmentName)
        {
            string query = "SELECT id FROM material_equipment WHERE Name_obj = @materialEquipmentName";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@materialEquipmentName", materialEquipmentName);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return 0; // Возвращаем 0, если не удалось получить Material_Equipment_id
        }

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

        private void Button_SaveService_Click(object sender, EventArgs e)
        {


            // Получение данных из TextBox1
            decimal totalSum = decimal.Parse(textBox1.Text);

            // Определение последнего добавленного ID в таблице estimate
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

            // Определение текущей даты и времени
            DateTime currentDate = DateTime.Now;

            // Определение текущего номера записи в таблице estimate
            int currentYear = currentDate.Year;
            string currentNumber = "1";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command_check1 = new MySqlCommand("SELECT Number, Date, alttabn_id, Number_type FROM diplom_alice.estimate where alttabn_id = @objectid AND DATE(Date) = CURDATE() order by date desc limit 1; ", connection);
                var command_check2 = new MySqlCommand("SELECT COUNT(*) FROM diplom_alice.estimate where alttabn_id = @objectid AND DATE(Date) = CURDATE() order by date desc limit 1; ", connection);
                command_check1.Parameters.AddWithValue("@objectid", objectId);
                command_check2.Parameters.AddWithValue("@objectid", objectId);
                int count = Convert.ToInt32(command_check2.ExecuteScalar());
                MySqlDataReader reader_check = command_check1.ExecuteReader();
                reader_check.Read();
                //Если работы на текущую дату еще не были выполнены или если в базе данных нет записей на текущую дату
                if (!reader_check.HasRows || (reader_check.HasRows && Convert.ToDateTime(reader_check.GetValue(1)).Date != currentDate.Date && count == 0))
                {
                    reader_check.Close();
                    var command = new MySqlCommand("SELECT MAX(Number) FROM estimate WHERE YEAR(Date) = @year", connection);
                    command.Parameters.AddWithValue("@year", currentYear);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        object maxNumberObj = reader.GetValue(0);
                        if (maxNumberObj != DBNull.Value)
                        {
                            int maxNumber = Convert.ToInt32(maxNumberObj);
                            currentNumber = (maxNumber + 1).ToString();
                        }
                        else
                        {
                            int maxNumber = 1;
                        }
                    }
                }
                else if (reader_check.HasRows && Convert.ToDateTime(reader_check.GetValue(1)).Date == currentDate.Date && Convert.ToInt32(reader_check.GetValue(3)) == 1 && count == 1)
                {
                    int maxNumber = Convert.ToInt32(reader_check.GetValue(0));
                    currentNumber = (maxNumber).ToString();
                    reader_check.Close();
                }
                else
                {
                    MessageBox.Show("Смета сегодня уже была создана", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }

            // Добавление новой записи в таблицу estimate
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO estimate (id, Number, Date, Total_sum, Staff_id, alttabn_id, Contract_id, Number_type) VALUES (NULL, @number, @date, @totalSum, @staffId, @objectId, @contractId, @number_type)", connection);
                command.Parameters.AddWithValue("@number", currentNumber);
                command.Parameters.AddWithValue("@date", currentDate);
                command.Parameters.AddWithValue("@totalSum", totalSum);
                command.Parameters.AddWithValue("@staffId", staffID);
                command.Parameters.AddWithValue("@objectId", objectId);
                command.Parameters.AddWithValue("@contractId", DBNull.Value);
                command.Parameters.AddWithValue("@number_type", "2");
                command.ExecuteNonQuery();
            }

            //ЗАПОЛНЕНИЕ ТАБЛИЦЫ SERVICE_HAS_ESTIMATE

            SaveServiceToDatabase();


            //ЗАПОЛНЕНИЕ СМЕТЫ В ВИДЕ ДОКУМЕНТА

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string filePath = @"C:\Users\Алиса\Desktop\Диплом\Диплом\bin\Debug\document\estimate1.docx";

                    string getClientData = "SELECT id, Number, Number_type, Date FROM estimate ORDER BY id DESC LIMIT 1";
                    string getStaffData = @"SELECT CONCAT(Surname, ' ', first_name, ' ', Second_name) AS FullName from staff WHERE id=@staff_Id";

                    MySqlCommand cmd = new MySqlCommand(getClientData, connection);
                    MySqlCommand getStaffDataCmd = new MySqlCommand(getStaffData, connection);
                    getStaffDataCmd.Parameters.AddWithValue("@staff_Id", staffID);
                    string staffFullName = string.Empty;

                    using (MySqlDataReader staffReader = getStaffDataCmd.ExecuteReader())
                    {
                        if (staffReader.Read())
                        {
                            staffFullName = staffReader.GetString("FullName");

                            string[] names = staffFullName.Split(' ');
                            string formattedName = string.Empty;

                            formattedName += names[1].Substring(0, 1) + "."; // Имя
                            formattedName += names[2].Substring(0, 1) + "."; // Отчество
                            formattedName += names[0]; // Фамилия


                            staffFullName = formattedName;

                        }

                        staffReader.Close();
                    }

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        int number = reader.GetInt32("Number");
                        int numberType = reader.GetInt32("Number_type");
                        DateTime date = reader.GetDateTime("Date");
                        string formattedDate = date.ToString("dd.MM.yyyy");

                        string newFileName = "Смета " + number.ToString() + "." + numberType.ToString() + " от " + formattedDate;

                        // Duplicate the main file
                        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + ".docx");
                        File.Copy(filePath, newFilePath);

                        Dictionary<string, string> hashMap = new Dictionary<string, string>()
                                                    {
                                                        {"{id таблица estimate}", id.ToString()},
                                                        {"{number таблица estimate}", number.ToString() + "/" + numberType.ToString()},
                                                        {"{Date таблица estimate}", formattedDate},
                                                        {"{address таблица alttabn}", label1.Text},
                                                        {"{work_name таблица alttabn}", label3.Text},
                                                        {"{Surname + first_name + Second_name таблица staff}", staffFullName},
                                                        {"{total_sum таблица estimate}", totalSum.ToString()}
                                                    };

                        foreach (KeyValuePair<string, string> pair in hashMap)
                        {
                            ReplaceWordInDocument(newFilePath, pair.Key, pair.Value);
                        }

                        // Open the Word document
                        Word.Application wordApp = new Word.Application();
                        Document doc = wordApp.Documents.Open(newFilePath);

                        // Find the table in the document (assuming it's the first table)
                        Word.Table table = doc.Tables[1];

                        // Подсчитываем количество строк, которое нужно добавить
                        int rowsToAdd = dataGridView1.Rows.Count - 1;

                        // Добавляем необходимое количество строк в таблицу
                        for (int i = 0; i < rowsToAdd; i++)
                        {
                            table.Rows.Add(table.Rows[2]); // Копируем вторую строку и добавляем ее в конец таблицы
                        }


                        // Populate the table with data from DataGridView
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                table.Cell(i + 2, j + 1).Range.Text = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                        }



                        // Save and close the modified document
                        doc.Save();
                        doc.Close();


                        Form10 form10 = new Form10();
                        // Установка значения FilePath в Form10
                        form10.newFilePath = newFilePath;
                        // Установка значения Label3Text в Form10
                        form10.Label3Text = label3.Text;
                        // Установка значения Label1Text в Form10
                        form10.Label1Text = label1.Text;
                        // Установка значения TextBoxValue в Form10
                        form10.TextBoxValue = textBox1.Text;
                        form10.Show();
                    }

                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        


        ////ВНЕСЕНИЕ ДАННЫХ В SERVICE_HAS_ESTIMATE

        private void SaveServiceToDatabase()
        {
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Получаем id Estimate, который был создан при сохранении на форме
                int estimateId = GetEstimateIdFromDatabase(connection); // Здесь необходимо реализовать получение id Estimate

                // Проходимся по каждой строке DataGridView и сохраняем данные
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    // Получаем данные из нужных столбцов
                    string serviceName = row.Cells["Name_obj"].Value.ToString();
                    decimal quantity = Convert.ToDecimal(row.Cells["Quantity"].Value);
                    decimal sum = Convert.ToDecimal(row.Cells["Sum"].Value);

                    // Получаем Service_id по наименованию из таблицы service
                    int serviceId = GetServiceIdFromDatabase(connection, serviceName);

                    // Создаем SQL-запрос для вставки данных в таблицу service_has_estimate
                    string query = "INSERT INTO service_has_estimate (Service_id, Estimate_id, Quantity, Summa) " +
                                   "VALUES (@serviceId, @estimateId, @quantity, @sum)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Задаем параметры для SQL-запроса
                        command.Parameters.AddWithValue("@serviceId", serviceId);
                        command.Parameters.AddWithValue("@estimateId", estimateId);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@sum", sum);

                        // Выполняем SQL-запрос
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }


        private int GetServiceIdFromDatabase(MySqlConnection connection, string serviceName)
        {
            string query = "SELECT id FROM service WHERE Name_obj = @serviceName";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@serviceName", serviceName);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }

            return 0; // Возвращаем 0, если не удалось получить Service_id
        }

        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                textBox3.KeyPress += textBox3_KeyPress;
                button3.Visible = true;
                Button_SaveMaterial.Visible = false;
                Button_SaveService.Visible = false;
                dataGridView1.Rows.Clear();

                // ДЛЯ ЗАГРУЗКИ УЖЕ СОЗДАННОЙ СМЕТЫ

                if (!String.IsNullOrEmpty(textBox3.Text))
                {
                    int id = Convert.ToInt32(textBox3.Text);

                    string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";

                    // Создание соединения с базой данных
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Проверяем существование введенного ID в столбце Estimate_id
                        string checkQuery = "SELECT COUNT(*) FROM estimate WHERE id = @estimateId";
                        using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                        {
                            checkCommand.Parameters.AddWithValue("@estimateId", id);
                            int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                            if (count == 0)
                            {
                                MessageBox.Show("Сметы с таким ID не существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Запрос для получения alttabn_id из таблицы estimate
                        string alttabnIdQuery = "SELECT alttabn_id FROM estimate WHERE id = @estimateId";
                        using (MySqlCommand alttabnIdCommand = new MySqlCommand(alttabnIdQuery, connection))
                        {
                            alttabnIdCommand.Parameters.AddWithValue("@estimateId", id);
                            using (MySqlDataReader reader = alttabnIdCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int alttabnId = reader.GetInt32("alttabn_id");


                                    // Закрыть первый DataReader
                                    reader.Close();


                                    // Запрос для получения данных из таблицы alttabn
                                    string alttabnQuery = "SELECT Address, Work_name FROM alttabn WHERE id = @alttabnId";
                                    using (MySqlCommand alttabnCommand = new MySqlCommand(alttabnQuery, connection))
                                    {
                                        alttabnCommand.Parameters.AddWithValue("@alttabnId", alttabnId);
                                        using (MySqlDataReader alttabnReader = alttabnCommand.ExecuteReader())
                                        {
                                            if (alttabnReader.Read())
                                            {
                                                string address = alttabnReader.GetString("Address");
                                                string workName = alttabnReader.GetString("Work_name");

                                                label1.Text = "Адрес объекта: " + address;
                                                label3.Text = workName;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Не удалось получить id объекта для указанной сметы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        // Запрос для получения данных из material_equipment_has_estimate
                        string estimateQuery = "SELECT Material_Equipment_id, Quantity, Summa FROM material_equipment_has_estimate WHERE Estimate_id = @id";
                        using (MySqlCommand estimateCommand = new MySqlCommand(estimateQuery, connection))
                        {
                            estimateCommand.Parameters.AddWithValue("@id", id);

                            // Создание таблицы для хранения данных из material_equipment_has_estimate
                            System.Data.DataTable estimateTable = new System.Data.DataTable();
                            MySqlDataAdapter estimateAdapter = new MySqlDataAdapter(estimateCommand);
                            estimateAdapter.Fill(estimateTable);

                            // Запрос для получения данных из material_equipment
                            string equipmentQuery1 = "SELECT Name_obj, Unit, Price FROM material_equipment WHERE id = @equipmentId";
                            using (MySqlCommand equipmentCommand1 = new MySqlCommand(equipmentQuery1, connection))
                            {
                                // Добавление параметра для заполнения Material_Equipment_id из material_equipment_has_estimate
                                equipmentCommand1.Parameters.Add("@equipmentId", MySqlDbType.Int32);

                                // Создание таблицы для хранения данных из material_equipment
                                System.Data.DataTable equipmentTable1 = new System.Data.DataTable();
                                MySqlDataAdapter equipmentAdapter1 = new MySqlDataAdapter(equipmentCommand1);

                                // Заполнение таблицы данными из material_equipment_has_estimate и material_equipment
                                foreach (DataRow estimateRow1 in estimateTable.Rows)
                                {
                                    int equipmentId1 = Convert.ToInt32(estimateRow1["Material_Equipment_id"]);
                                    equipmentCommand1.Parameters["@equipmentId"].Value = equipmentId1;

                                    equipmentTable1.Clear();
                                    equipmentAdapter1.Fill(equipmentTable1);

                                    DataGridViewRow newRow1 = dataGridView1.Rows[dataGridView1.Rows.Add()];

                                    newRow1.Cells["ID"].Value = newRow1.Index + 1;
                                    newRow1.Cells["Name_obj"].Value = equipmentTable1.Rows[0]["Name_obj"];
                                    newRow1.Cells["Unit"].Value = equipmentTable1.Rows[0]["Unit"];
                                    newRow1.Cells["Price"].Value = equipmentTable1.Rows[0]["Price"];
                                    newRow1.Cells["Quantity"].Value = estimateRow1["Quantity"];
                                    newRow1.Cells["Sum"].Value = estimateRow1["Summa"];
                                }
                            }
                        }

                        // Запрос для получения данных из service_has_estimate
                        string estimateQuery2 = "SELECT Service_id, Quantity, Summa FROM service_has_estimate WHERE Estimate_id = @id";
                        using (MySqlCommand estimateCommand2 = new MySqlCommand(estimateQuery2, connection))
                        {
                            estimateCommand2.Parameters.AddWithValue("@id", id);

                            // Создание таблицы для хранения данных из service_has_estimate
                            System.Data.DataTable estimateTable = new System.Data.DataTable();
                            MySqlDataAdapter estimateAdapter = new MySqlDataAdapter(estimateCommand2);
                            estimateAdapter.Fill(estimateTable);

                            // Запрос для получения данных из service
                            string equipmentQuery2 = "SELECT Name_obj, Unit, Price FROM service WHERE id = @equipmentId";
                            using (MySqlCommand equipmentCommand2 = new MySqlCommand(equipmentQuery2, connection))
                            {
                                // Добавление параметра для заполнения Service_id из service_has_estimate
                                equipmentCommand2.Parameters.Add("@equipmentId", MySqlDbType.Int32);

                                // Создание таблицы для хранения данных из service
                                System.Data.DataTable equipmentTable2 = new System.Data.DataTable();
                                MySqlDataAdapter equipmentAdapter2 = new MySqlDataAdapter(equipmentCommand2);

                                // Заполнение таблицы данными из service_has_estimate и service
                                foreach (DataRow estimateRow2 in estimateTable.Rows)
                                {
                                    int equipmentId2 = Convert.ToInt32(estimateRow2["Service_id"]);
                                    equipmentCommand2.Parameters["@equipmentId"].Value = equipmentId2;

                                    equipmentTable2.Clear();
                                    equipmentAdapter2.Fill(equipmentTable2);

                                    DataGridViewRow newRow2 = dataGridView1.Rows[dataGridView1.Rows.Add()];

                                    newRow2.Cells["ID"].Value = newRow2.Index + 1;
                                    newRow2.Cells["Name_obj"].Value = equipmentTable2.Rows[0]["Name_obj"];
                                    newRow2.Cells["Unit"].Value = equipmentTable2.Rows[0]["Unit"];
                                    newRow2.Cells["Price"].Value = equipmentTable2.Rows[0]["Price"];
                                    newRow2.Cells["Quantity"].Value = estimateRow2["Quantity"];
                                    newRow2.Cells["Sum"].Value = estimateRow2["Summa"];
                                }
                            }
                        }
                    }
                    decimal totalSum = 0;

                    // Перебрать все строки в DataGridView
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        // Получить значение столбца "Sum" текущей строки
                        if (row.Cells["Sum"].Value != null)
                        {
                            decimal sumValue;
                            if (decimal.TryParse(row.Cells["Sum"].Value.ToString(), out sumValue))
                            {
                                // Добавить значение к общей сумме
                                totalSum += sumValue;
                            }
                        }
                    }

                    // Записать результат в textBox1
                    textBox1.Text = totalSum.ToString();
                }
                else
                {
                    MessageBox.Show("Введите id сметы", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }


        

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index; // Получаем индекс выбранной строки
                decimal deletedValue = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["Sum"].Value); // Получаем значение ячейки сумма в выбранной строке

                // Выполняем удаление строки
                dataGridView1.Rows.RemoveAt(rowIndex);

                // Вычитаем значение ячейки сумма удаленной строки из TextBox1
                decimal currentValue = Convert.ToDecimal(textBox1.Text);
                decimal updatedValue = currentValue - deletedValue;
                textBox1.Text = updatedValue.ToString();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Columns["ID"].ReadOnly = true; // Устанавливаем столбец "ID" доступным только для чтения
            dataGridView1.Columns["Name_obj"].ReadOnly = true; // Устанавливаем столбец "Name_obj" только для чтения
            dataGridView1.Columns["Unit"].ReadOnly = true; // Устанавливаем столбец "Unit" только для чтения
            dataGridView1.Columns["Price"].ReadOnly = true; // Устанавливаем столбец "Price" только для чтения
            dataGridView1.Columns["Quantity"].ReadOnly = false; // Устанавливаем столбец "Quantity" доступным для редактирования
            dataGridView1.Columns["Sum"].ReadOnly = true; // Устанавливаем столбец "Sum" только для чтения

            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Quantity"].Index) // Проверяем, что была отредактирована ячейка столбца "Quantity"
            {
                int quantity = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value);
                double price = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value);
                double sum = quantity * price;
                dataGridView1.Rows[e.RowIndex].Cells["Sum"].Value = sum;

                if (e.ColumnIndex == dataGridView1.Columns["Quantity"].Index) // Проверяем, что была отредактирована ячейка столбца "Quantity"
                {
                    RecalculateSum(); // Вызываем метод для пересчета суммы
                }
            }
        }

        private void RecalculateSum()
        {
            double sum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Если ячейка со столбцом "Sum" пуста или не содержит числовое значение, пропускаем строку
                if (row.Cells["Sum"].Value == null || !double.TryParse(row.Cells["Sum"].Value.ToString(), out double value))
                {
                    continue;
                }
                sum += value;
            }

            textBox1.Text = sum.ToString();
        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Символы, которые недопустимы
            string invalidChars = "!№;%:?*()_=-/\"`~@#$^&[{}]'<>,.|+";

            // Проверяем, является ли введенный символ недопустимым
            if (invalidChars.Contains(e.KeyChar))
            {
                e.Handled = true; // Отменяем событие KeyPress
                MessageBox.Show("Введено недопустимое значение", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textBox3.Clear();

            Form19 form19 = new Form19();

            //// ЗАПОЛНЕНИЕ ТАБЛИЦЫ ESTIMATE

            int id = Convert.ToInt32(textBox3.Text);

            // Получение данных из TextBox1
            decimal totalSum = decimal.Parse(textBox1.Text);

            // Определение последнего добавленного ID в таблице estimate
            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";



            // Добавление новой записи в таблицу estimate
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Установка текущей даты в столбец Date
                string updateDateQuery = "UPDATE estimate SET Date = NOW() WHERE id = @estimateId";
                using (MySqlCommand updateDateCommand = new MySqlCommand(updateDateQuery, connection))
                {
                    updateDateCommand.Parameters.AddWithValue("@estimateId", id);
                    updateDateCommand.ExecuteNonQuery();
                }

                // Обновление общей суммы в столбце Total_sum
                string updateTotalSumQuery = "UPDATE estimate SET Total_sum = @totalSum WHERE id = @estimateId";
                using (MySqlCommand updateTotalSumCommand = new MySqlCommand(updateTotalSumQuery, connection))
                {
                    updateTotalSumCommand.Parameters.AddWithValue("@totalSum", totalSum);
                    updateTotalSumCommand.Parameters.AddWithValue("@estimateId", id);
                    updateTotalSumCommand.ExecuteNonQuery();
                }

                // Обновление сотрудника в столбце Staff_id
                string updateStaffIdQuery = "UPDATE estimate SET Staff_id = @staffId WHERE id = @estimateId";
                using (MySqlCommand updateStaffIdCommand = new MySqlCommand(updateStaffIdQuery, connection))
                {
                    updateStaffIdCommand.Parameters.AddWithValue("@staffId", staffID);
                    updateStaffIdCommand.Parameters.AddWithValue("@estimateId", id);
                    updateStaffIdCommand.ExecuteNonQuery();
                }

            }

            int type_est;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var check = new MySqlCommand("SELECT Number_type FROM diplom_alice.estimate where id = @id", connection);
                check.Parameters.AddWithValue("@id", id);
                type_est = Convert.ToInt32(check.ExecuteScalar());
            }

            if (type_est == 1)
            {
                ////ЗАПОЛНЕНИЕ ТАБЛИЦЫ MATERIAL_EQUIPMENT_HAS_ESTIMATE

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Удаление прежних записей в material_equipment_has_estimate
                    string deleteQuery = "DELETE FROM material_equipment_has_estimate WHERE Estimate_id = @estimateId";
                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@estimateId", id);
                        deleteCommand.ExecuteNonQuery();
                    }




                    //int estimateId = id;

                    // Добавление новых записей в material_equipment_has_estimate
                    string insertQuery = "INSERT INTO material_equipment_has_estimate (Estimate_id, Material_Equipment_id, Quantity, Summa) VALUES (@estimateId, @equipmentId, @quantity, @sum)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@estimateId", id); // Добавляем параметр только один раз
                        insertCommand.Parameters.AddWithValue("@equipmentId", 0);
                        insertCommand.Parameters.AddWithValue("@quantity", 0);  // Здесь можно использовать любое значение по умолчанию
                        insertCommand.Parameters.AddWithValue("@sum", 0);       // Здесь можно использовать любое значение по умолчанию



                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Получаем данные из нужных столбцов
                            string materialEquipmentName = row.Cells["Name_obj"].Value.ToString();
                            decimal quantity = Convert.ToDecimal(row.Cells["Quantity"].Value);
                            decimal sum = Convert.ToDecimal(row.Cells["Sum"].Value);
                            // Получаем Material_Equipment_id по наименованию из таблицы material_equipment
                            int materialEquipmentId = GetMaterialEquipmentIdFromDatabase(connection, materialEquipmentName);

                            // Изменяем значение параметра "@estimateId"
                            insertCommand.Parameters["@estimateId"].Value = id;
                            insertCommand.Parameters["@equipmentId"].Value = materialEquipmentId;
                            insertCommand.Parameters["@quantity"].Value = quantity;
                            insertCommand.Parameters["@sum"].Value = sum;

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
            else if (type_est == 2)
            {
                ////ЗАПОЛНЕНИЕ SERVICE_HAS_ESTIMATE

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Удаление прежних записей в service_has_estimate
                    string deleteQuery = "DELETE FROM service_has_estimate WHERE Estimate_id = @estimateId";
                    using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@estimateId", id);
                        deleteCommand.ExecuteNonQuery();
                    }




                    //int estimateId = id;

                    // Добавление новых записей в service_has_estimate
                    string insertQuery = "INSERT INTO service_has_estimate (Estimate_id, Service_id, Quantity, Summa) VALUES (@estimateId, @serviceId, @quantity, @sum)";
                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@estimateId", id); // Добавляем параметр только один раз
                        insertCommand.Parameters.AddWithValue("@serviceId", 0);
                        insertCommand.Parameters.AddWithValue("@quantity", 0);  // Здесь можно использовать любое значение по умолчанию
                        insertCommand.Parameters.AddWithValue("@sum", 0);       // Здесь можно использовать любое значение по умолчанию



                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Получаем данные из нужных столбцов
                            string serviceName = row.Cells["Name_obj"].Value.ToString();
                            decimal quantity = Convert.ToDecimal(row.Cells["Quantity"].Value);
                            decimal sum = Convert.ToDecimal(row.Cells["Sum"].Value);
                            // Получаем Service_id по наименованию из таблицы service
                            int serviceId = GetServiceIdFromDatabase(connection, serviceName);

                            // Изменяем значение параметра "@estimateId"
                            insertCommand.Parameters["@estimateId"].Value = id;
                            insertCommand.Parameters["@serviceId"].Value = serviceId;
                            insertCommand.Parameters["@quantity"].Value = quantity;
                            insertCommand.Parameters["@sum"].Value = sum;

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }







            ////СОЗДАНИЕ СМЕТЫ В ВИДЕ ДОКУМЕНТА

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string filePath = @"C:\Users\Алиса\Desktop\Диплом\Диплом\bin\Debug\document\estimate1.docx";



                    string getClientData = "SELECT id, Number, Number_type, Date FROM estimate WHERE id=@newId";
                    string getStaffData = @"SELECT CONCAT(Surname, ' ', first_name, ' ', Second_name) AS FullName from staff WHERE id=@staff_Id";

                    MySqlCommand cmd = new MySqlCommand(getClientData, connection);
                    cmd.Parameters.AddWithValue("@newId", id);
                    MySqlCommand getStaffDataCmd = new MySqlCommand(getStaffData, connection);
                    getStaffDataCmd.Parameters.AddWithValue("@staff_Id", staffID);


                    string staffFullName = string.Empty;

                    using (MySqlDataReader staffReader = getStaffDataCmd.ExecuteReader())
                    {
                        if (staffReader.Read())
                        {
                            staffFullName = staffReader.GetString("FullName");

                            string[] names = staffFullName.Split(' ');
                            string formattedName = string.Empty;

                            formattedName += names[1].Substring(0, 1) + "."; // Имя
                            formattedName += names[2].Substring(0, 1) + "."; // Отчество
                            formattedName += names[0]; // Фамилия


                            staffFullName = formattedName;
                        }
                        staffReader.Close();
                    }


                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        //int id = reader.GetInt32("id");
                        int number = reader.GetInt32("Number");
                        int numberType = reader.GetInt32("Number_type");
                        DateTime date = reader.GetDateTime("Date");
                        string formattedDate = date.ToString("dd.MM.yyyy");





                        string newFileName = "Смета " + number.ToString() + "." + numberType.ToString() + " от " + formattedDate + " new";

                        // Duplicate the main file
                        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName + ".docx");
                        File.Copy(filePath, newFilePath);


                        Dictionary<string, string> hashMap = new Dictionary<string, string>()
                                                    {
                                                        {"{id таблица estimate}", id.ToString()},
                                                        {"{number таблица estimate}", number.ToString() + "/" + numberType.ToString()},
                                                        {"{Date таблица estimate}", formattedDate},
                                                        {"{address таблица alttabn}", label1.Text},
                                                        {"{work_name таблица alttabn}", label3.Text},
                                                        {"{Surname + first_name + Second_name таблица staff}", staffFullName},
                                                        {"{total_sum таблица estimate}", totalSum.ToString()}
                                                    };

                        foreach (KeyValuePair<string, string> pair in hashMap)
                        {
                            ReplaceWordInDocument(newFilePath, pair.Key, pair.Value);
                        }



                        // Open the Word document
                        Word.Application wordApp = new Word.Application();
                        Document doc = wordApp.Documents.Open(newFilePath);

                        // Find the table in the document (assuming it's the first table)
                        Word.Table table = doc.Tables[1];

                        // Подсчитываем количество строк, которое нужно добавить
                        int rowsToAdd = dataGridView1.Rows.Count - 1;

                        // Добавляем необходимое количество строк в таблицу
                        for (int i = 0; i < rowsToAdd; i++)
                        {
                            table.Rows.Add(table.Rows[2]); // Копируем вторую строку и добавляем ее в конец таблицы
                        }


                        // Populate the table with data from DataGridView
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                table.Cell(i + 2, j + 1).Range.Text = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            }
                        }



                        // Save and close the modified document
                        doc.Save();
                        doc.Close();


                        Form10 form10 = new Form10();
                        // Установка значения FilePath в Form10
                        form10.newFilePath = newFilePath;
                        // Установка значения Label3Text в Form10
                        form10.Label3Text = label3.Text;
                        // Установка значения Label1Text в Form10
                        form10.Label1Text = label1.Text;
                        // Установка значения TextBoxValue в Form10
                        form10.TextBoxValue = textBox1.Text;
                        form10.Show();

                    }

                    reader.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

   

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["ID"].Index)
            {
                // Проверяем, что изменения произошли в столбце "Номер"
                UpdateNumberColumn();
            }
        }

        private void UpdateNumberColumn()
        {
            // Проходимся по всем строкам таблицы и обновляем значения столбца "Номер"
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                row.Cells["ID"].Value = i + 1;
            }
        }

        private void главноеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5.Show();
        }

        private void объектыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form20 form20 = new Form20();
            form20.HideLinkLabel();
            form20.Show();
        }

        private void материалыИОборудованиеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.HideLinkLabel();
            form14.Show();
        }

        private void клиентыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();
            form13.HideLinkLabel();
            form13.Show();
        }

        private void договораToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form15 form15 = new Form15();
            form15.HideLinkLabel();
            form15.Show();
        }

        private void сметыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form16 form16 = new Form16();
            form16.HideLinkLabel();
            form16.Show();
        }

        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form17 form17 = new Form17();
            form17.HideLinkLabel();
            form17.Show();
        }
    }
}
