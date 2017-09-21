using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Utils
{
    public static class Converter
    {
        public static string toJson(this IDictionary<string, object> dic)
        {
            return dic == null ? null : JsonConvert.SerializeObject(dic);
        }

        public static string toJson(this IDictionary<string, int> dic)
        {
            return dic == null ? null : JsonConvert.SerializeObject(dic);
        }
    }
}