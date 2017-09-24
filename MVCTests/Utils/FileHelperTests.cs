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
            Assert.AreEqual(s1, "a我bc中国人民bc");
        }

        [TestMethod()]
        public void GetFileNameBySqlTest()
        {
            string s = "SELECT ISCZXM AS Username, EBPSWD AS Password, ISRYXM AS EmployeeName, ISKHDM AS DealerCode, ISFXDM AS RegionCode, ZMCZJB AS RightLevel " +
                "FROM SJVDTALIB.ISM01 DEALER_USER WHERE ISDWDM='08' AND ISJLZT='Y' AND ISCZXM=@USERNAME";
            string s1 = FileHelper.GetFileNameBySql(s);
            Assert.AreEqual(s1, CfgReader.FileDataFilePath+"\\"+"DEALER_USER"+".txt");
            s = "SELECT * FROM SJVDTALIB.IST15 RETAIL, SJVDTALIB.VST17 WHERE";
            s1 = FileHelper.GetFileNameBySql(s);
            Assert.AreEqual(s1, CfgReader.FileDataFilePath + "\\" + "RETAIL" + ".txt");
        }

        [TestMethod()]
        public void PropertyMatchTest()
        {
            Assert.IsTrue(FileHelper.PropertyMatch("ModelCode","MODEL_CODE"));
        }
    }
}