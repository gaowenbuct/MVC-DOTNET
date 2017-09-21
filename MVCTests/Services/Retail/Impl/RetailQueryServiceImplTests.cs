using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC.Services.Retail.Impl;
using MVC.Services.Retail.Vo;
using MVC.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Services.Retail.Impl.Tests
{
    [TestClass()]
    public class RetailQueryServiceImplTests
    {
        private RetailQueryService retailQueryService = new RetailQueryServiceImpl();
        [TestMethod()]
        public void doFindRetailOrderByConditionTest()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("dealerCode", "2250010");
            nvc.Add("regionCode", "2256666");
            nvc.Add("startIndex", "0");
            nvc.Add("pageSize", "3");
            nvc.Add("startDateCreate", "2017-09-01");
            nvc.Add("endDateCreate", "2017-09-21");
            QueryCriteria queryCriteria = new QueryCriteria(nvc);
            PageResult<RetailOrderQueryVo> result=retailQueryService.QueryRetailOrderList(queryCriteria);
        }
        [TestMethod()]
        public void initData()
        {
            List<RetailOrderQueryVo> list = new List<RetailOrderQueryVo>();
            RetailOrderQueryVo vo = null;
           
            for (int i = 0; i <= 20; i++)
            {
                vo = new RetailOrderQueryVo();
                vo.Vin = "VIN"+i;
                vo.OrderNo = "ORDER"+i;
                list.Add(vo);
            }

            using (StreamWriter sw = new StreamWriter(FileHelper.GetFileNameByTable("RETAIL_OERDER"), false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(list));
            }
            return;
        }
    }
}