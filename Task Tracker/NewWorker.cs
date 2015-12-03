﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_Tracker
{
    public partial class NewWorker : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        public NewWorker()
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
            //comboBoxValues.Add(1, "dfdfdf");
            //comboBoxValues.Add(2, "d23a");
            //comboBoxValues.Add(4, "d124156787s");
           

            // Get combobox selection (in handler)
            //string value = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Key;
            



            while (reader.Read()){
                comboBoxValues.Add(reader[0].ToString(), reader["Name"].ToString());
                //MessageBox.Show(reader["Id"].ToString());
            }
            con.ConnectionClose();
            //MessageBox.Show(reader["Name"].ToString());
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
            con.SqlQuery("INSERT INTO workers (Name, Surname, Born, Joined, Telephone, Address, TeamId) VALUES(@NameP, @SurnameP, @BornP, @JoinedP, @TelephoneP, @AddressP, @TeamIdP)");
            con.command.Parameters.AddWithValue("@NameP", textBox1.Text.Trim());
            con.command.Parameters.AddWithValue("@SurnameP", textBox2.Text.Trim());
            con.command.Parameters.AddWithValue("@BornP", dateTimePicker1.Value);
            con.command.Parameters.AddWithValue("@JoinedP", dateTimePicker2.Value);
            con.command.Parameters.AddWithValue("@TelephoneP", textBox3.Text.Trim());
            con.command.Parameters.AddWithValue("@AddressP", textBox4.Text.Trim());
            con.command.Parameters.AddWithValue("@TeamIdP", comboBox1.SelectedValue);

            con.NonQueryEx("Worker " + textBox1.Text.Trim() + " created");
            //MessageBox.Show("Worker " + textBox1.Text.Trim() + " created");
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
