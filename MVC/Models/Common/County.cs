using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Common
{
    public class County
    {
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }
}