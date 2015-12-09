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
    public partial class EditWorker : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        private string workerId;
        public EditWorker(string workerId)
        {
            this.workerId = workerId;
            InitializeComponent();
            con = new SqlConnect();
            loadData(); 
        }

        private void loadData()
        {
            loadTeams();// loads the combobox for team selection
//--------------------------------fills other data--------------------------------
            con.SqlQuery("SELECT * FROM workers WHERE Id = \"" + workerId + '"');
            reader = con.QueryEx();
            while (reader.Read())
            {
                textBox1.Text = reader["Name"].ToString();
                textBox2.Text = reader["Surname"].ToString();
                textBox3.Text = reader["Address"].ToString();
                textBox4.Text = reader["Telephone"].ToString();
                dateTimePicker1.Value = DateTime.Parse(reader["Born"].ToString());
                dateTimePicker2.Value = DateTime.Parse(reader["Joined"].ToString());
                comboBox1.SelectedValue = reader["TeamId"].ToString();
            }
            con.ConnectionClose();
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (verifyData() == false)
            {
                return;
            }
            con.SqlQuery("UPDATE Workers SET  Name = @NameP, Surname = @SurnameP,  Born = @BornP , Joined = @JoinedP, Telephone = @TelephoneP, Address = @AddressP, TeamId = @TeamIdP WHERE Id = \"" + workerId + '"');
            con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
            con.command.Parameters.AddWithValue("@SurnameP", textBox2.Text.Trim());
            con.command.Parameters.AddWithValue("@BornP", dateTimePicker1.Value);
            con.command.Parameters.AddWithValue("@JoinedP", dateTimePicker2.Value);
            con.command.Parameters.AddWithValue("@TelephoneP", textBox4.Text.Trim());
            con.command.Parameters.AddWithValue("@AddressP", textBox3.Text.Trim());
            con.command.Parameters.AddWithValue("@TeamIdP", comboBox1.SelectedValue);

            con.NonQueryEx();
            MessageBox.Show("Worker " + textBox1.Text.Trim() + " updated");

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
            if (textBox4.Text.Length <= 5 || textBox4.Text.Length >= 12)
            {
                MessageBox.Show("Telephone number too long or too short");
                return false;
            }
            else return true;
        }

    }
}
