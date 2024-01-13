using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BigPharma
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
            mainWindow.Unlock_Resources();
            this.Hide();
            e.Cancel = true;
        }
    }
}
