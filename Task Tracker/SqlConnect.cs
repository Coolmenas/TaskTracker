using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Task_Tracker
{
    class SqlConnect
    {
        private MySqlConnection _connection;
        public MySqlCommand command;

        public   SqlConnect()
        {
            _connection = new MySqlConnection("server=localhost;user id=armantas;Password=armantas;database=task_manager;persist security info=False;Convert Zero Datetime=True");
         
        }

        public void SqlQuery(string queryText)
        {
            command = new MySqlCommand();
            command.Connection = _connection;
            command.CommandText = queryText;
        }

        public MySqlDataReader QueryEx()
        {
            MySqlDataReader msqlReader = null;
            try
            {
                //open the connection
                _connection.Open();
                //use a DataReader to process each record
                msqlReader = command.ExecuteReader();
                return msqlReader;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
            return msqlReader;
        }

        public void NonQueryEx()
        {
            try
            {
                _connection.Open();
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
        public void NonQueryEx(string message)
        {
            try
            {
                _connection.Open();
                command.ExecuteNonQuery();
                MessageBox.Show(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public void ConnectionClose()
        {
            command.Connection.Close();
        }

        

        public void TestMysqlConnection()
        {

            string connectionString = "server=localhost;user id=armantas;Password=armantas;database=task_manager;persist security info=False";
            //define the connection reference and initialize it
            MySql.Data.MySqlClient.MySqlConnection msqlConnection = null;
            msqlConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            //define the command reference
            MySql.Data.MySqlClient.MySqlCommand msqlCommand = new MySql.Data.MySqlClient.MySqlCommand();
            //define the connection used by the command object
            msqlCommand.Connection = msqlConnection;
            //define the command text

            msqlCommand.CommandText = "SELECT * FROM workers;";

            try
            {
                //open the connection
                msqlConnection.Open();
                //use a DataReader to process each record
                MySql.Data.MySqlClient.MySqlDataReader msqlReader = msqlCommand.ExecuteReader();

                while (msqlReader.Read())
                {
                    MessageBox.Show(msqlReader["Name"].ToString());
                    //do something with each record
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                //do something with the exception
            }
            finally
            {
                //always close the connection
                msqlConnection.Close();
            }
        }

        

    }
}
