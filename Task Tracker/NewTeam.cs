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
        public NewTeam()
        {
            InitializeComponent();
            con = new SqlConnect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.SqlQuery ("INSERT INTO Teams (Name, Date, Description) VALUES(@NameP,@DateP,@DescriptionP)");
            con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
            con.command.Parameters.AddWithValue("@DateP", dateTimePicker1.Value);
            con.command.Parameters.AddWithValue("@DescriptionP", textBox2.Text);
            con.NonQueryEx();
            MessageBox.Show("Team " + textBox1.Text.Trim() + " created");
        }
    }
}
