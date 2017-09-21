using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace MVC.Services.Impl
{
    public class BaseServiceImpl:BaseService
    {
        //protected static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName);
        protected static log4net.ILog getLogger(Object obj)
        {
            return log4net.LogManager.GetLogger(obj.GetType());
        }
    }
}