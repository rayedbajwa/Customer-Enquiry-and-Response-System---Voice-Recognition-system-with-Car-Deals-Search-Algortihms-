using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechRecognition
{
    public partial class car_data : Form
    {
        private SqlCeDataAdapter da;
        private DataTable dt;
       
        public car_data()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");

        private void car_data_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'icersDataSet.cars' table. You can move, or remove it, as needed.
            try
            {
                con.Open();
                da = new SqlCeDataAdapter("select * from enquiry",con);
                SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
               
                
                con.Close();

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            try {
                da.Update(dt);
                MessageBox.Show("Updated");
            
            }

            catch (Exception er)
            {

                MessageBox.Show(""+er);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainmenu hey = new mainmenu();
            hey.Show();
        }
    }
}
