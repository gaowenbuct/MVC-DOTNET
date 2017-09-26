using MVC.Models.Common;
using MVC.Models.Retail;
using MVC.Services.Common;
using MVC.Services.Common.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Cache
{
    public class CommonCache
    {
        private static IDictionary<string, string> regionDic;
        public static IDictionary<string, string> GetRegionDic()//ISM12
        {
            if (regionDic == null)
            {
                regionDic = new Dictionary<string, string>
                {
                    {"2216666","江苏"},
                    {"2036666","华北"},
                    {"2208888","大用户"},
                    {"2436666","中南"},
                    {"2456666","华中"},
                    {"2716666","西北"},
                    {"2256666","山东"},
                    {"2109999","北方"},
                    {"2319999","华东"},
                    {"2619999","西南"},
                    {"2519999","华南"},
                    {"2119999","东北"},
                    {"2659999","云桂黔"}
                };
            }
            return regionDic;
        }
        private static CommonService commonService = new CommonServiceImpl();
        private static IDictionary<string, County> countyDic;
        public static IDictionary<string, County> GetCountyDic()//ISM10,09,08
        {
            if (countyDic == null)
            {
                List<County> list = commonService.doFindCountyAll();
                countyDic = list.ToDictionary(x => x.CountyCode, x => x);
            }
            return countyDic;
        }
    }
}