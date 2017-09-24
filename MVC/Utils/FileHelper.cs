using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MVC.Utils
{
    public class FileHelper
    {
        private static string path = CfgReader.FileDataFilePath;//AppDomain.CurrentDomain.BaseDirectory;
        private static string ParseTableNameFromSQL(string sql)
        {
            if(string.IsNullOrWhiteSpace(sql))
            {
                return string.Empty;
            }
            string[] sArray=sql.Split(new string[] { "FROM", "WHERE" },3, StringSplitOptions.None);
            if (sArray.Length>1)
            {
                return sArray[1].Trim().Trim('"').Split(new string[] { " ","," }, 3, StringSplitOptions.None)[1];
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetFileNameBySql(string sql)
        {
            return path + "\\" + ParseTableNameFromSQL(sql)+".txt";
        }
        public static string GetFileNameByTable(string tableName)
        {
            return path + "\\" + tableName + ".txt";
        }
        public static byte[] CsvFormat(StringBuilder sb)
        {
            byte[] bytetxt = Encoding.UTF8.GetBytes(sb.ToString());
            int preambleLength = Encoding.UTF8.GetPreamble().Length;
            byte[] outBuffer = new byte[bytetxt.Length + preambleLength];
            Array.Copy(Encoding.UTF8.GetPreamble(), 0, outBuffer, 0, preambleLength);
            Array.Copy(bytetxt, 0, outBuffer, preambleLength, bytetxt.Length);
            return outBuffer;
        }
        public static string FilterSpecialChar(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;
            int len = source.Length;
            char[] dest = new char[len];
            int point = 0;
            for (int i = 0; i < len; i++)
            {
                char c = source[i];
                if (c != '\r' && c != '\n' && c != '\t'&&c!=','&&c!='"')
                    dest[point++] = c;
            }
            return new String(dest, 0, point);
        }
        public static bool PropertyMatch(string propertyName,string parmNameName)
        {
            if (propertyName.ToUpper().Equals(parmNameName.Replace("_", "")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}