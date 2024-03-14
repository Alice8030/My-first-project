using Google.Protobuf.WellKnownTypes;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace Диплом
{
    public partial class Form4 : Form
    {
        public Form mainForm;
        public Form4()
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

        private void Form4_Load(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart2.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                "SUM(CASE WHEN Name_obj = 'На выполнение монтажных работ' THEN 1 ELSE 0 END) AS MontageContracts, " +
                "SUM(CASE WHEN Name_obj = 'На годовое сервисное (техническое) обслуживание' THEN 1 ELSE 0 END) AS ServiceContracts, " +
                "COUNT(*) AS TotalContracts " +
                "FROM contract ";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int montageContracts = reader.GetInt32("MontageContracts");
                    int serviceContracts = reader.GetInt32("ServiceContracts");
                    int totalContracts = reader.GetInt32("TotalContracts");

                    // Здесь можно передать полученные значения для отображения на графике

                    chart1.Series.Clear();

                    // Серия для контрактов "на монтаж"
                    Series montageSeries = new Series("Монтажные работы");
                    montageSeries.Points.AddXY("Контракты за весь промежуток времени", montageContracts);

                    // Серия для контрактов "на обслуживание"
                    Series serviceSeries = new Series("Обслуживание");
                    serviceSeries.Points.AddXY("", serviceContracts);

                    // Серия для общего количества контрактов
                    Series totalSeries = new Series("Общее количество");
                    totalSeries.Points.AddXY("", totalContracts);

                    chart1.Series.Add(montageSeries);
                    chart1.Series.Add(serviceSeries);
                    chart1.Series.Add(totalSeries);
                }

                reader.Close();

            }


        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

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

               

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart2.Visible = false;

            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                "SUM(CASE WHEN Name_obj = 'На выполнение монтажных работ' THEN 1 ELSE 0 END) AS MontageContracts, " +
                "SUM(CASE WHEN Name_obj = 'На годовое сервисное (техническое) обслуживание' THEN 1 ELSE 0 END) AS ServiceContracts, " +
                "COUNT(*) AS TotalContracts " +
                "FROM contract ";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int montageContracts = reader.GetInt32("MontageContracts");
                    int serviceContracts = reader.GetInt32("ServiceContracts");
                    int totalContracts = reader.GetInt32("TotalContracts");

                    // Здесь можно передать полученные значения для отображения на графике

                    chart1.Series.Clear();

                    // Серия для контрактов "на монтаж"
                    Series montageSeries = new Series("Монтажные работы");
                    montageSeries.Points.AddXY("Контракты за весь промежуток времени", montageContracts);

                    // Серия для контрактов "на обслуживание"
                    Series serviceSeries = new Series("Обслуживание");
                    serviceSeries.Points.AddXY("", serviceContracts);

                    // Серия для общего количества контрактов
                    Series totalSeries = new Series("Общее количество");
                    totalSeries.Points.AddXY("", totalContracts);

                    chart1.Series.Add(montageSeries);
                    chart1.Series.Add(serviceSeries);
                    chart1.Series.Add(totalSeries);
                }

                reader.Close();

            }
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form20 form20 = new Form20();
            form20.mainForm = this;
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

        private void button2_Click(object sender, EventArgs e)
        {
            
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart2.Visible = false;

            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;


            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                    "DATE(Date) AS ContractDate, " +
                    "SUM(CASE WHEN Name_obj = 'На выполнение монтажных работ' THEN 1 ELSE 0 END) AS MontageContracts, " +
                    "SUM(CASE WHEN Name_obj = 'На годовое сервисное (техническое) обслуживание' THEN 1 ELSE 0 END) AS ServiceContracts, " +
                    "COUNT(*) AS TotalContracts " +
                    "FROM contract " +
                    "WHERE Date >= @StartDate AND Date <= @EndDate " +
                    "GROUP BY ContractDate";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                MySqlDataReader reader = command.ExecuteReader();


                if (reader.HasRows)
                {
                    chart1.Series.Clear();

                    // Серия для контрактов "на монтаж"
                    Series montageSeries = new Series("Монтажные работы");

                    // Серия для контрактов "на обслуживание"
                    Series serviceSeries = new Series("Обслуживание");

                    // Серия для общего количества контрактов
                    Series totalSeries = new Series("Общее количество");

                    while (reader.Read())
                    {
                        DateTime contractDate = reader.GetDateTime("ContractDate");
                        int montageContracts = reader.GetInt32("MontageContracts");
                        int serviceContracts = reader.GetInt32("ServiceContracts");
                        int totalContracts = reader.GetInt32("TotalContracts");

                        montageSeries.Points.AddXY(contractDate, montageContracts);
                        serviceSeries.Points.AddXY(contractDate, serviceContracts);
                        totalSeries.Points.AddXY(contractDate, totalContracts);
                    }

                    reader.Close();

                    
                    chart1.Series.Add(montageSeries);
                    chart1.Series.Add(serviceSeries);
                    chart1.Series.Add(totalSeries);

                    chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy"; // Формат даты на оси X
                }

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart2.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button3.Visible = false;
            button4.Visible = false;

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT " +
                "SUM(CASE WHEN Name_obj = 'На выполнение монтажных работ' THEN 1 ELSE 0 END) AS MontageContracts, " +
                "SUM(CASE WHEN Name_obj = 'На годовое сервисное (техническое) обслуживание' THEN 1 ELSE 0 END) AS ServiceContracts, " +
                "COUNT(*) AS TotalContracts " +
                "FROM contract ";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int montageContracts = reader.GetInt32("MontageContracts");
                    int serviceContracts = reader.GetInt32("ServiceContracts");
                    int totalContracts = reader.GetInt32("TotalContracts");

                    // Здесь можно передать полученные значения для отображения на графике

                    chart1.Series.Clear();

                    // Серия для контрактов "на монтаж"
                    Series montageSeries = new Series("Монтажные работы");
                    montageSeries.Points.AddXY("Контракты за весь промежуток времени", montageContracts);

                    // Серия для контрактов "на обслуживание"
                    Series serviceSeries = new Series("Обслуживание");
                    serviceSeries.Points.AddXY("", serviceContracts);

                    // Серия для общего количества контрактов
                    Series totalSeries = new Series("Общее количество");
                    totalSeries.Points.AddXY("", totalContracts);

                    chart1.Series.Add(montageSeries);
                    chart1.Series.Add(serviceSeries);
                    chart1.Series.Add(totalSeries);
                }

                reader.Close();

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = true;
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = true;
            button4.Visible = true;

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Запрос для получения общих сумм контрактов для каждого вида работы
                string query = @"SELECT c.Name_obj, SUM(e.Total_sum) AS TotalSum
                        FROM contract c
                        INNER JOIN estimate e ON e.Contract_id = c.id
                        GROUP BY c.Name_obj";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                // Создание серий данных для каждого вида работы
                Series montageSeries = new Series("Монтажные работы");
                Series serviceSeries = new Series("Обслуживание");
                Series totalSeries = new Series("Общая сумма");

                while (reader.Read())
                {
                    string nameObj = reader.GetString("Name_obj");
                    double totalSum = reader.GetDouble("TotalSum");

                    if (nameObj == "На выполнение монтажных работ")
                    {
                        montageSeries.Points.AddXY("", totalSum);
                    }
                    else if (nameObj == "На годовое сервисное (техническое) обслуживание")
                    {
                        serviceSeries.Points.AddXY("Общая сумма за весь промежуток времени", totalSum);
                    }

                    totalSeries.Points.AddXY("", totalSum);
                }

                reader.Close();

                // Очистка графика перед добавлением серий данных
                chart2.Series.Clear();

                // Добавление серий данных на график
                chart2.Series.Add(montageSeries);
                chart2.Series.Add(serviceSeries);
                chart2.Series.Add(totalSeries);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = true;

            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Запрос для получения общих сумм контрактов для каждого вида работы и даты контрактов
                string query = @"SELECT c.Name_obj, c.Date, SUM(e.Total_sum) AS TotalSum
                        FROM contract c
                        INNER JOIN estimate e ON e.Contract_id = c.id
                        WHERE c.Date >= @StartDate AND c.Date <= @EndDate
                        GROUP BY c.Name_obj, c.Date";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                MySqlDataReader reader = command.ExecuteReader();

                // Создание серий данных для каждого вида работы
                Series montageSeries = new Series("Монтажные работы");
                Series serviceSeries = new Series("Обслуживание");
                Series totalSeries = new Series("Общая сумма");

                while (reader.Read())
                {
                    string nameObj = reader.GetString("Name_obj");
                    DateTime date = reader.GetDateTime("Date");
                    double totalSum = reader.GetDouble("TotalSum");

                    if (nameObj == "На выполнение монтажных работ")
                    {
                        montageSeries.Points.AddXY(date, totalSum);
                    }
                    else if (nameObj == "На годовое сервисное (техническое) обслуживание")
                    {
                        serviceSeries.Points.AddXY(date, totalSum);
                    }

                    totalSeries.Points.AddXY(date, totalSum);
                }

                reader.Close();

                // Очистка графика перед добавлением серий данных
                chart2.Series.Clear();

                // Добавление серий данных на график
                chart2.Series.Add(montageSeries);
                chart2.Series.Add(serviceSeries);
                chart2.Series.Add(totalSeries);

                // Установка промежутка дат на оси X
                chart2.ChartAreas[0].AxisX.Minimum = startDate.ToOADate();
                chart2.ChartAreas[0].AxisX.Maximum = endDate.ToOADate();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart1.Visible = false;
            chart2.Visible = true;

            dateTimePicker1.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            dateTimePicker2.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

            string connectionString = "server=localhost;database=diplom_alice;uid=root;password=alice.21";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Запрос для получения общих сумм контрактов для каждого вида работы
                string query = @"SELECT c.Name_obj, SUM(e.Total_sum) AS TotalSum
                        FROM contract c
                        INNER JOIN estimate e ON e.Contract_id = c.id
                        GROUP BY c.Name_obj";

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                // Создание серий данных для каждого вида работы
                Series montageSeries = new Series("Монтажные работы");
                Series serviceSeries = new Series("Обслуживание");
                Series totalSeries = new Series("Общая сумма");

                while (reader.Read())
                {
                    string nameObj = reader.GetString("Name_obj");
                    double totalSum = reader.GetDouble("TotalSum");

                    if (nameObj == "На выполнение монтажных работ")
                    {
                        montageSeries.Points.AddXY("", totalSum);
                    }
                    else if (nameObj == "На годовое сервисное (техническое) обслуживание")
                    {
                        serviceSeries.Points.AddXY("Общая сумма за весь промежуток времени", totalSum);
                    }

                    totalSeries.Points.AddXY("", totalSum);
                }

                reader.Close();

                // Очистка графика перед добавлением серий данных
                chart2.Series.Clear();

                // Добавление серий данных на график
                chart2.Series.Add(montageSeries);
                chart2.Series.Add(serviceSeries);
                chart2.Series.Add(totalSeries);

            }
        }
    }
}