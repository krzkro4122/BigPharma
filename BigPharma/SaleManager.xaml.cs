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
            this.Hide();
            e.Cancel = true;
        }
    }
}
