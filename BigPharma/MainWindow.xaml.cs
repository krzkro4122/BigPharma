using System;
using System.ComponentModel;
using System.Windows;

namespace BigPharma
{
    public partial class MainWindow
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
            authWindow.Close();
            stock.Close();
            sale.Close();
            summaries.Close();
            Console.WriteLine("Quitting...");
        }

        public void Unlock_Resources()
        {
            MenuAuthButton.Content = "_Log out 🔐";
            MenuAuthButton.Click -= Open_Auth_Click;
            MenuAuthButton.Click += Lock_Resources;

            MenuStockButton.IsHitTestVisible = true;
            MenuStockButton.Opacity = 1;
            MenuStockButton.Content = "_Stock Manager 🔢";

            MenuSaleButton.IsHitTestVisible = true;
            MenuSaleButton.Opacity = 1;
            MenuSaleButton.Content = "S_ale Manager 📈";

            MenuSummariesButton.IsHitTestVisible = true;
            MenuSummariesButton.Opacity = 1;
            MenuSummariesButton.Content = "S_ummaries 📊";
        }

        public void Lock_Resources(object sender, RoutedEventArgs e)
        {
            MenuAuthButton.Content = "_Authenticate 🔓";
            MenuAuthButton.Click -= Lock_Resources;
            MenuAuthButton.Click += Open_Auth_Click;

            MenuStockButton.IsHitTestVisible = false;
            MenuStockButton.Opacity = 0.5;
            MenuStockButton.Content = "Stock Manager 🔢";

            MenuSaleButton.IsHitTestVisible = false;
            MenuSaleButton.Opacity = 0.5;
            MenuSaleButton.Content = "Sale Manager 📈";

            MenuSummariesButton.IsHitTestVisible = false;
            MenuSummariesButton.Opacity = 0.5;
            MenuSummariesButton.Content = "Summaries 📊";
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
