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

using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Reflection;
using System.Speech.Recognition;
using System.Speech.Synthesis;

using System.Windows.Forms;
using System.Threading;
using System.Windows.Media;
using System.Data.SqlServerCe;
using System.Data;
namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for call.xaml
    /// </summary>
    public partial class call : Window
    {
        String title;
        String carmodel;
        String engine;
        String color;
        String trans;
        int budget;
        String city;
        public List<string> ColorsList;
        SpeechSynthesizer obj = new SpeechSynthesizer();
        SpeechRecognitionEngine recognizer;
        SqlCeConnection con;
        String sel_year;
        public call()
        {
            InitializeComponent();
            con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
            // constring="Data Source=C:\Users\FrastrayMBF\Downloads\Speech Recognition2 (2)\DDSpeech Recognition2\SpeechRecognition\bin\Debug\icers.sdf";
          main_menu();

            
        }
        public void main_menu()
        {

            recognizer = new SpeechRecognitionEngine();
            obj.SpeakAsync("Hello My name is ICERS, What can i do for you: 1 for checking old enquiries, 2 for entering new enquiry, and 3 for latest car analytics...");
            Thread.Sleep(10000);
            recognizer.RecognizeAsyncStop();
            GrammarBuilder gb = new GrammarBuilder();
            Choices menu = new Choices();
            menu.Add(new string[] { "one", "two", "three"});

            gb.Append(menu);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SetInputToDefaultAudioDevice();

            
            recognizer.RecognizeAsync(RecognizeMode.Single);
         
            recognizer.SpeechRecognized +=
             new EventHandler<SpeechRecognizedEventArgs>(mainRecog);
        }
       
        void mainRecog(object sender, SpeechRecognizedEventArgs e)
        {
            obj.SpeakAsync(e.Result.Text);
            if (e.Result.Text.Equals("one"))
            {
                check_enq();
            }
            else if (e.Result.Text.Equals("two"))
            {
               car_model();
                
            }
            else
            {
                analys h1 = new analys();
                h1.analys_Load(null,e);
                Thread.Sleep(13000);
                obj.SpeakAsync("Returning to main menu....");
                Thread.Sleep(2000);
            }
        
        }
        int enq_check;
        private void check_enq()
        {
            recognizer = new SpeechRecognitionEngine();
            obj.SpeakAsync("Speak Enquiry ID:");
            Thread.Sleep(1500);
            recognizer.RecognizeAsyncStop();
            GrammarBuilder gb = new GrammarBuilder();
            Choices menu = new Choices();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCeCommand cmd = new SqlCeCommand("select id from enquiry", con);


                SqlCeDataReader dr = cmd.ExecuteReader();

                    while(dr.Read())
                    {
                       
                        menu.Add(dr["id"].ToString());
                    }
                
                   
                
            }
            catch (Exception ers)
            {
                Console.Write(ers.Message);
            }
            gb.Append(menu);
            
            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SetInputToDefaultAudioDevice();
            Thread.Sleep(1500);

            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
           new EventHandler<SpeechRecognizedEventArgs>(enqRecog);
        }
        void enqRecog(object sender, SpeechRecognizedEventArgs e)
        {
            obj.SpeakAsync("Chosen:"+e.Result.Text);
            enq_check = int.Parse( e.Result.Text);
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Say yes or no");
            recognizer.RecognizeAsyncStop();
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);

            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(3000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(enqCheck);

        }

        private void enqCheck(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {
                try
                {
                    con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                    con.Open();
                    SqlCeCommand da = new SqlCeCommand("select title from enquiry where id=" + enq_check, con);
                    SqlCeDataReader dr= da.ExecuteReader();
                    while(dr.Read())
                    {
                    obj.SpeakAsync("Found, "+ dr["title"].ToString()+".....Wait While I check Matching results");
                    Thread.Sleep(4000);
                    }
                    match_enq();

                }
                catch (Exception ea)
                {
                    Console.WriteLine(ea.Message);
                    obj.SpeakAsync("Sorry, I can not find car with such details, Please refine or update the query");
                    Thread.Sleep(4500);
                    main_menu();



                }
                finally { con.Close(); }
            }
            else
            {
                obj.SpeakAsync("Retry");
                Thread.Sleep(500);
                check_enq();
            }
        }

        private void match_enq()
        {
            try
            {
                con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeDataAdapter da = new SqlCeDataAdapter("select distinct(sel_id) from matches where enq_id=" + enq_check, con);
               DataTable dt= new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count==0)
                {
                obj.SpeakAsync("I can not Finf any matching results...");
                    Thread.Sleep(1500);
               
                }
                else
                {
                    foreach (DataRow row in dt.Rows) 
                    { 
                        
                int sel_id = int.Parse(row["sel_id"].ToString());
                
                Console.WriteLine(row["sel_id"].ToString());
                SqlCeDataAdapter da1 = new SqlCeDataAdapter("select * from sellers where id="+sel_id, con);
                DataTable dt_sel = new DataTable();
                da1.Fill(dt_sel);
                recognizer = new SpeechRecognitionEngine();

                recognizer.RecognizeAsyncStop();
                GrammarBuilder gb = new GrammarBuilder();
                Choices id = new Choices();
                foreach (DataRow row2 in dt_sel.Rows)
                { obj.SpeakAsync("Unique ID "+row2["id"].ToString()+": "+row2["title"].ToString());
                    
                obj.SpeakAsync(",Contact Number... : " + row2["contact"].ToString());
                obj.SpeakAsync("I repeat: " + row2["contact"].ToString());
                id.Add(row2["id"].ToString());

                }
                gb.Append(id);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                recognizer.SetInputToDefaultAudioDevice();

                while (recognizer.AudioState.Equals( AudioState.Speech))
                {
                    Thread.Sleep(100);
                }

                     
                    
                
                    
                    }

                }

                
                    Thread.Sleep(500);

                    obj.SpeakAsync("Thank you for using my help, Please come back again.....");
                    Thread.Sleep(4000);
                    mainmenu hey = new mainmenu();
                    hey.Show();
                    this.Close();
              

            }
            catch (Exception ea)
            {
                Console.WriteLine(ea.Message);
                obj.SpeakAsync("I can not find any such results, Please try another enquiry...");
                Thread.Sleep(5000);
                check_enq();



            }
            finally { con.Close(); }
        }
        private void id_recog(object sender, SpeechRecognizedEventArgs e)
        {
            obj.SpeakAsync("Chosen: " + e.Result.Text);
        
        }
        public void car_model()
        {
            main.Visibility = System.Windows.Visibility.Hidden;
            brand.Visibility = System.Windows.Visibility.Visible;
            recognizer = new SpeechRecognitionEngine();
            obj.SpeakAsync("Please Choose the Car Brand....");
            recognizer.RecognizeAsyncStop();
            GrammarBuilder gb = new GrammarBuilder();
            Choices models = new Choices();
            models.Add(new string[] { "Toyota", "Suzuki", "Honda", "Kia", "BMW" });

            gb.Append(models);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SetInputToDefaultAudioDevice();

            Thread.Sleep(3000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
           
            recognizer.SpeechRecognized +=
             new EventHandler<SpeechRecognizedEventArgs>(srs_SpeechRecognized);
           
        }

       
       

        void srs_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            if (e.Result.Text == "Toyota")
            {

                obj.SpeakAsync("You have chosen Toyota");
                title = "Toyota";
                toyo_rb.IsChecked = true;

            }
            else if (e.Result.Text == "Honda")
            {

                obj.SpeakAsync("You have chosen Honda");
                title = "Honda";
                hon_rb.IsChecked = true;

            }
            else if (e.Result.Text == "Suzuki")
            {

                obj.SpeakAsync("You have chosen Pakistani SUZUKI");
                title = "Suzuki";
                suzu_rb.IsChecked = true;
            }

            else if (e.Result.Text == "Kia")
            {

                obj.SpeakAsync("You have chosen KIA");
               title = "Kia";
                kia_rb.IsChecked = true;
            }
            else if (e.Result.Text == "BMW")
            {
                title = "BMW";
                obj.SpeakAsync("You have chosen B M W");
                bmw_rb.IsChecked = true;
            }
            else
            {
                obj.SpeakAsync("Please choose from the list.");
            }
            obj.SpeakAsync("Say yes or no");
            recognizer.RecognizeAsyncStop();
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            recognizer.RecognizeAsyncStop();
            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(4000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(carname);

        }
        void carname(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Text.Equals("yes"))
            {
                recognizer.RecognizeAsyncStop();
                recognizer.RecognizeAsyncCancel();
                model.Visibility = System.Windows.Visibility.Visible;
                modelPic.Visibility = System.Windows.Visibility.Visible;
                Carname_Load();
            }
            else
            {

                obj.SpeakAsync("Please Retry");

                Thread.Sleep(1000);
                recognizer.RecognizeAsyncStop();
                car_model();
            }
        }
        public void Carname_Load()
        {


            recognizer = new SpeechRecognitionEngine();

            recognizer.SetInputToDefaultAudioDevice();

            modelPic.Visibility = System.Windows.Visibility.Visible;
            recognizer.RecognizeAsyncStop();
            model.Visibility = System.Windows.Visibility.Visible;
            if (title == "BMW")
            {


                obj.SpeakAsync("Choose from BMW cars");
                obj.SpeakAsync("3 Series");
                obj.SpeakAsync("5 Series");
                obj.SpeakAsync("7 Series");
                obj.SpeakAsync("C class");
                mod.Content = "BMW";
                model1.Content = "3 Series";
                model2.Content = "5 Series";
                model3.Content = "7 Series";
                model4.Content = "C Class";
                model1.Visibility = System.Windows.Visibility.Visible;
                model2.Visibility = System.Windows.Visibility.Visible;
                model3.Visibility = System.Windows.Visibility.Visible;
                model4.Visibility = System.Windows.Visibility.Visible;
                Choices models = new Choices();
                models.Add(new string[] { "3 series", "5 series", "7 series", "c class" });

                // Create a GrammarBuilder object and append the Choices object.
                GrammarBuilder gb = new GrammarBuilder();

                gb.Append(models);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                Thread.Sleep(10000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(icers_carnamerecognized);

            }
            else if (title == "Toyota")
            {
                // Image image = Image.FromFile("toyota.jpg");

                //pictureBox6.Image = image;
                obj.SpeakAsync("Choose from TOYOTA cars");
                obj.SpeakAsync("Corolla");
                obj.SpeakAsync("Camry");
                obj.SpeakAsync("Prado");
                obj.SpeakAsync("prius");
                obj.SpeakAsync("hilux");
                mod.Content = "Toyota";
                model1.Content = "Corolla";
                model2.Content = "Camry";
                model3.Content = "Prado";
                model4.Content = "Prius";
                model5.Content = "Hilux";
                model1.Visibility = System.Windows.Visibility.Visible;
                model2.Visibility = System.Windows.Visibility.Visible;
                model3.Visibility = System.Windows.Visibility.Visible;
                model4.Visibility = System.Windows.Visibility.Visible;
                model5.Visibility = System.Windows.Visibility.Visible;
                Choices toyomodels = new Choices();
                toyomodels.Add(new string[] { "Corolla", "Camry", "Prado", "Prius", "Hilux" });

                // Create a GrammarBuilder object and append the Choices object.
                GrammarBuilder gb = new GrammarBuilder();

                gb.Append(toyomodels);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                Thread.Sleep(10000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                // Register a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(icers_carnamerecognized);
            }
            else if (title == "Suzuki")
            {
                obj.SpeakAsync("Choose from suzuki cars");
                obj.SpeakAsync("mehran");
                obj.SpeakAsync("cultus");
                obj.SpeakAsync("bolan");
                obj.SpeakAsync("swift");
                obj.SpeakAsync("liana");
                mod.Content = "Suzuki";
                model1.Content = "Mehran";
                model2.Content = "Cultus";
                model3.Content = "Bolan";
                model4.Content = "Swift";
                model5.Content = "Liana";
                model1.Visibility = System.Windows.Visibility.Visible;
                model2.Visibility = System.Windows.Visibility.Visible;
                model3.Visibility = System.Windows.Visibility.Visible;
                model4.Visibility = System.Windows.Visibility.Visible;
                model5.Visibility = System.Windows.Visibility.Visible;
                Choices suzmodels = new Choices();
                suzmodels.Add(new string[] { "Mehran", "Cultus", "Bolan", "Swift", "Liana" });

                // Create a GrammarBuilder object and append the Choices object.
                GrammarBuilder gb = new GrammarBuilder();

                gb.Append(suzmodels);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                // recognizer.Enabled = true;

                Thread.Sleep(10000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                // Register a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(icers_carnamerecognized);

            }
            else if (title == "Kia")
            {
                obj.SpeakAsync("Choose from kia cars");
                obj.SpeakAsync("spectra");
                obj.SpeakAsync("sportage");
                mod.Content = "Kia";
                model1.Content = "Spectra";
                model2.Content = "Sportage";

                model1.Visibility = System.Windows.Visibility.Visible;
                model2.Visibility = System.Windows.Visibility.Visible;
                Choices kiamodels = new Choices();
                kiamodels.Add(new string[] { "Spectra", "Sportage" });

                // Create a GrammarBuilder object and append the Choices object.
                GrammarBuilder gb = new GrammarBuilder();

                gb.Append(kiamodels);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                Thread.Sleep(6000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                // Register a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(icers_carnamerecognized);

            }
            else if (title == "Honda")
            {
                obj.SpeakAsync("Choose from honda cars");
                obj.SpeakAsync("City");
                obj.SpeakAsync("civic");
                obj.SpeakAsync("accord");
                mod.Content = "Honda";
                model1.Content = "City";
                model2.Content = "Civic";
                model3.Content = "Accord";

                model1.Visibility = System.Windows.Visibility.Visible;
                model2.Visibility = System.Windows.Visibility.Visible;
                model3.Visibility = System.Windows.Visibility.Visible;


                Choices hondamodels = new Choices();
                hondamodels.Add(new string[] { "City", "Civic", "Accord" });

                // Create a GrammarBuilder object and append the Choices object.
                GrammarBuilder gb = new GrammarBuilder();

                gb.Append(hondamodels);

                // Create the Grammar instance and load it into the speech recognition engine.
                Grammar g = new Grammar(gb);
                recognizer.LoadGrammar(g);
                //  recognizer.Enabled = true;
                Thread.Sleep(8000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                // Register a handler for the SpeechRecognized event.
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(icers_carnamerecognized);

            }


        }
        void icers_carnamerecognized(object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("You have chosen: " + e.Result.Text);
            obj.SpeakAsync("Say yes or no");
            recognizer.RecognizeAsyncStop();
            carmodel = e.Result.Text;

            if (model1.Content.ToString().Equals(e.Result.Text.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model1.IsChecked = true;
            }
            if (model2.Content.ToString().Equals(e.Result.Text.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model2.IsChecked = true;
            }
            if (model3.Content.ToString().Equals(e.Result.Text.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model3.IsChecked = true;
            }
            if (model4.Content.ToString().Equals(e.Result.Text.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model4.IsChecked = true;
            }
            if (model5.Content.ToString().Equals(e.Result.Text.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                model5.IsChecked = true;
            }
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);

            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(4000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(car_check);
        }
        void car_check(object sender, SpeechRecognizedEventArgs e)
        {

            recognizer.RecognizeAsyncStop();
            recognizer.RecognizeAsyncCancel();
            if (e.Result.Text.Equals("yes"))
            {

                title = String.Concat(title+" "+ carmodel);
                colourform_Load();
            }
            else
            {
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(1000);
                recognizer.RecognizeAsyncStop();
                Carname_Load();
            }


        }

      
        
        private void colourform_Load()
        {


            recognizer = new SpeechRecognitionEngine();

            recognizer.SetInputToDefaultAudioDevice();

            colour.Visibility = System.Windows.Visibility.Visible;
            colourPic.Visibility = System.Windows.Visibility.Visible;


           
            Choices choices = new Choices("Black","Yellow","Gray","Red","Maroon","Green","Silver","White","Pink","Blue","Brown","Purple");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);

            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.LoadGrammar(grammar);
            obj.SpeakAsync("Say your Desired Colour");
            Thread.Sleep(3000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
                new EventHandler<SpeechRecognizedEventArgs>(color_rec);
        }

        void color_rec(object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            Console.WriteLine(e.Result.Text);
            obj.SpeakAsync("You have chosen: " + e.Result.Text);
            
                var mcolor = (Color)ColorConverter.ConvertFromString(e.Result.Text);
                SolidColorBrush myBrush = new SolidColorBrush(mcolor);
                colorRect.Fill = myBrush;
                Colour.Foreground = new SolidColorBrush(Colors.White);

               
                obj.SpeakAsync("Say yes or no");

                Choices choices = new Choices("yes", "no");
                GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
                color = e.Result.Text;
                Grammar grammar = new Grammar(grammarBuilder);
                recognizer.UnloadAllGrammars();
                recognizer.LoadGrammar(grammar);
                Thread.Sleep(4000);
                recognizer.RecognizeAsync(RecognizeMode.Single);
                recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_Speechconfirm);

            


        }


        void sre_Speechconfirm(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync(color + " stored.");
                engine_select();
            }
            else
            {
                color = "";
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(1000);
                recognizer.RecognizeAsyncStop();
                colourform_Load();
            }
        }

        private void engine_select()
        {
            engGrid.Visibility = System.Windows.Visibility.Visible;
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Select Engine Type");
            obj.SpeakAsync("CNG");
            obj.SpeakAsync("Petrol");
            obj.SpeakAsync("Diesel");
            Choices choices1 = new Choices("CNG", "Petrol", "Diesel");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices1);

            Grammar grammarss = new Grammar(grammarBuilder);
            recognizer.LoadGrammar(grammarss);

            // Create the Grammar instance and load it into the speech recognition engine.
            Thread.Sleep(8000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recognizeEngine);
        }
        void recognizeEngine(Object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("You have chosen: " + e.Result.Text);
            obj.SpeakAsync("Say yes or no");
            engine = e.Result.Text;
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            if (cng_rb.Content.ToString().Equals(engine.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                cng_rb.IsChecked = true;
            }
            if (petrol_rb.Content.ToString().Equals(engine.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                petrol_rb.IsChecked = true;
            }
            if (diesel_rb.Content.ToString().Equals(engine.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                diesel_rb.IsChecked = true;
            }

            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(4000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(engine_confirm);

        }
        void engine_confirm(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync(engine + " stored.");
                transmission_select();
            }
            else
            {
                
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(2000);
                recognizer.RecognizeAsyncStop();
                engine_select();
            }


        }
        public void transmission_select()
        {
            transGrid.Visibility = System.Windows.Visibility.Visible;
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Select Transmission Type");
            obj.SpeakAsync("Automatic");
            obj.SpeakAsync("Manual");

            Choices choices1 = new Choices("Automatic", "Manual");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices1);

            Grammar grammarss = new Grammar(grammarBuilder);
            recognizer.LoadGrammar(grammarss);

            // Create the Grammar instance and load it into the speech recognition engine.
            Thread.Sleep(7000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recognizeTrans);


        }
        void recognizeTrans(Object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("You have chosen: " + e.Result.Text);
            obj.SpeakAsync("Say yes or no");
           
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            trans = e.Result.Text;
            if (auto_rb.Content.ToString().Equals(trans.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                auto_rb.IsChecked = true;
            }
            if (manual_rb.Content.ToString().Equals(trans.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                manual_rb.IsChecked = true;
            }

            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(4000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(transConf);

        }
        void transConf(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync(trans + " stored.");
                budget_select();
            }
            else
            {
                trans = "";
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(2000);
                recognizer.RecognizeAsyncStop();
                transmission_select();
            }


        }
        void budget_select()
        {

            budGrid.Visibility = System.Windows.Visibility.Visible;
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

            Choices opt = new Choices("one", "two", "three", "four", "five");
            obj.SpeakAsync("Select Your Budget ");
            obj.SpeakAsync("Say 1 for under 5 lakh");
            obj.SpeakAsync("Say 2 for under 10 lakh");
            obj.SpeakAsync("Say 3 for under 15 lakh");
            obj.SpeakAsync("Say 4 for under 20 lakh");
            obj.SpeakAsync("Say 5 for 25 lakh and above");

            GrammarBuilder yo = new GrammarBuilder(opt);
            Grammar g = new Grammar(yo);
            recognizer.LoadGrammar(g);

            Thread.Sleep(18 * 1000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recogBudget);
        }
        void recogBudget(Object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

          
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);

            if (e.Result.Text == "one")
            {

                obj.SpeakAsync("You have chosen 5 lakh range");
                budget = 500000;

            }
            else if (e.Result.Text == "two")
            {

                obj.SpeakAsync("You have chosen  10 lakh range");
                budget = 1000000;

            }
            else if (e.Result.Text == "three")
            {

                obj.SpeakAsync("You have chosen  15 lakh range");
                budget = 150000;

            }
            else if (e.Result.Text == "four")
            {

                obj.SpeakAsync("You have chosen 20 lakh range");
                budget = 2000000;
            }
            else if (e.Result.Text == "five")
            {

                obj.SpeakAsync("You have chosen 25 lakh and above range ");
                budget = 2500000;
            }

            if (bud1.Content.ToString().Equals(budget.ToString()))
            {
                bud1.IsChecked = true;
            }
            else if (bud2.Content.ToString().Equals(budget.ToString()))
            {
                bud2.IsChecked = true;
            }
            else if (bud3.Content.ToString().Equals(budget.ToString()))
            {
                bud3.IsChecked = true;
            }
            else if (bud4.Content.ToString().Equals(budget.ToString()))
            {
                bud4.IsChecked = true;
            }
            else if (bud5.Content.ToString().Equals(budget.ToString()))
            {
                bud5.IsChecked = true;
            }
            else
            {
                Console.Write("nai mila");
            }
            obj.SpeakAsync("Say yes or no");
            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(5000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(budConf);

        }
        void budConf(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync("Budget Stored.");
                regCity();
            }
            else
            {
                
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(2000);
                recognizer.RecognizeAsyncStop();
                budget_select();
            }


        }
        void regCity()
        {
            main.Visibility = System.Windows.Visibility.Hidden;
            engGrid.Visibility = System.Windows.Visibility.Hidden;
            transGrid.Visibility = System.Windows.Visibility.Hidden;
            budGrid.Visibility = System.Windows.Visibility.Hidden;
            brand.Visibility = System.Windows.Visibility.Hidden;
            model.Visibility = System.Windows.Visibility.Hidden;
            colour.Visibility = System.Windows.Visibility.Hidden;
            reg.Visibility = System.Windows.Visibility.Visible;
            modelPic.Visibility = System.Windows.Visibility.Hidden;
            colourPic.Visibility = System.Windows.Visibility.Hidden;
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Select Your City ");
            obj.SpeakAsync("Rawalpindi,Islamabad,Lahore,Karachi,Peshawar");
            Choices opt = new Choices("Rawalpindi", "Islamabad", "Lahore", "Peshawar", "Karachi");
            GrammarBuilder yo = new GrammarBuilder(opt);
            Grammar g = new Grammar(yo);
            recognizer.LoadGrammar(g);
            Thread.Sleep(9* 1000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(recogCity);
        }
        void recogCity(Object sender, SpeechRecognizedEventArgs e)
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("You have Chosen: " + e.Result.Text);
            city = e.Result.Text;

            if (c1.Content.ToString().Equals(city.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                c1.IsChecked = true;
            }
            else if (c2.Content.ToString().Equals(city.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                c2.IsChecked = true;
            }
            else if (c3.Content.ToString().Equals(city.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                c3.IsChecked = true;
            }
            else if (c4.Content.ToString().Equals(city.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                c4.IsChecked = true;
            }
            else if (c5.Content.ToString().Equals(city.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                c5.IsChecked = true;
            }
            else
            {
                Console.Write("nai mila");
            }
            Choices choices = new Choices("yes", "no");
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            obj.SpeakAsync("Say yes or no");
            Grammar grammar = new Grammar(grammarBuilder);
            recognizer.UnloadAllGrammars();
            recognizer.LoadGrammar(grammar);
            Thread.Sleep(4000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(cityConf);
        }
        void cityConf(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync(city + " stored.");
                year();
            }
            else
            {
                engine = "";
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(2000);
                recognizer.RecognizeAsyncStop();
                regCity();
            }


        }
        int id;
        void db_store()
        {
            
            try
            {
                con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeCommand com = new SqlCeCommand("INSERT INTO enquiry (title,colour,engine,trans,budget,reg_city,year,process) VALUES ('" +title+ "','" + color + "','" + engine + "','" + trans + "','" + budget + "','" + city + "','"+sel_year+"','0')", con);
                com.ExecuteNonQuery();
                SqlCeDataReader dr;
                    com = new SqlCeCommand("select MAX(id) from enquiry",con);
                    dr = com.ExecuteReader(); 
                    while (dr.Read())
                    { 
                    id=dr.GetInt32(0);
                        Console.WriteLine(dr.GetInt32(0));
                    }
               
                weight_menu();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            finally { con.Close();  }


        
        }
        void year()
        {
            reg.Visibility = System.Windows.Visibility.Hidden;
            yearGrid.Visibility = System.Windows.Visibility.Visible;
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Select year ");
            Choices opt = new Choices("2006", "2007", "2008", "2009", "2010","2011","2012","2013","2014");
            GrammarBuilder yo = new GrammarBuilder(opt);
            Grammar g = new Grammar(yo);
            recognizer.LoadGrammar(g);
            Thread.Sleep(3000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized +=
             new EventHandler<SpeechRecognizedEventArgs>(recogYear);
        }
        
        void recogYear(Object sender, SpeechRecognizedEventArgs e)
        {

            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("You have Chosen: " + e.Result.Text);
           sel_year = e.Result.Text;
           Choices choices = new Choices("yes", "no");
           GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
           obj.SpeakAsync("Say yes or no");
           Grammar grammar = new Grammar(grammarBuilder);
           recognizer.UnloadAllGrammars();
           recognizer.LoadGrammar(grammar);
           Thread.Sleep(4500);
           recognizer.RecognizeAsync(RecognizeMode.Single);
           recognizer.SpeechRecognized +=
          new EventHandler<SpeechRecognizedEventArgs>(yearConf);
        }
        void yearConf(Object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text.Equals("yes"))
            {

                obj.SpeakAsync(sel_year + " stored.");
                db_store();
                
            }
            else
            {
                sel_year = "";
                obj.SpeakAsync("Please Retry");
                Thread.Sleep(2000);
                recognizer.RecognizeAsyncStop();
                year();
            }


        }
        void weight_menu()
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Do you want me to assign priority of searching manually?");
            Choices opt = new Choices("yes","no");
            GrammarBuilder gb = new GrammarBuilder(opt);
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            Thread.Sleep(5000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(autoWeight);
        
        
        
        }int[] weight = new int[6];
        void autoWeight(Object sender, SpeechRecognizedEventArgs e)
        {
                if(e.Result.Text.Equals("no"))
                {
                    for (int i = 0; i < weight.Length;i++ )
                    {
                        weight[i] = 5;

                        
                    }
                    obj.SpeakAsync("Auto Weights assigned for enquiry ID:"+id+".");
                    db_weights();
                }
                else
                {
                assign_weight();
                }
        
        
        }
        int assign=0;
        

        void assign_weight()
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();
            obj.SpeakAsync("Choose Priority for searching...Please speak values from 1-5, priority wise and do not repeat");
            obj.SpeakAsync("Colour: ");
          
            Choices opt= new Choices("1","2","3","4","5");
            GrammarBuilder gb= new GrammarBuilder(opt);
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            Thread.Sleep(7000);
            recognizer.RecognizeAsync(RecognizeMode.Single);
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Weight);
        
        
        }
       
      
        void Weight(Object sender, SpeechRecognizedEventArgs e)
        {
            Boolean flag=false;
           recognizer = new SpeechRecognitionEngine();
           recognizer.SetInputToDefaultAudioDevice();
           Choices opt = new Choices("1", "2", "3", "4", "5");
           GrammarBuilder gb = new GrammarBuilder(opt);
           Grammar g = new Grammar(gb);
           recognizer.LoadGrammar(g);
          
           foreach (int w in weight)
           {
               if (w == int.Parse(e.Result.Text))
               {
                   flag = true;
                   break;
               }
               else
               {
                   flag = false;
               }
           }
           if (flag)
           {
              
               Console.WriteLine(assign);
               Console.WriteLine(weight[assign]);
               obj.SpeakAsync("Already Chosen, Please retry");
               Thread.Sleep(2000);
               recognizer.RecognizeAsync(RecognizeMode.Single);
               recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Weight);

           }
           else
           {
               obj.SpeakAsync("Chosen: " + e.Result.Text);
               Thread.Sleep(2000);
               assign++;

               weight[assign] = int.Parse(e.Result.Text);



               switch (assign)
               {
                   case 1:
                       {



                           obj.SpeakAsync("Budget: ");


                           break;
                       }
                   case 2:
                       {

                           obj.SpeakAsync("Transmission: ");


                           break;
                       }
                   case 3:
                       {

                           obj.SpeakAsync("Engine: ");

                           

                           break;
                       }
                   case 4:
                       {

                           obj.SpeakAsync("Registation city: ");
                           Thread.Sleep(500);

                           break;
                       }

                   default:
                       {
                           db_weights();
                           break;
                       }


               }


               Thread.Sleep(1000);
               recognizer.RecognizeAsync(RecognizeMode.Single);
               recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Weight);
           }
        }
      
        void db_weights()
        {
            try
            {
                con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
                con.Open();
                SqlCeCommand com = new SqlCeCommand("INSERT INTO weights (colour,budget,trans,engine,reg_city,enq_id) VALUES ('" + weight[1] + "','" + weight[2] + "','" + weight[3] + "','" + weight[4] + "','" + weight[5] + "','" + id + "')", con);
                com.ExecuteNonQuery();
              
              
                obj.SpeakAsync("Search in Progress, Your Enquiry ID is, " + id+" , Thank you for your cooperation !!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            finally { con.Close(); }
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            obj.SpeakAsyncCancelAll();
            recognizer.RecognizeAsyncStop();
            System.Environment.Exit(0);
        }
       
    }
}    
 
    
      


    

