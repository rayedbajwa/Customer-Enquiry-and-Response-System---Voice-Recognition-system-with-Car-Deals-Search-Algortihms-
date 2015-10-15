using System;
using System.Collections.Generic;
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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Shapes;

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class carmod : Window
    {
        public carmod()
        {
            InitializeComponent();
            Window1_Load();
        }
        SpeechSynthesizer obj = new SpeechSynthesizer();
        SpeechRecognizer recognizer = new SpeechRecognizer();
        private void Window1_Load()
        {

           

       
            obj.SpeakAsync("hello, Please Choose the Model of car....");
            // Create a simple grammar that recognizes "red", "green", or "blue".
            Choices models = new Choices();
            models.Add(new string[] { "toyota", "suzuki", "honda", "kia","bmw"});

            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();


            gb.Append(models);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
           recognizer.Enabled= true; 
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        
        }

         void sre_speechRej(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            obj.SpeakAsync("Please try again");
        }
        // Create a simple handler for the SpeechRecognized event.
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {  


            if (e.Result.Text == "toyota")
            {
                obj.SpeakAsync("You have chosen Toyota");

            }
            else if (e.Result.Text == "honda")
            {
                obj.SpeakAsync("You have chosen Honda");

            }
            else if (e.Result.Text == "suzuki")
            {
                obj.SpeakAsync("You have chosen Pakistani SUZUKI");

            }
            
            else if(e.Result.Text=="kia")
            {
                obj.SpeakAsync("KIA");
            }
            else if (e.Result.Text == "bmw")
            {
                obj.SpeakAsync("B M W");
            }
            else
            {
                obj.SpeakAsync("Please choose from the list.");
            }
            this.Hide();
            MainWindow yo = new MainWindow();
           yo.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }//yes or no validatoin 
    }

