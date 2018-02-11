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
using System.Windows.Threading;
using System.Timers;
using System.Media;

namespace FlipFlopGame
{
    /* Refresh the UI 
     * Describes the priorities at which operations can be invoked 
     */
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate() { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    //Fisher algorithm to shuffle the array containing images' name
    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        //used to communicate with page 1
        public static bool flag = false;
        public static string message = "";

        SoundPlayer sp = new SoundPlayer(@"C:\Windows\Media\Ring08.wav");
        
        //contains two clicked buttons
        Button b_pre = null;
        Button b_next = null;
        int count = 0;  //maintain count of visible cards
        int steps = 30;

        //contain list of images & buttons
        List<Image> container = new List<Image>();  
        List<Button> buttons = new List<Button>();
        
        int[] arr = new int[12]{1,2,3,4,5,6,6,5,4,3,2,1}; //contain array of image name 

        
        public Page2()
        {
            InitializeComponent();
            Initialize_list();
            Randomize_list();
            btn_score.Content = steps.ToString();
            
            sp.PlayLooping();            
        }

        //initialize list with all images
        private void Initialize_list()
        {
            container.Add(A1);
            container.Add(A2);
            container.Add(A3);
            container.Add(A4);
            container.Add(A5);
            container.Add(A6);
            container.Add(A7);
            container.Add(A8);
            container.Add(A9);
            container.Add(A10);
            container.Add(A11);
            container.Add(A12);

            buttons.Add(B1);
            buttons.Add(B2);
            buttons.Add(B3);
            buttons.Add(B4);
            buttons.Add(B5);
            buttons.Add(B6);
            buttons.Add(B7);
            buttons.Add(B8);
            buttons.Add(B9);
            buttons.Add(B10);
            buttons.Add(B11);
            buttons.Add(B12);
        }
        
        //random images at different locations
        private void Randomize_list()
        {
            new Random().Shuffle(arr);
            int rand_num = 0;
            int i = 0;
            foreach (Image img in container)
            {
                rand_num = arr[i];
                img.Source = new BitmapImage(new Uri("pokemon/P" +rand_num+".png", UriKind.Relative));
                i++;
            }
        }

        //event whenever user click button
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            
            steps--;
            btn_score.Content = steps.ToString();   //display the count of steps

            if (count == 0)
            {
                b_pre = b;
                b_pre.Visibility = Visibility.Hidden;
            }
            else if (count == 1)
            {
                b_next = b;
                b_next.Visibility = Visibility.Hidden;
                b_next.Refresh();
                
                System.Threading.Thread.Sleep(500);
                
                if (Check_image())
                {
                    b_pre.IsEnabled = false;
                    b_next.IsEnabled = false;
                }
                else
                {
                    b_pre.Visibility = Visibility.Visible;
                    b_next.Visibility = Visibility.Visible;
                }

                if (Game_over())
                {
                    System.Threading.Thread.Sleep(500);
                    message = "Congratulation, you won";
                    Btn_reset_Click(sender, e);
                }

                else if (steps <= 0)
                {
                    btn_score.Refresh();
                    
                    b_pre.IsEnabled = false;
                    b_next.IsEnabled = false;

                    System.Threading.Thread.Sleep(1000);
                    message = "Sorry, out of moves";
                    Btn_reset_Click(sender, e);
                }
                count = -1;
            }
            count++;
        }

        //Check if currently visible images are same or not
        private bool Check_image()
        {
            string image1 = ImageDictionary(b_pre.Name);
            string image2 = ImageDictionary(b_next.Name);
            string src1 = "";
            string src2 = "";
            
            foreach (Image img in container)
            {
                if (img.Name == image1 || img.Name == image2)
                {
                    if (src1 == "")
                        src1 = img.Source.ToString();
                    else
                        src2 = img.Source.ToString();
                }
            }

            if (src1 == src2)
                return true;
            
            return false;
        }

        //Dictionary contain image in their corresponding button
        private string ImageDictionary(string btnName)
        {
            //Dictionary contain key values

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "B1", "A1" },
                { "B2", "A2" },
                { "B3", "A3" },
                { "B4", "A4" },
                { "B5", "A5" },
                { "B6", "A6" },
                { "B7", "A7" },
                { "B8", "A8" },
                { "B9", "A9" },
                { "B10", "A10" },
                { "B11", "A11" },
                { "B12", "A12" }
            };

            if (dictionary.ContainsKey(btnName))
            {
                string value = dictionary[btnName];
                return value;
            }

            return "";
        }

        //Check game is over or not
        private bool Game_over()
        {
            foreach (Button btn in buttons)
            {
                if (btn.IsEnabled == true)
                    return false;
            }

            return true;
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            sp.Stop();
            flag = true;
            this.NavigationService.Navigate(new Uri("Page1.xaml", UriKind.Relative));

        }
    
    }
}
