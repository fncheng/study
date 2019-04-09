﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }

    }
    public class Register
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
    }
}