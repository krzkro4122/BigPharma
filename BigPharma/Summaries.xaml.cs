using System.ComponentModel;
using System.Windows;

namespace BigPharma
{
    public partial class Summaries : Window
    {
        public Summaries()
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
