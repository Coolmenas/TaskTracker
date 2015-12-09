using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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

        private void LoadDataGridView(int level)
        {
            DataTable dt = new DataTable();
            if (level == 0)
            {
                con.SqlQuery("SELECT Name, Surname, Telephone FROM workers WHERE TeamId  = \"" + treeView1.SelectedNode.Name.ToString() + '"');
                reader = con.QueryEx();
            }
            else if (level == 1)
            {
                con.SqlQuery("SELECT  Name, Description, DateDue FROM tasks WHERE WorkerId  = \"" + treeView1.SelectedNode.Name.ToString() + '"');
                reader = con.QueryEx();
            }
            else
            {
                con.SqlQuery("SELECT  Name, Description, DateDue FROM tasks WHERE WorkerId  = \"" + treeView1.SelectedNode.Parent.Name.ToString() + '"');
                reader = con.QueryEx();
            }
            dt.Load(reader);
            con.ConnectionClose();
            dataGridView1.DataSource = dt;
        }

        public void LoadTreeView()
        {
            treeView1.Nodes.Clear();
            //-----------------------------------------------Fill first level--------------------------------------------------------------------
            con.SqlQuery("SELECT * FROM TEAMS ");
            reader = con.QueryEx();
            while (reader.Read())
            {
                treeView1.Nodes.Add(reader["Id"].ToString(),reader["Name"].ToString());
            }
            con.ConnectionClose();
            //------------------------------------------------------Fill second level-------------------------------------------------------------
            con.SqlQuery("SELECT * FROM workers ");
            reader = con.QueryEx();
            while (reader.Read())
            {
                treeView1.Nodes[reader["TeamId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
            }
            con.ConnectionClose();
            //------------------------------------------------------------Fill third level-------------------------------------------------------
            con.SqlQuery("SELECT workers.TeamId, tasks.WorkerId, tasks.Name, tasks.Id FROM tasks INNER JOIN workers on tasks.WorkerId = workers.Id ");
            reader = con.QueryEx();
            while (reader.Read())
            {
                treeView1.Nodes[reader["TeamId"].ToString()].Nodes[reader["WorkerId"].ToString()].Nodes.Add(reader["Id"].ToString(), reader["Name"].ToString());
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
            var selectedNode = treeView1.SelectedNode;
            if (selectedNode.Level == 0)
            {
                LoadDataGridView(selectedNode.Level);  
            }
            else
            {
                if (selectedNode.Level == 1)
                {
                    LoadDataGridView(selectedNode.Level);
                }
                else
                {
                    LoadDataGridView(selectedNode.Level);
                }
            }    
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            e.Graphics.DrawImage(bm, 0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0)
            {
                NewTeam editTeam = new NewTeam(treeView1.SelectedNode.Name);
                editTeam.Show();
            }
            else if (treeView1.SelectedNode.Level == 1)
            {
                EditWorker editWorker = new EditWorker(treeView1.SelectedNode.Name);
                editWorker.Show();
            }
            else
            {
                EditTask editTask = new EditTask(treeView1.SelectedNode.Name);
                editTask.Show();
            }
        }

        public void refresh()
        {
            string path = treeView1.SelectedNode.FullPath;
           
            LoadTreeView();

            var path_list = path.Split('\\').ToList();
            foreach (TreeNode node in treeView1.Nodes)
                if (path_list.Count > 0) 
                    if (node.Text == path_list[0])
                        ExpandOnPath(node, path_list);
            
            try
            {
                    LoadDataGridView(treeView1.SelectedNode.Level);
            }
            catch (Exception)
            {
                
            }
            
        }

        private void ExpandOnPath(TreeNode node, List<string> path)
        {
            path.RemoveAt(0);

            node.Expand();

            if (path.Count == 0)
                return;
            TreeNode selected = treeView1.Nodes[0];
            foreach (TreeNode mynode in node.Nodes)
                if (mynode.Text == path[0])
                {
                    selected = mynode;
                    ExpandOnPath(mynode, path); //recursive call
                    break;
                }
            treeView1.SelectedNode = selected;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0)
            {
                con.SqlQuery("DELETE teams.*, workers.*, tasks.* FROM teams LEFT JOIN workers ON teams.Id = workers.TeamId LEFT JOIN tasks ON tasks.workerId = workers.Id WHERE teams.Id = \"" + treeView1.SelectedNode.Name + '"');
                con.NonQueryEx();

                MessageBox.Show("Team " + treeView1.SelectedNode.Name + " deleted");
                var principalForm = Application.OpenForms.OfType<Form1>().Single();
                principalForm.refresh();
            }
            else if (treeView1.SelectedNode.Level == 1)
            {
                con.SqlQuery("DELETE FROM tasks WHERE WorkerId = \"" + treeView1.SelectedNode.Name + '"');
                con.NonQueryEx();

                con.SqlQuery("DELETE FROM workers WHERE Id = \"" + treeView1.SelectedNode.Name + '"');
                con.NonQueryEx();

                MessageBox.Show("Worker " + treeView1.SelectedNode.Name + " deleted");
                var principalForm = Application.OpenForms.OfType<Form1>().Single();
                principalForm.refresh();
            }
            else
            {
                con.SqlQuery("DELETE FROM tasks WHERE Id = \"" + treeView1.SelectedNode.Name + '"');
                con.NonQueryEx();
                MessageBox.Show("Task " + treeView1.SelectedNode.Name + " deleted");
                var principalForm = Application.OpenForms.OfType<Form1>().Single();
                principalForm.refresh();
                
            }
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Author: Armantas Vaskelis\n Purpose:  SCIIL Baltic test");
        }
    }
}
