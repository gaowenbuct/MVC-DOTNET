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
        private static readonly BaseDao<RetailOrderQueryVo> baseDao = DaoFactory<RetailOrderQueryVo>.CreateBaseDao(typeof(RetailOrderQueryVo));
        public PageResult<RetailOrderQueryVo> doFindRetailOrderByCondition(QueryCriteria criteria)
        {
            PageResult<RetailOrderQueryVo> result = null;
            Dictionary<string, object> parms = new Dictionary<string, object>();

            string selectFields = "ID AS Vin,ID AS OrderNo,ID AS RetailTime,ID AS OutStockDate,ID AS InvoiceDate," +
                "ID AS ModelCode,ID AS Color,ID AS ModelYear,ID AS ModelVersion,ID AS Interior,ID AS PrList," +
                "ID AS CustomerName,ID AS SaleSource,ID AS OrderStatus,ID AS Accessory,ID AS Club";
            string orderField = "VSLSDD";
            string tableName = "SJVDTALIB.IST15 RETAIL_ORDER";

            string condition = "AND ZMDWDM='08' AND ZMKHDM=@DEALER_CODE AND ZMFXDM=@REGION_CODE ";
            parms.Add("DEALER_CODE", criteria.QueryCondition["dealerCode"]);
            parms.Add("REGION_CODE", criteria.QueryCondition["regionCode"]);
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["modelCode"]))
            {
                condition += "AND BSCLDM=@MODEL_CODE ";
                parms.Add("MODEL_CODE", criteria.QueryCondition["modelCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["colorCode"]))
            {
                condition += "AND XSYSDM=@COLOR_CODE ";
                parms.Add("COLOR_CODE", criteria.QueryCondition["colorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["interiorCode"]))
            {
                condition += "AND BSNSYM=@INTERIOR_CODE ";
                parms.Add("INTERIOR_CODE", criteria.QueryCondition["interiorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["customerName"]))
            {
                condition += "AND LOCATE(@INTERIOR_CODE,VSYHMC)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["vin"]))
            {
                condition += "AND CYVINM=@VIN ";
                parms.Add("VIN", criteria.QueryCondition["vin"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["invoiceNo"]))
            {
                condition += "AND DMFPHM=@INVOICE_NO ";
                parms.Add("INVOICE_NO", criteria.QueryCondition["invoiceNo"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["saleSource"]))
            {
                condition += "AND VSXSLY=@SALE_SOURCE ";
                parms.Add("SALE_SOURCE", criteria.QueryCondition["saleSource"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["saleStatus"]))
            {
                condition += "AND VSDDZT=@SALE_STATUS ";
                parms.Add("SALE_STATUS", criteria.QueryCondition["saleStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["outStockStatus"]))
            {
                condition += "AND VSPCCL=@OUTSTOCK_STATUS ";
                parms.Add("OUTSTOCK_STATUS", criteria.QueryCondition["outStockStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["startDateCreate"]))
            {
                condition += "AND VSDJRQ>=@START_DATE_CREATE ";
                parms.Add("START_DATE_CREATE", long.Parse(criteria.QueryCondition["startDateCreate"].ToString().Replace("-","")));
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["endDateCreate"]))
            {
                condition += "AND VSDJRQ<=@END_DATE_CREATE ";
                parms.Add("END_DATE_CREATE", long.Parse(criteria.QueryCondition["endDateCreate"].ToString().Replace("-", "")));
            }

            string sql = DB2Helper.GetPageSql(selectFields, orderField, tableName, condition, criteria.StartIndex, criteria.PageSize);
            string sqlCount = DB2Helper.GetCountSql(tableName,condition);

            result = baseDao.nativeQuerySql(sql, sqlCount, parms,criteria.StartIndex, criteria.PageSize); 
            return result;
        }
        public List<RetailOrderQueryVo> doFindRetailOrderByCondition(NameValueCollection queryString)
        {
            List<RetailOrderQueryVo> result = null;

            String sql = "SELECT '1' AS Vin,'1' AS OrderNo,'1' AS RetailTime,'1' AS OutStockDate,'1' AS InvoiceDate," +
                "'1' AS ModelCode,'1' AS Color,'1' AS ModelYear,'1' AS ModelVersion,'1' AS Interior,'1' AS PrList," +
                "'1' AS CustomerName,'1' AS SaleSource,'1' AS OrderStatus,'1' AS Accessory,'1' AS Club" +
                " FROM RETAIL_ORDER";
            IDictionary<string, object> parms = null;

            result = baseDao.nativeQuerySql(sql, parms);
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