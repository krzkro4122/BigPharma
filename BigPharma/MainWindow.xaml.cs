using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BigPharma
{
    public partial class MainWindow : Window
    {
        StockManager stock;
        SaleManager sale;
        Summaries summaries;
        AuthWindow authWindow;

        public MainWindow()
        {
            InitializeComponent();
            authWindow = new();
            stock = new();
            sale = new();
            summaries = new();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            foreach(Window window in Application.Current.Windows) window.Close();
        }

        public void Unlock_Resources()
        {
            MenuAuthButton.Opacity = 0.5;
            MenuAuthButton.IsHitTestVisible = false;

            MenuStockButton.IsHitTestVisible = true;
            MenuStockButton.Opacity = 1;

            MenuSaleButton.IsHitTestVisible = true;
            MenuSaleButton.Opacity = 1;

            MenuSummariesButton.IsHitTestVisible = true;
            MenuSummariesButton.Opacity = 1;
        }

        private void Open_Stock_Click(object sender, RoutedEventArgs e)
        {
            Open_Child_Window(stock);
        }

        private void Open_Auth_Click(object sender, RoutedEventArgs e)
        {
            Open_Child_Window(authWindow);
        }

        private void Open_Sale_Click(object sender, RoutedEventArgs e)
        {
            Open_Child_Window(sale);
        }
        private void Open_Summaries_Click(object sender, RoutedEventArgs e)
        {
            Open_Child_Window(summaries);
        }

        private void Open_Child_Window(Window childToOpen)
        {
            childToOpen.ShowDialog();
        }
    }
}
