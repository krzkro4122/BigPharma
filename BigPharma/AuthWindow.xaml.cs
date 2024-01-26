using BigPharmaEngine;
using System.ComponentModel;
using System.Windows;

namespace BigPharma
{
    public partial class AuthWindow
    {
        private Authenticator authenticator;
        public AuthWindow()
        {
            InitializeComponent();
            authenticator = new();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (authenticator.Login(LoginUsername.Text, LoginPassword.Text))
            {
                if(Application.Current.MainWindow is not MainWindow mainWindow) return;
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
            if (authenticator.Register(RegisterUsername.Text, RegisterEmail.Text, RegisterPassword.Text, RegisterConfirmPassword.Text))
            {
                if(Application.Current.MainWindow is not MainWindow mainWindow) return;
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
