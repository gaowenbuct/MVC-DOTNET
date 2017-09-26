using MVC.Daos;
using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.System.Impl
{
    public class DealerServiceImpl : DealerService
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
        public List<Dealer> doFindDealerAll()
        {
            BaseDao<Dealer> baseDao = DaoFactory<Dealer>.CreateBaseDao(typeof(Dealer));
            string sql = "SELECT DEALER.ZMKHDM AS DealerCode,DEALER.ZMKHMC AS DealerName,DEALER.ZMNDDM AS NetCode,EXTEND.JSFXDM AS RegionCode FROM SJVDTALIB.XTM16 DEALER,SJVDTALIB.ISM02 EXTEND " +
                "WHERE DEALER.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND DEALER.ZMKHDM=EXTEND.ZMKHDM";
            return baseDao.FindAll(sql);
        }
    }
}