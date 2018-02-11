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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;

namespace FlipFlopGame
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        public static SoundPlayer sp = new SoundPlayer(@"C:\Windows\Media\Ring08.wav");
      
        public Page1()
        {
            InitializeComponent();
            
            //Display the message 
            if (Page2.flag)
            {
                btn_play.Content = "Replay";
                if (Page2.message != "")
                {
                    btn_message.Visibility = Visibility.Visible;
                    btn_message.Content = Page2.message;
                }
            }
        }

        //display page 2 if user want to replay the game
        private void Btn_play_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
         
        }
    }
}
