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
    public partial class sellers : Form
    {
        private SqlCeDataAdapter da;

        private DataTable dt;
        
        public sellers()
        {
            InitializeComponent();
        }
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
        private void sellers_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                da = new SqlCeDataAdapter("select * from sellers", con);
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

        private void back_Click(object sender, EventArgs e)
        {
            this.Hide();
            mainmenu hey = new mainmenu();
            hey.Show();
        }
    }
}
