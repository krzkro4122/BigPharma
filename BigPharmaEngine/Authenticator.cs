using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BigPharmaEngine.AuthenticationService;

namespace BigPharmaEngine
{
    public interface IAuthenticator
    {
        bool IsLoggedIn { get; }
        string currentUserName { get; }
        bool Login(string username, string password);
        bool Register(string username, string email, string password, string confirmPassword);
        void Logout();
    }
    public class Authenticator : IAuthenticator
    {

        private readonly AuthenticationService authenticationService;

        public Authenticator()
        {
            this.authenticationService = new AuthenticationService();
            this.currentUserName = "";
        }

        public bool IsLoggedIn => currentUserName != "" && currentUserName != null;
        public string currentUserName { get; private set; }

        public bool Login(string username, string password)
        {
            bool loginSuccessful = authenticationService.Login(username, password);
            if (loginSuccessful)
            {
                currentUserName = username; 
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Register(string username, string email, string password, string confirmPassword)
        {
            return authenticationService.Register(username, email, password, confirmPassword);
        }
        public void Logout()
        {
            currentUserName = "";
        }
    }
}
