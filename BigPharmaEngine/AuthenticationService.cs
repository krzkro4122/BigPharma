using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using BigPharmaEngine.Models;

namespace BigPharmaEngine
{
    public interface IAuthenticationService
    {
        bool Register(string username, string email, string password, string confirmPassword);
        bool Login(string username, string password);
    }
    public class AuthenticationService
    {
        public AuthenticationService() { }

        public bool Register(string username, string email, string password, string confirmPassword)
        {
            if (password == confirmPassword)
            {
                User user = new User() { Email = email, Username = username, Password = password };
                SQLiteDataAccess.SaveUser(user);
                return true;
            }

            return false;
        }

        public bool Login(string username, string password)
        {
            foreach (var user in SQLiteDataAccess.LoadUsers())
            {
                if (user.Username == username && user.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
