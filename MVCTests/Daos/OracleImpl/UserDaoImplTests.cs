using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Daos.OracleImpl;
using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Daos.OracleImpl.Tests
{
    [TestClass()]
    public class UserDaoImplTests
    {
        UserDaoImpl dao = new UserDaoImpl();
        [TestMethod()]
        public void createTest()
        {
            User user = new User();
            user.username = "gw";
            user.password = "gw";
            dao.create(user);
        }
    }
}