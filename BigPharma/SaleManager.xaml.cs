using System.ComponentModel;
using System.Windows;

namespace BigPharma
{
    public partial class SaleManager : Window
    {
        public SaleManager()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() != typeof(MainWindow))
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
