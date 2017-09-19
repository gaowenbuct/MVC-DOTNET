using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Utils.Tests
{
    [TestClass()]
    public class FileHelperTests
    {
        [TestMethod()]
        public void ParseTableNameFromSQLTest()
        {
            string result = string.Empty;
            string sql = "SELECT 1 FROM TABLEA WHERE 1=1 FROM TABLE B WHERE 1=2";
            result = FileHelper.GetFileNameBySql(sql);
            Assert.AreEqual(result, "TABLEA");
        }

        [TestMethod()]
        public void FilterSpecialCharTest()
        {
            string s = "a我bc,中国\"人民bc";
            string s1 = FileHelper.FilterSpecialChar(s);
        }
    }
}