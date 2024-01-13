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
            this.Hide();
            e.Cancel = true;
        }
    }
}
