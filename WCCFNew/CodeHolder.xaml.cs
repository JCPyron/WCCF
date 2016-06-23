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

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for CodeHolder.xaml
    /// </summary>
    public partial class CodeHolder : Window
    {
        public CodeHolder()
        {
            InitializeComponent();
        }

    //    private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
    //    {
    //        EventText.MaxLength = 0;

    //        if (EventText.Text.Length >= MAX_CHARS)
    //        {
    //            RemainingChar.Foreground = Brushes.Red;
    //        }
    //        else
    //        {
    //            RemainingChar.Foreground = Brushes.LimeGreen;
    //        }

    //        RemainingChar.Content = "Characters remaining: Unlimited";
    //    }

    //    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    //    {

    //        EventText.MaxLength = MAX_CHARS;


    //        if (EventText.Text.Length >= MAX_CHARS)
    //        {
    //            RemainingChar.Foreground = Brushes.Red;
    //        }
    //        else
    //        {
    //            RemainingChar.Foreground = Brushes.LimeGreen;
    //        }

    //        RemainingChar.Content = "Characters remaining: " + Convert.ToString(EventText.MaxLength - EventText.Text.Length);
    //    }

    //    //private void EventText_TextChanged(object sender, TextChangedEventArgs e)
    //    //{
    //    //    if (twittercheck.IsChecked.Value)
    //    //    {
    //    //        RemainingChar.Content = "Characters remaining: " + Convert.ToString(EventText.MaxLength - EventText.Text.Length);
    //    //    }
    //    //    else
    //    //    {
    //    //        RemainingChar.Content = "Characters remaining: Unlimited";
    //    //    }

    //    //    if (EventText.Text.Length >= MAX_CHARS && twittercheck.IsChecked.Value == true)
    //    //    {
    //    //        RemainingChar.Foreground = Brushes.Red;
    //    //    }
    //    //    else
    //    //    {
    //    //        RemainingChar.Foreground = Brushes.LimeGreen;
    //    //    }

    //    //}

    //    private void SendMSGBTN_Click(object sender, RoutedEventArgs e)
    //    {
    //        string eventString = EventText.Text;
    //        EventText.Clear();
    //    }

    //    private void ClearMSGBTN_Click(object sender, RoutedEventArgs e)
    //    {
    //        EventText.Clear();
    //    }

    //    private void MainMenuBTN_Click(object sender, RoutedEventArgs e)
    //    {
    //        MainMenuPG mm = new MainMenuPG();
    //        this.NavigationService.Navigate(mm);
    //    }
    //}
}
}
