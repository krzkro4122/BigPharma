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
            if (sender.GetType() != typeof(MainWindow))
            {                
                MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Unlock_Resources();
                }
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
