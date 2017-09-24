using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MVC.Utils
{
    public class CfgReader
    {
        public static string RetailOrderQueryPigeSize {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RetailOrderQueryPigeSize"])){
                    return ConfigurationManager.AppSettings["RetailOrderQueryPigeSize"];
                }
                else
                {
                    return "10";
                }
            }
        }
        public static string ModelGroupDataBeginYear {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["ModelGroupDataBeginYear"])){
                    return ConfigurationManager.AppSettings["ModelGroupDataBeginYear"];
                }
                else
                {
                    return "1990";
                }
            }
        }
        public static string FileDataFilePath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["FileDataFilePath"]))
                {
                    return ConfigurationManager.AppSettings["FileDataFilePath"];
                }
                else
                {
                    return "";
                }
            }
        }
    }
}