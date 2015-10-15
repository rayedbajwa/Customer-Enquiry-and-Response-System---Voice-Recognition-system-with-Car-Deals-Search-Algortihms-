using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> ColorsList;
        SpeechRecognizer speechRecognizer;
        public MainWindow()
        {
            InitializeComponent();
            ColorsList = new List<string>(); 
            speechRecognizer = new SpeechRecognizer();
            this.Loaded += MainWindow_Loaded;
        }
        SpeechSynthesizer ss = new SpeechSynthesizer();
        Choices opt = new Choices("yes","no");

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Type colorType = typeof(Colors);
            // We take only static property to avoid properties like Name, IsSystemColor ...
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo propInfo in propInfos)
            {
                ColorsList.Add(propInfo.Name);
            }
            Choices choices = new Choices(ColorsList.ToArray());
            GrammarBuilder grammarBuilder = new GrammarBuilder(choices);
            
            Grammar grammar = new Grammar(grammarBuilder);
            speechRecognizer.LoadGrammar(grammar);
            ss.SpeakAsync("Please Choose your desired Colour");
            speechRecognizer.Enabled = true;
            speechRecognizer.SpeechRecognized += speechRecognizer_SpeechRecognized;
        }

        void speechRecognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (ColorsList.Contains(e.Result.Text))
            {
                var color = (Color)ColorConverter.ConvertFromString(e.Result.Text);
                ColorCanvas.Background = new SolidColorBrush(color);
                ColorslistBox.Items.Insert(0, e.Result.Text);
                ss.SpeakAsync("You have chosen:" + e.Result.Text);

                ss.SpeakAsync("Please confirm the colour: ." + e.Result.Text+". say yes or no");
                GrammarBuilder yo = new GrammarBuilder(opt);
                Grammar g = new Grammar(yo);
                speechRecognizer.LoadGrammar(g);

                speechRecognizer.SpeechRecognized += speech_confirm;
                
            }


        }
        void speech_confirm(object sender, SpeechRecognizedEventArgs e)
        {

                
            if (e.Result.Text=="yes")
            {
                this.Hide();
                carmod yo = new carmod();
                yo.Show();
            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        { 
            System.Environment.Exit(0);
        }
    }
}
