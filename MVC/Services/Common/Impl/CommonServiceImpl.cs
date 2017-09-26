using MVC.Daos;
using MVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Common.Impl
{
    public class CommonServiceImpl:CommonService
    {
        public List<County> doFindCountyAll()
        {
            BaseDao<County> baseDao = DaoFactory<County>.CreateBaseDao(typeof(County));
            string sql = "SELECT COUNTY.VSXJDM AS CountyCode,COUNTY.VSXJMC AS CountyName,CITY.VSDSDM AS CityCode,CITY.VSDSMC AS CityName," +
                "PROVINCE.VSSSDM AS ProvinceCode,PROVINCE.VSSSMC AS ProvinceName FROM SJVDTALIB.ISM10 COUNTY,SJVDTALIB.ISM09 CITY,SJVDTALIB.ISM08 PROVINCE " +
                "WHERE COUNTY.ZMDWDM='08' AND CITY.ZMDWDM='08' AND PROVINCE.ZMDWDM='08' AND COUNTY.VSDSDM=CITY.VSDSDM AND CITY.VSSSDM=PROVINCE.VSSSDM";
            return baseDao.FindAll(sql);
        }
    }
}