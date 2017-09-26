using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Daos;
using MVC.Services.System.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.System.Impl.Tests
{
    [TestClass()]
    public class VmsUserServiceImplTests
    {
        [TestMethod()]
        public void doGetDealerCodeByUsernameTest()
        {
            DealerService dealerService = new DealerServiceImpl();
            string result = string.Empty;
            result= dealerService.doGetDealerCodeByUsername("PGMTF");
            Assert.AreNotEqual(result, string.Empty);
            result = dealerService.doGetDealerCodeByUsername("PGMTF1");
            Assert.AreEqual(result, string.Empty);
        }
    }
}