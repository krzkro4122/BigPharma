using BigPharmaEngine;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BigPharma
{
    public partial class AuthWindow : Window
    {
        private Authenticator authenticator;
        public AuthWindow()
        {
            InitializeComponent();
            authenticator = new();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (authenticator.Login(Login_Username.Text, Login_Password.Text))
            {
                MainWindow mainWindow = (MainWindow) Application.Current.MainWindow;
                mainWindow.Unlock_Resources();
                this.Hide();
            }
            else
            {
                LoginWarningLabel.Content = "Invalid username or password";
            }
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (authenticator.Register(Register_Username.Text, Register_Email.Text, Register_Password.Text, Register_ConfirmPassword.Text))
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.Unlock_Resources();
                this.Hide();
            }
            else
            {
                RegisterWarningLabel.Content = "Passwords do not match";
            }
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
