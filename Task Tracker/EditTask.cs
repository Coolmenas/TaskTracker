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
    public partial class EditTask : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        private string taskId;
        public EditTask(string taskId)
        {
            this.taskId = taskId;
            InitializeComponent();
            con = new SqlConnect();
            loadData();
            

        }

        private void loadData()
        {
            loadTeams();


            string workerId = "" ;
            string teamId = "";
            con.SqlQuery("SELECT * FROM tasks WHERE Id = \"" + taskId + '"');
            reader = con.QueryEx();
            while (reader.Read())
            {
                textBox1.Text = reader["Name"].ToString();
                textBox2.Text = reader["Description"].ToString();
                dateTimePicker1.Value = DateTime.Parse(reader["DateCreated"].ToString());
                dateTimePicker2.Value = DateTime.Parse(reader["DateDue"].ToString());
                workerId = reader["WorkerId"].ToString();
            }
            con.ConnectionClose();

            con.SqlQuery("SELECT * FROM workers WHERE Id = \"" + workerId + '"');
            reader = con.QueryEx();
            while (reader.Read())
            {
                teamId = reader["TeamId"].ToString();
            }
            con.ConnectionClose();

            comboBox1.SelectedValue = teamId;
            comboBox2.SelectedValue = workerId;
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
            con.SqlQuery("SELECT * FROM workers WHERE TeamId = \"" + comboBox1.SelectedValue + '"');
            //MessageBox.Show(comboBox1.SelectedValue.ToString());
            reader = con.QueryEx();

            Dictionary<string, string> comboBoxValues = new Dictionary<string, string>();

            comboBox2.Hide();
            button1.Enabled = false;
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
            if (verifyData() == false)
            {
                return;
            }
            con.SqlQuery("UPDATE Tasks SET  Name = @NameP, Description = @DescriptionP,  DateCreated = @DateCreatedP , DateDue = @DateDueP, WorkerId = @WorkerIdP WHERE Id = \"" + taskId + '"');
            con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
            con.command.Parameters.AddWithValue("@DescriptionP", textBox2.Text.Trim());
            con.command.Parameters.AddWithValue("@DateCreatedP", dateTimePicker1.Value);
            con.command.Parameters.AddWithValue("@DateDueP", dateTimePicker2.Value);
            con.command.Parameters.AddWithValue("@WorkerIdP", comboBox2.SelectedValue);

            con.NonQueryEx();
            MessageBox.Show("Task " + textBox1.Text.Trim() + " updated");

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
