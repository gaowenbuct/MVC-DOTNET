using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Models.Common;
using MVC.Services.Common.Impl;
using MVC.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.Common.Impl.Tests
{
    [TestClass()]
    public class ProductServiceImplTests
    {
        [TestMethod()]
        public void doFindAllSeriesTest()
        {
            ProductService service = new ProductServiceImpl();
            service.doFindAllSeries();
        }
        [TestMethod()]
        public void initData()
        {
            List<Series> list = new List<Series>();
            Series vo = null;

            for (int i = 0; i <= 20; i++)
            {
                vo = new Series();
                vo.SeriesId = i;
                vo.SeriesCode = i.ToString()+i.ToString();
                vo.SeriesName = i.ToString() + i.ToString() + i.ToString();
                list.Add(vo);
            }

            using (StreamWriter sw = new StreamWriter(FileHelper.GetFileNameByTable("SERIES"), false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(list));
            }
            return;
        }
    }
}