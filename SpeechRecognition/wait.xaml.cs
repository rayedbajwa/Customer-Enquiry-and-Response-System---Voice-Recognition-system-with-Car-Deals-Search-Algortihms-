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
using System.Windows.Shapes;

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for wait.xaml
    /// </summary>
    public partial class wait : Window
    {
        public wait()
        {
           
            InitializeComponent();
            mp.Play();
         
            mp.MediaEnded += new RoutedEventHandler(previewPlayer_MediaEnded);
        }
        void previewPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            mp.Position = TimeSpan.Zero;
           mp.Play();
        }
        void changeTit(String hey)
        { 
           title.Content = hey + "Stored....";
        }


    }
}
