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
    public partial class Form1 : Form
    {
        private SqlConnect con;
        private MySqlDataReader reader;
        public Form1()
        {
            InitializeComponent();
            con = new SqlConnect();
            LoadTreeView();
            
            
        }

        private void LoadDataGridView()
        {
            string level = "teams";
            //if (treeView1.SelectedNode.Level == 0) level = "teams";
           // else if (treeView1.SelectedNode.Level == 1) level = "workers";
           // else level = "tasks";
            con.SqlQuery("SELECT teams.*, workers.*, tasks.* " +
                         "FROM teams INNER JOIN workers on workers.TeamId= teams.Id "+
                         " INNER JOIN tasks on workers.TeamId= teams.Id "+
                         "WHERE " + level + ".id = \"" + treeView1.SelectedNode.Name.ToString() + '"');
            reader = con.QueryEx();

            //Dictionary<string, string> dataGridViewValues = new Dictionary<string, string>();
            //while (reader.Read())
            //{
            //    dataGridViewValues.Add(reader["Id"].ToString(), reader["Name"].ToString());
            //    //treeView1.Nodes[reader["TeamId"].ToString()].Nodes[reader["WorkerId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
            //    //MessageBox.Show(reader["Id"].ToString());
            //}
            DataTable dt = new DataTable();
            dt.Load(reader);
            con.ConnectionClose();
            dataGridView1.DataSource = dt;
            //throw new NotImplementedException();
        }

        private void LoadTreeView()
        {
            //-----------------------------------------------Fill first level--------------------------------------------------------------------
            con.SqlQuery("SELECT * FROM TEAMS ");
            reader = con.QueryEx();

            while (reader.Read())
            {
                treeView1.Nodes.Add(reader["Id"].ToString(),reader["Name"].ToString());
                //MessageBox.Show(reader["Id"].ToString());
            }
            con.ConnectionClose();


            //------------------------------------------------------Fill second level-------------------------------------------------------------
            con.SqlQuery("SELECT * FROM workers ");
            reader = con.QueryEx();

            while (reader.Read())
            {
                treeView1.Nodes[reader["TeamId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
                //MessageBox.Show(reader["Id"].ToString());
            }
            con.ConnectionClose();


            //------------------------------------------------------------Fill third level-------------------------------------------------------
            con.SqlQuery("SELECT workers.TeamId, tasks.WorkerId, tasks.Name, tasks.Id FROM tasks INNER JOIN workers on tasks.WorkerId = workers.Id ");
            reader = con.QueryEx();

            while (reader.Read())
            {
                treeView1.Nodes[reader["TeamId"].ToString()].Nodes[reader["WorkerId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
                //MessageBox.Show(reader["Id"].ToString());
            }
            con.ConnectionClose();

            
        }

        private void workerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewWorker nW = new NewWorker();
            nW.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void teamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTeam nT = new NewTeam();
            nT.Show();
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTask nT = new NewTask();
            nT.Show();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LoadDataGridView();
           // MessageBox.Show(treeView1.SelectedNode.Name.ToString());
            DataTable dt = new DataTable();
            var selectedNode = treeView1.SelectedNode;
            if (selectedNode.Level == 0)
            {
                
                con.SqlQuery("SELECT * FROM teams ");
                reader = con.QueryEx();

                //Dictionary<string, string> dataGridViewValues = new Dictionary<string, string>();
                //while (reader.Read())
                //{
                //    dataGridViewValues.Add(reader["Id"].ToString(), reader["Name"].ToString());
                //    //treeView1.Nodes[reader["TeamId"].ToString()].Nodes[reader["WorkerId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
                //    //MessageBox.Show(reader["Id"].ToString());
                //}

                dt.Load(reader);  
                con.ConnectionClose();
               // dataGridView1.DataSource = dt;
                
                ////use binding source to hold dummy data
                //BindingSource binding = new BindingSource();
                //binding.DataSource = dataGridViewValues;

                ////bind datagridview to binding source
                //dataGridView1.DataSource = binding;
                //MessageBox.Show("team");
            }
            else
            {
                if (selectedNode.Level == 1)
                {
                    
                }
                else
                {
                    MessageBox.Show("task");
                }
            }    
        }
    }
}
