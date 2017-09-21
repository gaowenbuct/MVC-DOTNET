using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.System
{
    public class DealerUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmployeeName { get; set; }
        public string DealerCode { get; set; }
        public string RegionCode { get; set; }
        public string RightLevel { get; set; }
    }
}