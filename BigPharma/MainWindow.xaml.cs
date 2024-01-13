using System.ComponentModel;
using System.Windows;

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
            authWindow.Close();
            stock.Close();
            sale.Close();
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
