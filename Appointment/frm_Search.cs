using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Appointment
{
    public partial class frm_Search : Form
    {
        SqlConnection con;
        
        public frm_Search()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Create_Appointment ms = new Create_Appointment();
            ms.Show();
           
        }
      

        private void frm_Search_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DatabaseStringClass.dbstring);
            con.Open();
          
            var date1 = Convert.ToDateTime(dateTimePicker1.Value.ToString());
            var date2 = Convert.ToDateTime( dateTimePicker2.Value.ToString());

            if (date1<=date2)
            {
                string commandString = "select StartDateTime, EndDateTime, Patient, Doctor from appointments where  StartDateTime>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:MM") + "' and EndDateTime<='" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:MM") + "' ";
                SqlDataAdapter dataadapter = new SqlDataAdapter(commandString, con);
                DataSet ds = new DataSet();
                dataadapter.Fill(ds, "appointments");

                if (ds.Tables[0].Rows.Count > 0)
                {
                 dataGridView1.DataSource = ds.Tables["appointments"];
                }
                else
                {
                MessageBox.Show("No Records Found");
                    this.dateTimePicker1.CustomFormat = " ";
                    this.dateTimePicker2.CustomFormat = " ";
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                con.Close();

            }
            else
            {
                MessageBox.Show("User should not be able to search appointments with invalid date ranges");
                dateTimePicker1.Text = string.Empty;
                dateTimePicker2.Text = string.Empty;
            }
           
           }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:MM";
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:MM";
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
        }

        private void dateTimePicker2_ValueChanged_1(object sender, EventArgs e)
        {
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
        }
    }
    }

