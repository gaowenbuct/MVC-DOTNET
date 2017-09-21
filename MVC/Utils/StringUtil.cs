using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Utils
{
    public class StringUtil
    {
        public static string FormatTime(string strTime)
        {
            if (string.IsNullOrWhiteSpace(strTime) || strTime == "0")
            {
                return string.Empty;
            }
            else
            {
                string strTemp = strTime.PadLeft(6, '0');
                return strTemp.Substring(0, 2) + ":" + strTemp.Substring(2, 2) + ":" + strTemp.Substring(4, 2);
            }
        }
        public static string FormatDate(string strDate)
        {
            if (string.IsNullOrEmpty(strDate) && strDate != "0")
            {
                return string.Empty;
            }
            else
            {
                return strDate.Substring(0, 4) + "/" + strDate.Substring(4, 2) + "/" + strDate.Substring(6, 2);
            }
        }
    }
}