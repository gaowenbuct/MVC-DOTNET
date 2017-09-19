using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.System
{
    public class User
    {
        public long userNo { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}