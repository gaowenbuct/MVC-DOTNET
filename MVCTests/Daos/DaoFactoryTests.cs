using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Daos;
using MVC.Models.System;
using MVC.Services.Retail.Vo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Daos.Tests
{
    [TestClass()]
    public class DaoFactoryTests
    {
        [TestMethod()]
        public void CreateBaseDaoTest()
        {
            BaseDao<RetailOrderQueryVo> baseDao = DaoFactory<RetailOrderQueryVo>.CreateBaseDao(typeof(RetailOrderQueryVo));
            Assert.IsNotNull(baseDao);
        }

        [TestMethod()]
        public void CreateUserDaoTest()
        {
            UserDao userDao = DaoFactory<User>.CreateUserDao();
            Assert.IsNotNull(userDao);
        }
        
        /*public void Test()
        {
            test test = (test)Assembly.Load("MVC").CreateInstance("MVC.Daos.Db2Impl.testImpl");
            test1<User> test1 = (test1<User>)Assembly.Load("MVC").CreateInstance("MVC.Daos.Db2Impl.testImpl<User>");
            test1<User> test2 = (test1<User>)Assembly.Load("MVC").CreateInstance("MVC.Daos.Db2Impl.testImpl<T>");
            //test1<User> test3 = (test1<User>)Assembly.Load("MVC").CreateInstance("MVC.Daos.Db2Impl.test1Impl`1");
            //test1 test3 = (test1)Assembly.Load("MVC").CreateInstance("MVC.Daos.Db2Impl.testImpl");
            Assembly assembly = Assembly.Load("MVC");
            Type type = assembly.GetType("MVC.Daos.Db2Impl.test1Impl`1");
            Type t = type.MakeGenericType(typeof(User));
            object obj = assembly.CreateInstance(t.FullName);
        }*/
    }
}