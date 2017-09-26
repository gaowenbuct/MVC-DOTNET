using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.Retail;
using MVC.Daos;

namespace MVC.Services.Retail.Impl
{
    public class RetailServiceImpl : RetailService
    {
        public List<SalesPerson> doFindSalesPersonAll()
        {
            BaseDao<SalesPerson> baseDao = DaoFactory<SalesPerson>.CreateBaseDao(typeof(SalesPerson));
            string sql = "SELECT ZMKHDM AS DealerCode,VSXSRY AS SalesPersonCode,SALES_PERSON.VSXSYC AS SalesPersonName," +
                "SALES_PERSON.VTZJHM AS CertificateNo FROM SJVDTALIB.ISM40 SALES_PERSON WHERE ZMDWDM='08'";
            return baseDao.FindAll(sql);
        }
        public IDictionary<string, string> doFindInsuranceCompanyAll()
        {
            BaseDao<SalesPerson> baseDao = DaoFactory<SalesPerson>.CreateBaseDao(typeof(SalesPerson));
            string sql = "SELECT VSGSDM AS Code,VSGSMC AS Name FROM SJVDTALIB.VSM29 INSURANCE_COMPANY WHERE ZMDWDM='08'";
            return baseDao.FindAllDic(sql);
        }
    }
}