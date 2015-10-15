using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Speech.Synthesis;
using System.Threading;


namespace SpeechRecognition
{
    public partial class analys : Form
    {
       
        private SqlCeDataAdapter da;
        String constring;
        public analys()
        {
            InitializeComponent();
          
        }
       SpeechSynthesizer obj = new SpeechSynthesizer();

       SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
       // constring="Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf";
        

        private void func_favcar()
        {
            
            try
            {

                con.Open();
                SqlCeDataReader reader = null;
                SqlCeCommand sqlcommand = new SqlCeCommand("select top 5 title, count(*) as num from enquiry group by title order by num desc", con);
                // select name from mytable group by name having count(*) = (select max(count(*)) from mytable group by name);
                // SqlCeCommand sqlcommand = new SqlCeCommand("SELECT title FROM enquiry GROUP BY title having count(*) = (  select count(*) FROM enquiry  GROUP BY title  ORDER BY count(*) desc  limit 1)", con);
                //SqlCeCommand sqlcommand = new SqlCeCommand("SELECT title, COUNT(*) FROM enquiry GROUP BY title", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //dataGridView1.DataSource = dt;
                String favtitlecar;

                int a = 0;
                while (reader.Read())
                {
                    // label12.Text = (reader.ToString());
                    if(a==0)
                    label12.Text = (reader["title"].ToString());

                    listBox1.Items.Add(reader["title"].ToString());
                    a++;
                    // favtitlecar = Convert.ToString(reader["title"]);
                    //MessageBox.Show(favtitlecar);
                }
                obj.SpeakAsync("Hot Favourite Car is "+label12.Text);
                con.Close();
                // = Convert.ToString(favtitlecar);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }


        private void func_cheapcar()
        {

            try
            {
               // SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataReader reader = null;
                SqlCeCommand sqlcommand = new SqlCeCommand("select title,MIN(budget) as count from sellers group by title order by count desc", con);
               // select name from mytable group by name having count(*) = (select max(count(*)) from mytable group by name);
               // SqlCeCommand sqlcommand = new SqlCeCommand("SELECT title FROM enquiry GROUP BY title having count(*) = (  select count(*) FROM enquiry  GROUP BY title  ORDER BY count(*) desc  limit 1)", con);
                //SqlCeCommand sqlcommand = new SqlCeCommand("SELECT title, COUNT(*) FROM enquiry GROUP BY title", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //dataGridView1.DataSource = dt;
                String favtitlecar = sqlcommand.ExecuteScalar().ToString();

                while (reader.Read())
                {

                    if (!reader["count"].ToString().Equals(""))
                    {
                   //     Console.WriteLine(reader["count"].ToString());
                        label15.Text = (reader["title"].ToString());
                    }
                   
                }
                
                   // label12.Text = (reader.ToString());
                  //  Console.WriteLine(reader["count"].ToString());
                    label15.Text = (reader["title"].ToString());
                  
                   // favtitlecar = Convert.ToString(reader["title"]);
                  
                
                con.Close();
                //obj.SpeakAsync("Hot Favourite Car is "label1.Text);
                // = Convert.ToString(favtitlecar);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }

        private void func_favcolor()
        {

            try
            {
                //SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataReader reader = null;
                SqlCeCommand sqlcommand = new SqlCeCommand(" select top 1 colour, count(*) as num from enquiry group by colour order by num desc", con);
               
                //SqlCeCommand sqlcommand = new SqlCeCommand("SELECT top 1 colour,COUNT(*) FROM enquiry  GROUP BY colour ", con);
                //SqlCeCommand sqlcommand = new SqlCeCommand("SELECT colour, COUNT(*) FROM enquiry GROUP BY colour", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //dataGridView1.DataSource = dt;
                String favtitlecar;


                while (reader.Read())
                {

                    
                   // MessageBox.Show(reader["colour"].ToString());
                    label11.Text = (reader["colour"].ToString());
                    //favtitlecar = Convert.ToString(reader["title"]);
                    //MessageBox.Show(favtitlecar);
                }
                con.Close();
                // = Convert.ToString(favtitlecar);
                obj.SpeakAsync("Hot Favourite Car Colour is "+ label11.Text);
            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }


        private void func_favtrans()
        {

            try
            {
               // SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataReader reader = null;
                SqlCeCommand sqlcommand = new SqlCeCommand("select top 1 trans, count(*) as num from enquiry group by trans order by num desc", con);
               
                //SqlCeCommand sqlcommand = new SqlCeCommand("SELECT trans, COUNT(*) FROM enquiry GROUP BY trans ", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //MessageBox.Show("dql"+reader);
               // label8.Text = (reader["trans"].ToString());
                //dataGridView1.DataSource = dt;
                String favtitlecar;


                while (reader.Read())
                {
                    label8.Text = (reader["trans"].ToString());
                    //favtitlecar = Convert.ToString(reader["title"]);
                    //MessageBox.Show(favtitlecar);
                }
                con.Close();
                // = Convert.ToString(favtitlecar);
                obj.SpeakAsync("Best chosen transmission: "+reader["trans"].ToString());

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }


        private void func_favyear()
        {

            try
            {
               // SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataReader reader = null;

                SqlCeCommand sqlcommand = new SqlCeCommand("select top 1 year, count(*) as num from enquiry group by year order by num desc", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //dataGridView1.DataSource = dt;
                String favtitlecar;


                while (reader.Read())
                {
                    label9.Text = (reader["year"].ToString());
                    //favtitlecar = Convert.ToString(reader["title"]);
                    //MessageBox.Show(favtitlecar);
                    obj.SpeakAsync("Most Demanding registration year: "+reader["year"].ToString());
                }
                con.Close();
                // = Convert.ToString(favtitlecar);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }

        private void func_favengine()
        {

            try
            {
               // SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataReader reader = null;

                SqlCeCommand sqlcommand = new SqlCeCommand("SELECT top 1 engine, COUNT(*) FROM enquiry GROUP BY engine order by engine desc", con);
                sqlcommand.CommandType = CommandType.Text;
                //SqlCeCommandBuilder cmd = new SqlCeCommandBuilder(da);
                reader = sqlcommand.ExecuteReader();
                //dataGridView1.DataSource = dt;
                String favtitlecar;


                while (reader.Read())
                {
                   // MessageBox.Show(reader["engine"].ToString());
                    label7.Text = (reader["engine"].ToString());
                    //favtitlecar = Convert.ToString(reader["title"]);
                   // MessageBox.Show(favtitlecar);
                    obj.SpeakAsync("Most Demanding Engine Nowadays :  " + reader["engine"].ToString());
                }
                con.Close();
                // = Convert.ToString(favtitlecar);

            }
            catch (Exception er)
            {
                Console.WriteLine(er.Message);
            }
        }


        public void analys_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'icersDataSet.sellers' table. You can move, or remove it, as needed.
            this.sellersTableAdapter.Fill(this.icersDataSet.sellers);
            // TODO: This line of code loads data into the 'icersDataSet.enquiry' table. You can move, or remove it, as needed.
            this.enquiryTableAdapter.Fill(this.icersDataSet.enquiry);
            // TODO: This line of code loads data into the 'icersDataSet1.sellers' table. You can move, or remove it, as needed.
         //  this.sellersTableAdapter.Fill(this.icersDataSet1.sellers);
            // TODO: This line of code loads data into the 'icersDataSet1.enquiry' table. You can move, or remove it, as needed.
          // this.enquiryTableAdapter.Fill(this.icersDataSet1.enquiry);
            // TODO: This line of code loads data into the 'icersDataSet.cars' table. You can move, or remove it, as needed.
            //this.carsTableAdapter.Fill(this.icersDataSet.cars);

            func_favcar();
            func_favcolor();
            func_favtrans();
            func_favyear();
            func_favengine();
            func_cheapcar();
            //int flag = 0;
            //while (flag == 0)
            //{

            //    obj.SpeakAsync("My name is ICERS, Created by students of SE 19. Today is the day I came in existance. Born to solve the problems of People, Buy cars with me in ease by just sitting at home... Thank you for listening");
            //    Thread.Sleep(20000);
            //}
            

        
            // TODO: This line of code loads data into the 'icersDataSet.cars' table. You can move, or remove it, as needed.

           
        }

        

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           // obj.SpeakAsync(ra["title"].ToString());
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void enquiryBindingSource2_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
