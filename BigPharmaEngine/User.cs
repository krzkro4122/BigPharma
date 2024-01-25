﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPharmaEngine
{
    public class User
    {

        public int Id { get; set; }
        public required string Username
        {
            get;
            set;
        }

        public required string Email
        {
            get;
            set;
        }

        public required string Password
        {
            get;
            set;
        }
    }
}
