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
    public partial class NewTeam : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        private string teamId;
        public NewTeam()
        {
            InitializeComponent();
            con = new SqlConnect();
        }

        public NewTeam(string teamId)
        {
            this.teamId = teamId;
            InitializeComponent();
            con = new SqlConnect();
            this.Text = "Edit Team";
            con.SqlQuery("SELECT * FROM teams WHERE Id = \""+teamId+'"');
            reader = con.QueryEx();
            while (reader.Read())
            {
                textBox1.Text = reader["Name"].ToString();
                dateTimePicker1.Value = DateTime.Parse(reader["Date"].ToString());
                textBox2.Text = reader["Description"].ToString();
            }
            con.ConnectionClose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Text == "Edit Team")
            {
                con.SqlQuery("UPDATE Teams SET  Name = @NameP,  Date  = @DateP, Description = @DescriptionP WHERE Id = \""+teamId+'"');
                con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
                con.command.Parameters.AddWithValue("@DateP", dateTimePicker1.Value);
                con.command.Parameters.AddWithValue("@DescriptionP", textBox2.Text);
                con.NonQueryEx();
                MessageBox.Show("Team " + textBox1.Text.Trim() + " updated");
            }
            else
            {
                con.SqlQuery("INSERT INTO Teams (Name, Date, Description) VALUES(@NameP,@DateP,@DescriptionP)");
                con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
                con.command.Parameters.AddWithValue("@DateP", dateTimePicker1.Value);
                con.command.Parameters.AddWithValue("@DescriptionP", textBox2.Text);
                con.NonQueryEx();
                MessageBox.Show("Team " + textBox1.Text.Trim() + " created");
            }
            var principalForm = Application.OpenForms.OfType<Form1>().Single();
            principalForm.refresh();
        }
    }
}
