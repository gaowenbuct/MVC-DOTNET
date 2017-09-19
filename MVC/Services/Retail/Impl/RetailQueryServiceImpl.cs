using MVC.Daos;
using MVC.Services.Impl;
using MVC.Services.Retail.Vo;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace MVC.Services.Retail.Impl
{
    public class RetailQueryServiceImpl : BaseServiceImpl, RetailQueryService
    {
        public PageResult<RetailOrderQueryVo> doFindRetailOrderByCondition(QueryCriteria criteria)
        {
            PageResult<RetailOrderQueryVo> result = null;
            BaseDao<RetailOrderQueryVo> baseDao = DaoFactory<RetailOrderQueryVo>.CreateBaseDao(typeof(RetailOrderQueryVo));

            String sql = "SELECT '1' AS Vin,'1' AS OrderNo,'1' AS RetailTime,'1' AS OutStockDate,'1' AS InvoiceDate," +
                "'1' AS ModelCode,'1' AS Color,'1' AS ModelYear,'1' AS ModelVersion,'1' AS Interior,'1' AS PrList," +
                "'1' AS CustomerName,'1' AS SaleSource,'1' AS OrderStatus,'1' AS Accessory,'1' AS Club" +
                " FROM RETAIL_ORDER";// WHERE ORDER_NO=@ORDER_NO";
            string sqlCount = "SELECT COUNT(1) FROM RETAIL_ORDER";
            IDictionary<string, object> parms = null;

            /*if (criteria.QueryCondition.Count > 0)
            {
                parms = new Dictionary<string, object>();
                if(!string.IsNullOrWhiteSpace(criteria.QueryCondition["ORDER_NO"]))
                parms.Add("ORDER_NO", criteria.QueryCondition["ORDER_NO"]);
            }*/

            result = baseDao.nativeQuerySQL(sql, sqlCount, parms,criteria.StartIndex, criteria.PageSize); 
            return result;
        }
        public List<RetailOrderQueryVo> doFindRetailOrderByCondition(NameValueCollection queryString)
        {
            List<RetailOrderQueryVo> result = null;
            BaseDao<RetailOrderQueryVo> baseDao = DaoFactory<RetailOrderQueryVo>.CreateBaseDao(typeof(RetailOrderQueryVo));

            String sql = "SELECT '1' AS Vin,'1' AS OrderNo,'1' AS RetailTime,'1' AS OutStockDate,'1' AS InvoiceDate," +
                "'1' AS ModelCode,'1' AS Color,'1' AS ModelYear,'1' AS ModelVersion,'1' AS Interior,'1' AS PrList," +
                "'1' AS CustomerName,'1' AS SaleSource,'1' AS OrderStatus,'1' AS Accessory,'1' AS Club" +
                " FROM RETAIL_ORDER";
            IDictionary<string, object> parms = null;

            result = baseDao.nativeQuerySQL(sql, parms);
            return result;
        }
        public byte[] doExportRetailOrder(List<RetailOrderQueryVo> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("VIN码,零售订单,零售时间,销售出库日期,发票日期,车辆,颜色,车型年,版本号,内饰,选装包,用户,销售来源,");
            sb.Append("订单状态,是否装配原装附件包,是否加入会员俱乐部");
            sb.AppendLine();
            foreach (var item in list) {
                sb.Append(item.Vin + ",");
                sb.Append(item.OrderNo + ",");
                sb.Append(item.RetailTime + ",");
                sb.Append(item.OutStockDate + ",");
                sb.Append(item.InvoiceDate + ",");
                sb.Append(item.ModelCode + ",");
                sb.Append(item.Color + ",");
                sb.Append(item.ModelYear + ",");
                sb.Append(item.ModelVersion + ",");
                sb.Append(item.Interior + ",");
                sb.Append(item.PrList + ",");
                
                sb.Append(FileHelper.FilterSpecialChar(item.CustomerName) + ",");
                sb.Append(item.SaleSource + ",");
                sb.Append(item.OrderStatus + ",");
                sb.Append(item.Accessory + ",");
                sb.Append(item.Club);
                sb.AppendLine();
            }
            return FileHelper.CsvFormat(sb);
        }
    }
}