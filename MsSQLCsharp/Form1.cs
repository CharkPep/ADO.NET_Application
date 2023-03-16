using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace MsSQLCsharp
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlConnection NorthWindConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TESTDB"].ConnectionString);

            sqlConnection.Open();

            NorthWindConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString);

            NorthWindConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                //MessageBox.Show("Connection was opened");
            }

            var data = new SqlDataAdapter("SELECT * FROM Products", NorthWindConnection);
            var dataSet = new DataSet();
            data.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var command = new SqlCommand($"INSERT INTO Students VALUES(@Name, @Surname, @Date, @City, @Phone, @Email)", sqlConnection);

            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Surname", textBox2.Text);
            command.Parameters.AddWithValue("Date", textBox3.Text);
            command.Parameters.AddWithValue("City", textBox4.Text);
            command.Parameters.AddWithValue("Phone", textBox5.Text);
            command.Parameters.AddWithValue("Email", textBox6.Text);

            MessageBox.Show($"{command.ExecuteNonQuery().ToString()} row was effected.");


        }

        private void button2_Click(object sender, EventArgs e)
        {
            var data = new SqlDataAdapter(textBox7.Text, NorthWindConnection);
            var dataSet = new DataSet();
            try
            {
                data.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            SqlDataReader sqlDataReader = null;
            try
            {
                var Command = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products", NorthWindConnection);
                sqlDataReader = Command.ExecuteReader();
                ListViewItem ListItem = null;
                while (sqlDataReader.Read())
                {
                    ListItem = new ListViewItem(new string[] { Convert.ToString(sqlDataReader["ProductName"]), Convert.ToString(sqlDataReader["QuantityPerUnit"]), Convert.ToString(sqlDataReader["UnitPrice"]) });
                    listView1.Items.Add(ListItem);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlDataReader != null && sqlDataReader.IsClosed)
                {
                    
                }
                sqlDataReader = null;
            }
        }

        private void x(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'"; 
        }
    }
}
