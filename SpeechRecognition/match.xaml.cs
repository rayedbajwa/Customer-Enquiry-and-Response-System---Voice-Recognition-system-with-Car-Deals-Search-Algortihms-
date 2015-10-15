using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Synthesis;

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public partial class match : Window
    {
        private SqlCeDataAdapter da;
        BackgroundWorker bw;
        SpeechSynthesizer obj = new SpeechSynthesizer();
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
        public match()
        {
            InitializeComponent();
            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open(); SqlCeCommand cmd;
            cmd = new SqlCeCommand("delete from matches", con);
            cmd.ExecuteNonQuery();
            match_make();
        }
        public void match_make()
        { DataTable dtweights;
        DataTable dtcars;
        SqlCeCommand cmd;
            try
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open(); 
                //retrieve all enquiries
               SqlCeCommand com = new SqlCeCommand("select * from enquiry", con);
               dtcars = new DataTable();
               da = new SqlCeDataAdapter(com);
               da.Fill(dtcars);
               
               foreach (DataRow r in dtcars.Rows)
               {
                   Console.WriteLine(r["id"].ToString());
                   //retrieving all title matches
                   SqlCeCommand comm = new SqlCeCommand("select * from sellers where  LOWER(title) like LOWER('%" + r["title"].ToString() + "%')  ", con);
                   DataTable dtres = new DataTable();
                   da = new SqlCeDataAdapter(comm);
                   da.Fill(dtres);

                   //getting matches
                   SqlCeCommand comm_weights = new SqlCeCommand("select * from weights where enq_id=" + r["id"].ToString(), con);
                   dtweights = new DataTable();
                   da = new SqlCeDataAdapter(comm_weights);
                   da.Fill(dtweights);
                   String[] hey = new String[6];

                   
                   foreach (DataRow dr in dtweights.Rows)
                   {
                       hey[int.Parse(dr["colour"].ToString())] = "colour";
                       hey[int.Parse(dr["reg_city"].ToString())] = "reg_city";
                       hey[int.Parse(dr["budget"].ToString())] = "budget";
                       hey[int.Parse(dr["engine"].ToString())] = "engine";
                       hey[int.Parse(dr["trans"].ToString())] = "trans";

                       
                       
                      
                       //comparison if autoweights assigned
                       if (int.Parse(dr["colour"].ToString()) != int.Parse(dr["reg_city"].ToString()) || int.Parse(dr["budget"].ToString()) != int.Parse(dr["engine"].ToString()) || int.Parse(dr["trans"].ToString()) != int.Parse(dr["engine"].ToString()))
                       {
                           //ordering in a string array
                           

                           //adding to 1-5 ASSORTED array for better understanding
                           Dictionary<int, string> ord = new Dictionary<int, string>();
                           for (int i = 1; i < 6; i++)
                           {
                               ord.Add(i, hey[i]);

                           }
                          
                          
                           //logging and applying filters-priority wise
                           foreach (var pair in ord)
                           {


                               try
                               {

                                   if (pair.Value.Equals("budget"))
                                   {
                                   
                                   }
                                   else { 
                                   DataRow[] rows = dtres.Select(pair.Value + " like '%" + r[pair.Value].ToString() + "%'");
                                   for (int i = 0; i < rows.Length; i++)
                                   {
                                       Console.WriteLine("priority:" + pair.Key + "match by:" + pair.Value + "sell_id" + rows[i]["id"].ToString() + "enq_id:" + r["id"].ToString());


                                       if (con.State == System.Data.ConnectionState.Closed)
                                           con.Open();
                                       cmd = new SqlCeCommand("INSERT INTO matches (enq_id,sel_id,priority) VALUES (@enq,@sel,@pri)", con);

                                       cmd.Parameters.AddWithValue("@enq", r["id"].ToString());
                                       cmd.Parameters.AddWithValue("@sel", rows[i]["id"].ToString());
                                       cmd.Parameters.AddWithValue("@pri", pair.Key);
                                       cmd.ExecuteNonQuery();
                                   }
                                   }
                                   
                               }
                               catch (Exception ter)
                               {
                                   Console.WriteLine("not found" + ter.Message);
                               }
                               finally { con.Close(); }
                           }
                          

                       }
                       else {
                            int min = int.Parse(r["budget"].ToString
                           ()) - 300000;
                           int max = int.Parse(r["budget"].ToString
                           ()) + 300000;
                      Console.WriteLine( "engine like '%" + r["engine"].ToString() + "%' AND colour like '%" + r["colour"].ToString
                           () + "%' AND reg_city like '%" + r["reg_city"].ToString
                           () + "%'AND trans like '%" + r["trans"].ToString
                           () + "%'AND  budget  >=" + min + " AND budget <=" + max + "" );
                          
                          DataRow[] dtr= dtres.Select("engine like '%" + r["engine"].ToString
                           () +"%' AND colour like '%" + r["colour"].ToString
                           () +"%' AND reg_city like '%" + r["reg_city"].ToString
                           () +"%'AND trans like '%" + r["trans"].ToString
                           () +"%'AND  budget >=" + min+" AND budget<="+max);
                          Console.WriteLine("record:"+ dtr.Length);
                          for (int c = 0; c < dtr.Length;c++ )
                          {

                              Console.Write(dtr[c]["title"].ToString() + "-->");
                              Console.WriteLine(dtr[c]["id"].ToString());
                              try
                              {

                                  if (con.State == System.Data.ConnectionState.Closed)
                                      con.Open();


                                  cmd = new SqlCeCommand("INSERT INTO matches (enq_id,sel_id,priority) VALUES (@enq,@sel,@pri)", con);

                                  cmd.Parameters.AddWithValue("@enq", r["id"].ToString());
                                  cmd.Parameters.AddWithValue("@sel",dtr[c]["id"].ToString());
                                  cmd.Parameters.AddWithValue("@pri", 5);
                                  cmd.ExecuteNonQuery();

                              }
                              catch (Exception e)
                              {
                                  Console.WriteLine(e.Message);
                              }
                              finally { }

                          }
                       }
                   }

                   
                   }

                 
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

                con.Close();
            }

            
        }
        
    
    }
}
