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

namespace Task_Tracker
{
    public partial class NewWorker : Form
    {
        public NewWorker()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // MessageBox.Show(GetConnectionString());
            OpenSqlConnection();
        }

        private static void OpenSqlConnection()
        {
            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Connection Open ! ");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,"Can not open connection ! ");
                }
            }
        }

        static private string GetConnectionString()
        {
            // To avoid storing the connection string in your code, 
            // you can retrieve it from a configuration file, using the 
            // System.Configuration.ConfigurationSettings.AppSettings property 
            return "Data Source=(LocalDB)\v11.0;AttachDbFilename=\"C:\\Users\\Arma\\Documents\\Visual Studio 2013\\Projects\\TaskTracker\\Task Tracker\\Database1.mdf\";Integrated Security=True;";
            //return "Data Source=(local);Initial Catalog=AdventureWorks;"
            //    + "Integrated Security=SSPI;";
        }
    }
}
