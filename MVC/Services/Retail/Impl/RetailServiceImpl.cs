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
        public List<County> doFindCountyAll()
        {
            BaseDao<County> baseDao = DaoFactory<County>.CreateBaseDao(typeof(County));
            string sql = "SELECT COUNTY.VSXJDM AS CountyCode,COUNTY.VSXJMC AS CountyName,CITY.VSDSDM AS CityCode,CITY.VSDSMC AS CityName," +
                "PROVINCE.VSSSDM AS ProvinceCode,PROVINCE.VSSSMC AS ProvinceName FROM SJVDTALIB.ISM10 COUNTY,SJVDTALIB.ISM09 CITY,SJVDTALIB.ISM08 PROVINCE " +
                "WHERE COUNTY.ZMDWDM='08' AND CITY.ZMDWDM='08' AND PROVINCE.ZMDWDM='08' AND COUNTY.VSDSDM=CITY.VSDSDM AND CITY.VSSSDM=PROVINCE.VSSSDM";
            return baseDao.FindAll(sql);
        }
        public List<Dealer> doFindDealerAll()
        {
            BaseDao<Dealer> baseDao = DaoFactory<Dealer>.CreateBaseDao(typeof(Dealer));
            string sql = "SELECT DEALER.ZMKHDM AS DealerCode,DEALER.ZMKHMC AS DealerName,DEALER.ZMNDDM AS NetCode,EXTEND.JSFXDM AS RegionCode FROM SJVDTALIB.XTM16 DEALER,SJVDTALIB.ISM02 EXTEND " +
                "WHERE DEALER.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND DEALER.ZMKHDM=EXTEND.ZMKHDM";
            return baseDao.FindAll(sql);
        }
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