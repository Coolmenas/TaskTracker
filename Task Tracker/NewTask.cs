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

namespace Task_Tracker
{
    public partial class NewTask : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        public NewTask()
        {
            InitializeComponent();
            con = new SqlConnect();
            loadTeams();
        }

        private void loadTeams()
        {

            con.SqlQuery("SELECT * FROM TEAMS ");
            reader = con.QueryEx();

            Dictionary<string, string> comboBoxValues = new Dictionary<string, string>();

            while (reader.Read())
            {
                comboBoxValues.Add(reader[0].ToString(), reader["Name"].ToString());
            }
            con.ConnectionClose();
            comboBox1.DataSource = new BindingSource(comboBoxValues, null);
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";

            loadWorkers();
        }

        private void loadWorkers()
        {
            con.SqlQuery("SELECT * FROM workers WHERE TeamId = \""+comboBox1.SelectedValue+'"');
            reader = con.QueryEx();

            Dictionary<string, string> comboBoxValues = new Dictionary<string, string>();

            comboBox2.Hide();
            button1.Enabled= false;
            while (reader.Read())
            {
                comboBoxValues.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            con.ConnectionClose();

            if (comboBoxValues.Count > 0)
            {
                comboBox2.DataSource = new BindingSource(comboBoxValues, null);
                comboBox2.DisplayMember = "Value";
                comboBox2.ValueMember = "Key";
                comboBox2.Show();
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadWorkers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (verifyData() == false ){
                return;
            }
            con.SqlQuery("INSERT INTO tasks (Name, Description, DateCreated, DateDue, WorkerId) VALUES(@Name, @Description, @DateCreated, @DateDue, @WorkerId)");
            con.command.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
            con.command.Parameters.AddWithValue("@Description", textBox2.Text.Trim());
            con.command.Parameters.AddWithValue("@DateCreated", dateTimePicker1.Value);
            con.command.Parameters.AddWithValue("@DateDue", dateTimePicker2.Value);
            con.command.Parameters.AddWithValue("@WorkerId", comboBox2.SelectedValue);

            con.NonQueryEx("Task " + textBox1.Text.Trim() + " created");
            var principalForm = Application.OpenForms.OfType<Form1>().Single();
            principalForm.refresh();
        }

        private bool verifyData()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter a name for the task");
                return false;
            }
            else return true;
        }
    }
}
