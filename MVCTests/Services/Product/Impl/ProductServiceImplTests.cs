﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Models.Product;
using MVC.Services.Product;
using MVC.Services.Product.Impl;
using MVC.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.Product.Impl.Tests
{
    [TestClass()]
    public class ProductServiceImplTests
    {
        private ProductService service = new ProductServiceImpl();
        [TestMethod()]
        public void doFindAllSeriesTest()
        {
            service.doFindSeriesAll();
        }
        [TestMethod()]
        public void initData()
        {
            List<Series> list = new List<Series>();
            Series vo = null;

            for (int i = 0; i <= 20; i++)
            {
                vo = new Series();
                vo.SeriesCode = i.ToString() + i.ToString();
                vo.SeriesName = i.ToString() + i.ToString() + i.ToString();
                list.Add(vo);
            }

            using (StreamWriter sw = new StreamWriter(FileHelper.GetFileNameByTable("SERIES"), false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(list));
            }
            return;
        }

        [TestMethod()]
        public void doGetModelInfoTest()
        {
            Model model = service.doGetModelInfo("A422H2");
            Assert.IsNotNull(model);
        }

        [TestMethod()]
        public void doFindModelTest()
        {
            IDictionary<string,string> list = service.doFindModelList("16", "02");
            Assert.IsNotNull(list);
        }

        [TestMethod()]
        public void doFindModelPrAllTest()
        {
            List<ModelGroup> list = service.doFindModelGroupAll();
        }
    }
}