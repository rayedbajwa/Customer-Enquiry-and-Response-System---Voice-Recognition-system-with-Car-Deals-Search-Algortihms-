using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class mainmenu : Window
    {BackgroundWorker bw;
        public mainmenu()
        {
            InitializeComponent();
            bw = new BackgroundWorker();
            InitializeBackgroundWorker();
            
        }
       
        private void InitializeBackgroundWorker()
        {
           bw.DoWork +=
                new DoWorkEventHandler(bw_DoWork);
            bw.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            bw_RunWorkerCompleted);
            
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void car_click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            car_data yo = new car_data();
            yo.Show();


        }
        wait waitwind = new wait();
        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            sellers open = new sellers();
            open.Show();

        }
        ProgressBar ap;
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            bw.RunWorkerAsync();
            
            

                
          
            
        }
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {



                    waitwind.Show();
                    this.IsEnabled = false;
                });

                

                web_parse obj = new web_parse();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }


            
           
        }
        
           private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

          this.IsEnabled = true;
          waitwind.Hide();
            App.Current.Dispatcher.Invoke((Action)delegate
            {



                this.IsEnabled = true;
                waitwind.Hide();
            });
           
             
           
        }

           private void Button_Click_4(object sender, RoutedEventArgs e)
           {
               match mat=new match();

               mat.Show();
              
           }

           private void Button_Click_5(object sender, RoutedEventArgs e)
           {
               call test_call = new call();
               test_call.Show();
           }

           private void Button_Click_6(object sender, RoutedEventArgs e)
           {
               analys anal = new analys();
               anal.Show();
           }
         
    }
}
