using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPharmaEngine
{
    public class User
    {
        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
        public int Id { get; set; }
        public string Username
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }
    }
}
