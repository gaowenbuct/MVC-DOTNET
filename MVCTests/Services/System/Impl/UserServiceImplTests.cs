using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Models.System;
using MVC.Services.System.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.System.Impl.Tests
{
    [TestClass()]
    public class UserServiceImplTests
    {
        [TestMethod()]
        public void doCreateTest()
        {
            User user = new User();
            user.username = "1";
            user.password = "1";
            UserService service = new UserServiceImpl();
            service.doCreate(user);
        }
    }
}