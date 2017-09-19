using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Models.Retail;
using MVC.Services.Retail.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.Retail.Impl.Tests
{
    [TestClass()]
    public class RetailOrderServiceImplTests
    {
        [TestMethod()]
        public void DetailsTest()
        {
            RetailOrderService service = new RetailOrderServiceImpl();
            RetailOrder retailOrder=service.Details("1");
            //Assert.Fail();
        }
    }
}