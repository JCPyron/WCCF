﻿using System;
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

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for MainMenuPG.xaml
    /// </summary>
    public partial class MainMenuPG : Page
    {
        public MainMenuPG()
        {
            InitializeComponent();
        }

        private void QuickPost_Click(object sender, RoutedEventArgs e)
        {
            QuickPost qp = new QuickPost();
            this.NavigationService.Navigate(qp);
        }

        private void FacebookBTN_Click(object sender, RoutedEventArgs e)
        {
            FacebookPG fp = new FacebookPG();
            this.NavigationService.Navigate(fp);
        }

        private void TwitterBTN_Click(object sender, RoutedEventArgs e)
        {
            TwitterPG tp = new TwitterPG();
            this.NavigationService.Navigate(tp);
        }

        private void EMailBTN_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.Gmail.com/");
        }

        private void GoogleCalenderBTN_Click(object sender, RoutedEventArgs e)
        {
            GoogleCalenderPG gc = new GoogleCalenderPG();
            this.NavigationService.Navigate(gc);
        }
    }
}