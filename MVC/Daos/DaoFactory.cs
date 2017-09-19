using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MVC.Daos
{
    public class DaoFactory<T>
    {
        private static readonly string userPath = ConfigurationManager.AppSettings["UserDao"];
        private static readonly string basePath = ConfigurationManager.AppSettings["BaseDao"];

        public static BaseDao<T> CreateBaseDao(Type t)
        {
            string className = basePath + ".BaseDaoImpl`1";
            Type classType = Assembly.GetExecutingAssembly().GetType(className);
            Type parmType = classType.MakeGenericType(t);
            return (BaseDao<T>)Assembly.GetExecutingAssembly().CreateInstance(parmType.FullName);
        }
        public static UserDao CreateUserDao()
        {
            
            string className = userPath + ".UserDaoImpl";
            //return (MVC.Daos.UserDao)Assembly.Load("MVC").CreateInstance(className);
            return (UserDao)Assembly.GetExecutingAssembly().CreateInstance(className);
        }
    }
}