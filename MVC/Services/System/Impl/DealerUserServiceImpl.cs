using MVC.Daos;
using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.System.Impl
{
    public class DealerUserServiceImpl : DealerUserService
    {
        public string doGetDealerCodeByUsername(string username)
        {
            BaseDao<string> baseDao = DaoFactory<string>.CreateBaseDao(typeof(string));
            string sql = "SELECT ISKHDM FROM SJVDTALIB.ISM01 WHERE ISDWDM='08' AND ISJLZT='Y' AND ISCZXM=@USERNAME";
            IDictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("USERNAME", username);
            /*if (criteria.QueryCondition.Count > 0)
            {
                parms = new Dictionary<string, object>();
                if(!string.IsNullOrWhiteSpace(criteria.QueryCondition["ORDER_NO"]))
                parms.Add("ORDER_NO", criteria.QueryCondition["ORDER_NO"]);
            }*/
            return baseDao.nativeQuerySqlReturnString(sql, parms);
        }
        public DealerUser doGetDealerUserInfo(string username)
        {
            BaseDao<DealerUser> baseDao = DaoFactory<DealerUser>.CreateBaseDao(typeof(DealerUser));
            string sql = "SELECT ISCZXM AS Username,EBPSWD AS Password,ISRYXM AS EmployeeName,ISKHDM AS DealerCode,ISFXDM AS RegionCode,ZMCZJB AS RightLevel " +
                "FROM SJVDTALIB.ISM01 DEALER_USER WHERE ISDWDM='08' AND ISJLZT='Y' AND ISCZXM=@USERNAME";
            return baseDao.FindByid(sql, "USERNAME", username);
        }
    }
}