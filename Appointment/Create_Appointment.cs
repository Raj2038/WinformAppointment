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
    public partial class Create_Appointment : Form
    {
        SqlConnection con;
        DataSet ds = new DataSet();
        int rowNumber;
        public Create_Appointment()
        {
            InitializeComponent();
        }

        private void Create_Appointment_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
            frm_Search frm = new frm_Search();
            frm.Hide();
            Bind();
            btn_Update.Enabled=false;
            button3.Enabled=false;
            button4.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DatabaseStringClass.dbstring);

            con.Open();

            var date1 = dateTimePicker1.Value;
            var date2 = dateTimePicker2.Value;

            //con = new SqlConnection(DatabaseStringClass.dbstring);
            //con.Open();
            string commandString = "select * from appointments where  StartDateTime>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:MM") + "' and EndDateTime<='" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:MM") + "' ";
            SqlDataAdapter dataadapter = new SqlDataAdapter(commandString, con);
            DataSet ds = new DataSet();
            dataadapter.Fill(ds);

            if (ds.Tables[0].Rows.Count>0)
            {

                MessageBox.Show("System already have a appointment for same time. Please select different time frame.");
                clearData();

            }
            else
            { 

            if (date1 < date2)
            {
                SqlCommand cmd = new SqlCommand("Insert Into appointments(StartDateTime,EndDateTime,Patient,Doctor) Values (@StartDateTime,@EndDateTime,@Patient,@Doctor)", con);

                    cmd.Parameters.AddWithValue("StartDateTime", dateTimePicker1.Value.ToString("yyyy-MM-dd HH:MM"));
                    cmd.Parameters.AddWithValue("EndDateTime", dateTimePicker2.Value.ToString("yyyy-MM-dd HH:MM"));

                    cmd.Parameters.AddWithValue("Patient", textBox1.Text);
                cmd.Parameters.AddWithValue("Doctor", textBox2.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Inserted Successfully!");
                Bind();

                clearData();

            }
            else
            {
                MessageBox.Show("User should not be able to create appointments with invalid date ranges");

                clearData();
            }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            frm_Search frm = new frm_Search();
            frm.Show();


        }

        private void clearData()
        {

            this.dateTimePicker1.CustomFormat = " ";
            this.dateTimePicker2.CustomFormat = " ";
            textBox1.Text = "";
            textBox2.Text = "";

        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DatabaseStringClass.dbstring);
            con.Open();
            SqlCommand cmd = new SqlCommand("Update appointments set StartDateTime=@StartDateTime,EndDateTime=@EndDateTime,Patient=@Patient,Doctor=@Doctor where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", rowNumber);
            cmd.Parameters.AddWithValue("@StartDateTime", dateTimePicker1.Text);
            cmd.Parameters.AddWithValue("@EndDateTime", dateTimePicker2.Text);
            cmd.Parameters.AddWithValue("@Patient", textBox1.Text);
            cmd.Parameters.AddWithValue("@Doctor", textBox2.Text);           
                   
            cmd.ExecuteNonQuery();
            MessageBox.Show("Patient Info Updated......");
            con.Close();
            Bind();
            button1.Enabled = true;
            btn_Update.Enabled = false;
            button3.Enabled = false;
            clearData();
           
        }


        private void Bind()
        {
            con = new SqlConnection(DatabaseStringClass.dbstring);
            //con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select ID,StartDateTime, EndDateTime, Patient, Doctor from appointments", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowno;
            rowno = e.RowIndex;
            rowNumber = Convert.ToInt32(dataGridView1.Rows[rowno].Cells[0].Value); 
            dateTimePicker1.Text = dataGridView1.Rows[rowno].Cells[1].Value.ToString();
            dateTimePicker2.Text = dataGridView1.Rows[rowno].Cells[2].Value.ToString();
            textBox1.Text = dataGridView1.Rows[rowno].Cells[3].Value.ToString();
            textBox2.Text = dataGridView1.Rows[rowno].Cells[4].Value.ToString();
            btn_Update.Enabled = true;
            button3.Enabled = true;
            button1.Enabled = false;
            button4.Show();
            //button4.Enabled = true;
            button2.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DatabaseStringClass.dbstring);
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from appointments where Id=@Id", con);
            cmd.Parameters.AddWithValue("@Id", rowNumber);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Record Deleted......");
            con.Close();
            Bind();
            button1.Enabled = true;
            btn_Update.Enabled = false;
            button3.Enabled = false;
            clearData();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearData();
            button2.Show();
            button1.Enabled = true;
            btn_Update.Enabled = false;
            button3.Enabled = false;
            button4.Hide();

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:MM";
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd HH:MM";
        }
    }
}

