using MVC.Cache;
using MVC.Daos;
using MVC.Models.Common;
using MVC.Models.Product;
using MVC.Models.Retail;
using MVC.Models.System;
using MVC.Services.Impl;
using MVC.Services.Retail.Vo;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading;

namespace MVC.Services.Retail.Impl
{
    public class RetailQueryServiceImpl : BaseServiceImpl, RetailQueryService
    {
        public PageResult<RetailOrderQueryListVo>  QueryRetailOrderList(QueryCriteria queryCriteria)
        {
            PageResult<RetailOrderQueryListVo> result = doFindRetailOrderByCondition(queryCriteria);
            if (result.Content.Count > 0)
            {
                IDictionary<string, ModelGroup> modelGroupDic = ProductCache.GetModelGroupDic();
                foreach(var item in result.Content)
                {
                    string key = item.ModelCode + item.Interior + item.PrList;
                    if (modelGroupDic.ContainsKey(key))
                    {
                        item.ModelYear = modelGroupDic[key].ModelYear.ToString();
                        item.ModelVersion = modelGroupDic[key].ModelVersion;
                        item.PrList = modelGroupDic[key].PrList;
                    }
                    else
                    {
                        item.ModelYear = "数据错误";
                        item.ModelVersion="数据错误";
                        item.PrList = item.PrList + "数据错误";
                    }
                    IDictionary<string, string> colorDic = ProductCache.GetColorDic();
                    if ((item.Color!=null) &&colorDic.ContainsKey(item.Color))
                    {
                        item.Color = item.Color+"("+item.Color1+")"+'-'+colorDic[item.Color];
                    }
                    else
                    {
                        item.Color = item.Color + "(" + item.Color1 + ")";
                    }
                    item.SalesSource = RetailCache.GetSalesSource(item.SalesSource);
                    item.OrderStatus = RetailCache.GetOrderStatus(item.OrderStatus);
                    item.Accessory = RetailCache.GetYesNo(item.Accessory);
                    item.Club = RetailCache.GetYesNo(item.Club);
                }
            }
            return result;
        }
        public PageResult<RetailOrderQueryListVo> doFindRetailOrderByCondition(QueryCriteria criteria)
        {
            BaseDao<RetailOrderQueryListVo> baseDao = DaoFactory<RetailOrderQueryListVo>.CreateBaseDao(typeof(RetailOrderQueryListVo));
            string selectFields = string.Empty;
            string tableName = string.Empty;
            string condition = string.Empty;
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["remarkFlag"]) && "Y".Equals(criteria.QueryCondition["remarkFlag"]))
            {
                selectFields = "RETAIL.CYVINM AS Vin,RETAIL.VSLSDD AS OrderNo,GET_DATE_TIME_STRING(RETAIL.VSLSRQ,RETAIL.VSLSSJ) AS RetailDateTime,GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate," +
                    "RETAIL.BSCLDM AS ModelCode,RETAIL.XSYSDM AS Color,RETAIL.BSYSDM AS Color1,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSNSYM AS Interior,RETAIL.BSPNXH AS PrList," +
                    "REMARK.VSYHMC AS CustomerName,RETAIL.VSXSLY AS SalesSource,RETAIL.VSDDZT AS OrderStatus,RETAIL.VSSFFJ AS Accessory,EXTEND.VSJLBZ AS Club";
                tableName="SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND,SJVDTALIB.VST19 REMARK";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND REMARK.ZMDWDM='08' AND RETAIL.VSLSDD=EXTEND.VSLSDD AND RETAIL.VSLSDD=REMARK.VSLSDD " +
                    "AND RETAIL.ZMKHDM=@DEALER_CODE AND RETAIL.ZMFXDM=@REGION_CODE ";
                if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["customerName"]))
                {
                    condition += "AND REMARK.VSYHMC LIKE '%" + criteria.QueryCondition["customerName"] + "%' ";
                }
            }
            else
            {
                selectFields = "RETAIL.CYVINM AS Vin,RETAIL.VSLSDD AS OrderNo,GET_DATE_TIME_STRING(RETAIL.VSLSRQ,RETAIL.VSLSSJ) AS RetailDateTime,GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate," +
                    "RETAIL.BSCLDM AS ModelCode,RETAIL.XSYSDM AS Color,RETAIL.BSYSDM AS Color1,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSNSYM AS Interior,RETAIL.BSPNXH AS PrList," +
                    "RETAIL.VSYHMC AS CustomerName,RETAIL.VSXSLY AS SalesSource,RETAIL.VSDDZT AS OrderStatus,RETAIL.VSSFFJ AS Accessory,EXTEND.VSJLBZ AS Club";
                tableName = "SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND RETAIL.VSLSDD=EXTEND.VSLSDD " +
                    "AND RETAIL.ZMKHDM=@DEALER_CODE AND RETAIL.ZMFXDM=@REGION_CODE ";
                if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["customerName"]))
                {
                    condition += "AND RETAIL.VSYHMC LIKE '%" + criteria.QueryCondition["customerName"] + "%' ";
                }
            }
            
            string orderFields = "RETAIL.VSLSDD";
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("DEALER_CODE", criteria.QueryCondition["dealerCode"]);
            //parms.Add("REGION_CODE", criteria.QueryCondition["regionCode"]);
            parms.Add("REGION_CODE", "9999999");
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["modelCode"]))
            {
                condition += "AND RETAIL.BSCLDM=@MODEL_CODE ";
                parms.Add("MODEL_CODE", criteria.QueryCondition["modelCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["colorCode"]))
            {
                condition += "AND RETAIL.XSYSDM=@COLOR_CODE ";
                parms.Add("COLOR_CODE", criteria.QueryCondition["colorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["interiorCode"]))
            {
                condition += "AND RETAIL.BSNSYM=@INTERIOR_CODE ";
                parms.Add("INTERIOR_CODE", criteria.QueryCondition["interiorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["vin"]))
            {
                condition += "AND RETAIL.CYVINM=@VIN ";
                parms.Add("VIN", criteria.QueryCondition["vin"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["invoiceNo"]))
            {
                condition += "AND RETAIL.DMFPHM=@INVOICE_NO ";
                parms.Add("INVOICE_NO", criteria.QueryCondition["invoiceNo"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["salesSource"]))
            {
                condition += "AND RETAIL.VSXSLY=@SALES_SOURCE ";
                parms.Add("SALES_SOURCE", criteria.QueryCondition["salesSource"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["orderStatus"]))
            {
                condition += "AND RETAIL.VSDDZT=@ORDER_STATUS ";
                parms.Add("ORDER_STATUS", criteria.QueryCondition["orderStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["outStockStatus"]))
            {
                condition += "AND VSPCCL=@OUTSTOCK_STATUS ";
                parms.Add("OUTSTOCK_STATUS", criteria.QueryCondition["outStockStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["startDateCreate"]))
            {
                condition += "AND RETAIL.VSDJRQ>=@START_DATE_CREATE ";
                parms.Add("START_DATE_CREATE", long.Parse(criteria.QueryCondition["startDateCreate"].ToString().Replace("-","")));
            }
            if (!string.IsNullOrWhiteSpace(criteria.QueryCondition["endDateCreate"]))
            {
                condition += "AND RETAIL.VSDJRQ<=@END_DATE_CREATE ";
                parms.Add("END_DATE_CREATE", long.Parse(criteria.QueryCondition["endDateCreate"].ToString().Replace("-", "")));
            }

            string sql = DB2Helper.GetPageSql(selectFields, orderFields, tableName, condition, criteria.StartIndex, criteria.PageSize);
            string sqlCount = DB2Helper.GetCountSql(tableName,condition);
            
            return baseDao.nativeQuerySql(sql, sqlCount, parms, criteria.StartIndex, criteria.PageSize);
        }
        public RetailOrderQueryDetailsVo QueryRetailOrderDetails(string orderNo, string remarkFlag)
        {
            RetailOrderQueryDetailsVo result = doFindRetailOrderById(orderNo, remarkFlag);
            if (result != null)
            {
                IDictionary<string, ModelGroup> modelGroupDic = ProductCache.GetModelGroupDic();
                string key = result.Model + result.Interior + result.PrList;
                if (modelGroupDic.ContainsKey(key))
                {
                    result.ModelYear = modelGroupDic[key].ModelYear.ToString();
                    result.ModelVersion = modelGroupDic[key].ModelVersion;
                    result.PrList = modelGroupDic[key].PrList;
                }
                else
                {
                    result.ModelYear = "数据错误";
                    result.ModelVersion = "数据错误";
                    result.PrList = result.PrList + "数据错误";
                }
                IDictionary<string, Model> modelDic = ProductCache.GetModelDic();
                if (result.Model!=null&&modelDic.ContainsKey(result.Model))
                {
                    result.Model = result.Model + "-" + modelDic[result.Model].ModelName;
                }
                else
                {
                    result.Model = result.Model + "数据错误";
                }
                IDictionary<string, string> colorDic = ProductCache.GetColorDic();
                if ((result.Color != null) && colorDic.ContainsKey(result.Color))
                {
                    result.Color = result.Color + "(" + result.Color1 + ")" + '-' + colorDic[result.Color];
                }
                else
                {
                    result.Color = result.Color + "(" + result.Color1 + ")";
                }
                IDictionary<string, string> interiorDic = ProductCache.GetInteriorDic();
                if (result.Interior != null && interiorDic.ContainsKey(result.Interior))
                {
                    result.Interior = result.Interior + '-' + interiorDic[result.Interior];
                }
                else
                {
                    result.Interior = result.Interior + "数据错误";
                }
                IDictionary<string, County> countyDic = CommonCache.GetCountyDic();
                if (result.County != null && countyDic.ContainsKey(result.County))
                {
                    result.County = result.County + '-' + countyDic[result.County].ProvinceName+'-'+ countyDic[result.County].CityName+
                        '-'+ countyDic[result.County].CountyName;
                }
                else
                {
                    result.County = result.County + "数据错误";
                }
                DealerStockVehicleInfo vehicleInfo= doFindDealerStockVehicleInfoById(result.Vin);
                if (vehicleInfo!=null)
                {
                    result.VehicleInStockDate = vehicleInfo.VehicleInStockDate;
                    result.VehicleOutStockDate = vehicleInfo.VehicleOutStockDate;
                    result.EngineNo = vehicleInfo.EngineNo;
                }
                result.SalesSource = RetailCache.GetSalesSource(result.SalesSource);
                result.OrderStatus = RetailCache.GetOrderStatus(result.OrderStatus);
                result.Accessory = RetailCache.GetYesNo(result.Accessory);
                result.Club = RetailCache.GetYesNo(result.Club);
                result.Gender = RetailCache.GetGender(result.Gender);
                result.InvoiceType = RetailCache.GetInvoiceType(result.InvoiceType);
                result.VehiclePurpose = RetailCache.GetVehiclePurpose(result.VehiclePurpose);
                result.CertificateType = RetailCache.GetVehiclePurpose(result.CertificateType);
                result.Duty = RetailCache.GetVehiclePurpose(result.Duty);
                result.CompanyProperty = RetailCache.GetVehiclePurpose(result.CompanyProperty);
                result.CompanyIndustry = RetailCache.GetVehiclePurpose(result.CompanyIndustry);
                result.SalesType = RetailCache.GetVehiclePurpose(result.SalesType);
                result.UsePoint = RetailCache.GetYesNo(result.UsePoint);
                result.ThreeGuarantees = RetailCache.GetYesNo(result.ThreeGuarantees);
                result.CustomerType = RetailCache.GetCustomerType(result.CustomerType);
            }
            return result;
        }
        public RetailOrderQueryDetailsVo doFindRetailOrderById(string orderNo, string remarkFlag)
        {
            BaseDao<RetailOrderQueryDetailsVo> baseDao = DaoFactory<RetailOrderQueryDetailsVo>.CreateBaseDao(typeof(RetailOrderQueryDetailsVo));
            string selectFields = string.Empty;
            string tableName = string.Empty;
            string condition = string.Empty;
            if (!string.IsNullOrWhiteSpace(remarkFlag) && "Y".Equals(remarkFlag))
            {
                selectFields = "RETAIL.CYVINM AS Vin,RETAIL.VSLSDD AS OrderNo,GET_DATE_TIME_STRING(RETAIL.VSLSRQ,RETAIL.VSLSSJ) AS RetailDateTime," +
                    "GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate," +
                    "RETAIL.BSCLDM AS Model,RETAIL.XSYSDM AS Color,RETAIL.BSYSDM AS Color1,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSNSYM AS Interior,RETAIL.BSPNXH AS PrList," +
                    "REMARK.VSYHMC AS CustomerName,RETAIL.VSXSLY AS SalesSource,RETAIL.VSDDZT AS OrderStatus,RETAIL.VSSFFJ AS Accessory,EXTEND.VSJLBZ AS Club," +
                    "GET_DATE_STRING(RETAIL.VSDJRQ) AS CreateDate,'' AS EngineNo,RETAIL.DMFPLX AS InvoiceType,RETAIL.DMFPHM AS InvoiceNo,REMARK.VSLXDZ AS Address,REMARK.VSRYXB AS Gender," +
                    "REMARK.VSXJDM AS County,RETAIL.DMKHLB AS CustomerType,RETAIL.DMCLYT AS VehiclePurpose,REMARK.VTZJLX AS CertificateType,REMARK.VTZJHM AS CertificateNo," +
                    "REMARK.DMDHHM AS Telephone,REMARK.DMSJHM AS Mobile,REMARK.VSDJYJ AS Email,REMARK.DMLXRM AS Contact,REMARK.DMYZBM AS ZipCode,RETAIL.DMZWZW AS Duty," +
                    "RETAIL.DMGSXZ AS CompanyProperty,RETAIL.DMCSHY AS CompanyIndustry,RETAIL.VSLSSX AS SalesType,RETAIL.VSLSKH AS ResellDealer," +
                    "CHAR(RETAIL.VSLSDJ) AS Price,CHAR(RETAIL.VSLSSL) AS Quantity,RETAIL.VSXSRY AS SalesPerson,'' AS VehicleInStockDate,'' AS VehicleOutStockDate," +
                    "RETAIL.VSLSBZ AS Remark,RETAIL.DMJFSZ AS UsePoint,CHAR(RETAIL.VSZFJE) AS PointAmount,RETAIL.VSPSLS AS PosFlowNo,RETAIL.VSLPCX AS Prize," +
                    "EXTEND.VSSBCL AS ThreeGuarantees,EXTEND.XSCPBZ AS LicenseRemark,EXTEND.XSLSCP AS LicenseNo,REMARK.VSSFWS AS OutSource";
                tableName = "SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND,SJVDTALIB.VST19 REMARK";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND REMARK.ZMDWDM='08' AND RETAIL.VSLSDD=EXTEND.VSLSDD AND RETAIL.VSLSDD=REMARK.VSLSDD " +
                    "AND RETAIL.VSLSDD=@ORDER_CODE";
            }
            else
            {
                selectFields = "RETAIL.CYVINM AS Vin,RETAIL.VSLSDD AS OrderNo,GET_DATE_TIME_STRING(RETAIL.VSLSRQ,RETAIL.VSLSSJ) AS RetailDateTime," +
                    "GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate," +
                    "RETAIL.BSCLDM AS Model,RETAIL.XSYSDM AS Color,RETAIL.BSYSDM AS Color1,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSNSYM AS Interior,RETAIL.BSPNXH AS PrList," +
                    "RETAIL.VSYHMC AS CustomerName,RETAIL.VSXSLY AS SalesSource,RETAIL.VSDDZT AS OrderStatus,RETAIL.VSSFFJ AS Accessory,EXTEND.VSJLBZ AS Club," +
                    "GET_DATE_STRING(RETAIL.VSDJRQ) AS CreateDate,'' AS EngineNo,RETAIL.DMFPLX AS InvoiceType,RETAIL.DMFPHM AS InvoiceNo,RETAIL.VSLXDZ AS Address,RETAIL.VSRYXB AS Gender," +
                    "RETAIL.VSXJDM AS County,RETAIL.DMKHLB AS CustomerType,RETAIL.DMCLYT AS VehiclePurpose,RETAIL.VTZJLX AS CertificateType,RETAIL.VTZJHM AS CertificateNo," +
                    "RETAIL.DMDHHM AS Telephone,RETAIL.DMSJHM AS Mobile,RETAIL.VSDJYJ AS Email,RETAIL.DMLXRM AS Contact,RETAIL.DMYZBM AS ZipCode,RETAIL.DMZWZW AS Duty," +
                    "RETAIL.DMGSXZ AS CompanyProperty,RETAIL.DMCSHY AS CompanyIndustry,RETAIL.VSLSSX AS SalesType,RETAIL.VSLSKH AS ResellDealer," +
                    "CHAR(RETAIL.VSLSDJ) AS Price,CHAR(RETAIL.VSLSSL) AS Quantity,RETAIL.VSXSRY AS SalesPerson,'' AS VehicleInStockDate,'' AS VehicleOutStockDate," +
                    "RETAIL.VSLSBZ AS Remark,RETAIL.DMJFSZ AS UsePoint,CHAR(RETAIL.VSZFJE) AS PointAmount,RETAIL.VSPSLS AS PosFlowNo,RETAIL.VSLPCX AS Prize," +
                    "EXTEND.VSSBCL AS ThreeGuarantees,EXTEND.XSCPBZ AS LicenseRemark,EXTEND.XSLSCP AS LicenseNo,EXTEND.VSSFWS AS OutSource";
                tableName = "SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND RETAIL.VSLSDD=EXTEND.VSLSDD " +
                    "AND RETAIL.VSLSDD=@ORDER_CODE";
            }
            string sql = DB2Helper.GetSql(selectFields, tableName, condition);

            return baseDao.FindByid(sql, "ORDER_NO", orderNo);
        }
        public DealerStockVehicleInfo doFindDealerStockVehicleInfoById(string vin)
        {
            BaseDao<DealerStockVehicleInfo> baseDao = DaoFactory<DealerStockVehicleInfo>.CreateBaseDao(typeof(DealerStockVehicleInfo));
            string sql = "SELECT CYVINM AS Vin,XSFDJH AS EngineNo,XSDPHM AS ChassisNo,ZMKHDM AS DealerCode,VTCLZT AS VehicleStatus,VTCLSX AS VehicleProperties," +
                "GET_DATE_STRING(VTRKRQ) AS VehicleInStockDate,GET_DATE_STRING(VTOKRQ) AS VehicleOutStockDate FROM SJVDTALIB.ITM02 DEALER_VEHICLE WHERE ZMDWDM='08' AND CYVINM=@VIN";
            return baseDao.FindByid(sql, "VIN", vin);
        }
        public byte[] ExportRetailOrderList(NameValueCollection queryString)
        {
            List<RetailOrderQueryExportVo> list = doFindRetailOrderByCondition(queryString);

            foreach (var item in list)
            {
                IDictionary<string, string> regionDic = CommonCache.GetRegionDic();
                if (item.Region != null && regionDic.ContainsKey(item.Region))
                {
                    item.Region = regionDic[item.Region];
                }
                IDictionary<string, Dealer> dealerDic = SystemCache.GetDealerDic();
                if (item.DealerCode != null && dealerDic.ContainsKey(item.DealerCode))
                {
                    item.DealerName = dealerDic[item.DealerCode].DealerName;
                    item.NetCode = dealerDic[item.DealerCode].NetCode;
                }
                IDictionary<string, string> seriesDic = ProductCache.GetSeriesDic();
                if (item.Series != null && seriesDic.ContainsKey(item.Series))
                {
                    item.Series = seriesDic[item.Series];
                }
                IDictionary<string, ModelGroup> modelGroupDic = ProductCache.GetModelGroupDic();
                string key = item.ModelCode + item.InteriorCode + item.PrList;
                if (modelGroupDic.ContainsKey(key))
                {
                    item.ModelYear = modelGroupDic[key].ModelYear.ToString();
                    item.ModelVersion = modelGroupDic[key].ModelVersion;
                    item.PrList = modelGroupDic[key].PrList;
                }
                IDictionary<string, Model> modelDic = ProductCache.GetModelDic();
                if (item.ModelCode != null && modelDic.ContainsKey(item.ModelCode))
                {
                    item.ModelName = modelDic[item.ModelCode].ModelName;
                }
                IDictionary<string, string> colorDic = ProductCache.GetColorDic();
                if (item.ColorCode != null && colorDic.ContainsKey(item.ColorCode))
                {
                    item.ColorName = colorDic[item.ColorCode];
                }
                IDictionary<string, string> interiorDic = ProductCache.GetInteriorDic();
                if (item.InteriorCode != null && interiorDic.ContainsKey(item.InteriorCode))
                {
                    item.InteriorName = interiorDic[item.InteriorCode];
                }
                IDictionary<string, County> countyDic = CommonCache.GetCountyDic();
                if (item.County != null && countyDic.ContainsKey(item.County))
                {
                    item.Provice = countyDic[item.County].ProvinceName;
                    item.City = countyDic[item.County].CityName;
                    item.County = countyDic[item.County].CountyName;
                }
                IDictionary<string, string> salesPersonDic = RetailCache.GetSalesPersonDic();
                if (item.SalesPerson != null && salesPersonDic.ContainsKey(item.SalesPerson))
                {
                    item.SalesPerson = salesPersonDic[item.SalesPerson];
                }
                IDictionary<string, string> insuranceCompanyDic = RetailCache.GetInsuranceCompanyDic();
                if (item.InsuranceCompany != null && insuranceCompanyDic.ContainsKey(item.InsuranceCompany))
                {
                    item.InsuranceCompany = insuranceCompanyDic[item.InsuranceCompany];
                }
                item.Gender = RetailCache.GetGender(item.Gender);
                item.InvoiceType = RetailCache.GetInvoiceType(item.InvoiceType);
                item.CustomerType = RetailCache.GetCustomerType(item.CustomerType);
                item.VehiclePurpose = RetailCache.GetVehiclePurpose(item.VehiclePurpose);
                item.CertificateType = RetailCache.GetVehiclePurpose(item.CertificateType);
                item.Duty = RetailCache.GetVehiclePurpose(item.Duty);
                item.CompanyProperty = RetailCache.GetVehiclePurpose(item.CompanyProperty);
                item.CompanyIndustry = RetailCache.GetVehiclePurpose(item.CompanyIndustry);
                item.SalesType = RetailCache.GetVehiclePurpose(item.SalesType);
                item.SalesSource = RetailCache.GetSalesSource(item.SalesSource);
                item.OutStockStatus = RetailCache.GetOutStockStatus(item.OutStockStatus);
                item.OrderStatus = RetailCache.GetOrderStatus(item.OrderStatus);
                item.Accessory = RetailCache.GetYesNo(item.Accessory);
                item.PaymentType = RetailCache.GetPaymentType(item.PaymentType);
                item.UsedCar = RetailCache.GetYesNo(item.UsedCar);
                item.LongStorage = RetailCache.GetYesNo(item.LongStorage);
                item.Club = RetailCache.GetYesNo(item.Club);
                item.ThreeGuarantees = RetailCache.GetYesNo(item.ThreeGuarantees);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("零售订单号码,分销中心,经销商代码,经销商名称,网络代码,登记日期,登记时间,零售日期,零售时间,发票号码,车辆代码,车辆名称,");
            sb.Append("颜色代码,颜色名称,内饰,内饰名称,车型年,版本号,选装包,销售类型,车型分类,零售单价,用户名称,联系人,性别,省市,地极市,县,");
            sb.Append("联系地址,邮政编码,车辆用途,发票类型,客户类别,公司性质,职务,电话号码,手机号码,电子邮件,证件类型,证件号码,从事行业,");
            sb.Append("销售员,销售来源,配车处理,操作日期,订单状态,发动机号,VIN,零售数量,作废日期,出库日期,备注,礼品类型,发票日期,");
            sb.Append("是否装配原装附件包,车主,被保险人,保险公司,保险单,保险起始日期,保险终止日期,支付方式,金融机构,贷款类型,支付备注,");
            sb.Append("二手车置换,长库龄车,加入会员俱乐部,三包车辆,网外二网名称");
            sb.AppendLine();
            foreach (var item in list) {
                sb.Append(item.OrderNo + ",");
                sb.Append(item.Region + ",");
                sb.Append(item.DealerCode + ",");
                sb.Append(item.DealerName + ",");
                sb.Append(item.NetCode + ",");
                sb.Append(item.CreateDate + ",");
                sb.Append(item.CreateTime + ",");
                sb.Append(item.RetailDate + ",");
                sb.Append(item.RetailTime + ",");
                sb.Append(item.InvoiceNo + ",");
                sb.Append(item.ModelCode + ",");
                sb.Append(item.ModelName + ",");
                sb.Append(item.ColorCode + ",");
                sb.Append(item.ColorName + ",");
                sb.Append(item.InteriorCode + ",");
                sb.Append(item.InteriorName + ",");
                sb.Append(item.ModelYear + ",");
                sb.Append(item.ModelVersion + ",");
                sb.Append(item.PrList + ",");
                sb.Append(item.SalesType + ",");
                sb.Append(item.Series + ",");
                sb.Append(item.Price + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.CustomerName) + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Contact) + ",");
                sb.Append(item.Gender + ",");
                sb.Append(item.Provice + ",");
                sb.Append(item.City + ",");
                sb.Append(item.County + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Address) + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.ZipCode) + ",");
                sb.Append(item.VehiclePurpose + ",");
                sb.Append(item.InvoiceType + ",");
                sb.Append(item.CustomerType + ",");
                sb.Append(item.CompanyProperty + ",");
                sb.Append(item.Duty + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Telephone) + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Mobile) + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Email) + ",");
                sb.Append(item.CertificateType + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.CertificateNo) + ",");
                sb.Append(item.CompanyIndustry + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.SalesPerson) + ",");
                sb.Append(item.SalesSource + ",");
                sb.Append(item.OutStockStatus + ",");
                sb.Append(item.MatchedDate + ",");
                sb.Append(item.OrderStatus + ",");
                sb.Append(item.EngineNo + ",");
                sb.Append(item.Vin + ",");
                sb.Append(item.Quantity + ",");
                sb.Append(item.DeleteDate + ",");
                sb.Append(item.OutStockDate + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Remark) + ",");
                sb.Append(item.Prize + ",");
                sb.Append(item.InvoiceDate + ",");
                sb.Append(item.Accessory + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Owner) + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.Insured) + ",");
                sb.Append(item.InsuranceCompany + ",");
                sb.Append(item.InsuranceBillNo + ",");
                sb.Append(item.InsuranceBeginDate + ",");
                sb.Append(item.InsuranceEndDate + ",");
                sb.Append(item.PaymentType + ",");
                sb.Append(item.FinancialInstitution + ",");
                sb.Append(item.LoanType + ",");
                sb.Append(FileHelper.FilterSpecialChar(item.PaymentRemark) + ",");
                sb.Append(item.UsedCar + ",");
                sb.Append(item.LongStorage + ",");
                sb.Append(item.Club + ",");
                sb.Append(item.ThreeGuarantees + ",");
                sb.Append(item.OutSource + ",");
                sb.AppendLine();
            }
            return FileHelper.CsvFormat(sb);
        }
        public List<RetailOrderQueryExportVo> doFindRetailOrderByCondition(NameValueCollection queryString)
        {
            BaseDao<RetailOrderQueryExportVo> baseDao = DaoFactory<RetailOrderQueryExportVo>.CreateBaseDao(typeof(RetailOrderQueryExportVo));
            Thread.Sleep(5000);
            string selectFields = string.Empty;
            string tableName = string.Empty;
            string condition = string.Empty;
            if (!string.IsNullOrWhiteSpace(queryString["remarkFlag"]) && "Y".Equals(queryString["remarkFlag"]))
            {
                selectFields = "RETAIL.VSLSDD AS OrderNo,RETAIL.ZMFXDM AS Region,RETAIL.ZMKHDM AS DealerCode,'' AS DealerName,'' AS NetCode," +
                    "GET_DATE_STRING(RETAIL.VSDJRQ) AS CreateDate,GET_TIME_STRING(RETAIL.VSDJRQ) AS CreateTime," +
                    "GET_DATE_STRING(RETAIL.VSLSRQ) AS RetailDate,GET_TIME_STRING(RETAIL.VSLSSJ) AS RetailTime," +
                    "RETAIL.DMFPHM AS InvoiceNo,RETAIL.BSCLDM AS ModelCode,'' AS ModelName,RETAIL.XSYSDM AS ColorCode,'' AS ColorName," +
                    "RETAIL.BSNSYM AS InteriorCode,'' AS InteriorName,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSPNXH AS PrList,RETAIL.VSLSSX AS SalesType,'' AS Series," +
                    "CHAR(RETAIL.VSLSDJ) AS Price,REMARK.VSYHMC AS CustomerName,REMARK.DMLXRM AS Contact,REMARK.VSRYXB AS Gender,'' AS Provice,'' AS City,RETAIL.VSXJDM AS County," +
                    "REMARK.VSLXDZ AS Address,REMARK.DMYZBM AS ZipCode,RETAIL.DMCLYT AS VehiclePurpose,REMARK.DMFPLX AS InvoiceType,RETAIL.DMKHLB AS CustomerType,RETAIL.DMGSXZ AS CompanyProperty," +
                    "RETAIL.DMZWZW AS Duty,REMARK.DMDHHM AS Telephone,REMARK.DMSJHM AS Mobile,REMARK.VSDJYJ AS Email,REMARK.VTZJLX AS CertificateType,REMARK.VTZJHM AS CertificateNo," +
                    "RETAIL.DMCSHY AS CompanyIndustry,RETAIL.VSXSRY AS SalesPerson,RETAIL.VSXSLY AS SalesSource,RETAIL.VSPCCL AS OutStockStatus,'' AS MatchedDate,RETAIL.VSDDZT AS OrderStatus," +
                    "RETAIL.XSFDJH AS EngineNo,RETAIL.CYVINM AS Vin,CHAR(RETAIL.VSLSSL) AS Quantity,GET_DATE_STRING(RETAIL.VSZFRQ) AS DeleteDate,GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate," +
                    "RETAIL.VSLSBZ AS Remark,RETAIL.VSLPCX AS Prize,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate,RETAIL.VSSFFJ AS Accessory," +
                    "EXTEND1.VSYHMC AS Owner,EXTEND1.VSBBXR AS Insured,EXTEND1.VSGSDM AS InsuranceCompany,EXTEND1.SNSYBH AS InsuranceBillNo," +
                    "GET_DATE_STRING(EXTEND1.VSQSRQ) AS InsuranceBeginDate,GET_DATE_STRING(EXTEND1.VSZZRQ) AS InsuranceEndDate," +
                    "EXTEND.ZWZFDM AS PaymentType,EXTEND.ZWJJDM AS FinancialInstitution,EXTEND.ZWDLDM AS LoanType,EXTEND.ZWZFBZ AS PaymentRemark," +
                    "EXTEND.VSRSZH AS UsedCar,EXTEND.VSCKLC AS LongStorage,EXTEND.VSJLBZ AS Club,EXTEND.VSSBCL AS ThreeGuarantees,EXTEND.VSSFWS AS OutSource";
                tableName = "SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND,SJVDTALIB.VST16 EXTEND1,SJVDTALIB.VST19 REMARK";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND EXTEND1.ZMDWDM='08' AND REMARK.ZMDWDM='08' " +
                    "AND RETAIL.VSLSDD=EXTEND.VSLSDD AND RETAIL.VSLSDD=EXTEND1.VSLSDD AND RETAIL.VSLSDD=REMARK.VSLSDD ";
                if (!string.IsNullOrWhiteSpace(queryString["customerName"]))
                {
                    condition += "AND REMARK.VSYHMC LIKE '%" + queryString["customerName"] + "%' ";
                }
            }
            else
            {
                selectFields = "RETAIL.VSLSDD AS OrderNo,RETAIL.ZMFXDM AS Region,RETAIL.ZMKHDM AS DealerCode,'' AS DealerName,'' AS NetCode," +
                    "GET_DATE_STRING(RETAIL.VSDJRQ) AS CreateDate,GET_TIME_STRING(RETAIL.VSDJRQ) AS CreateTime," +
                    "GET_DATE_STRING(RETAIL.VSLSRQ) AS RetailDate,GET_TIME_STRING(RETAIL.VSLSSJ) AS RetailTime," +
                    "RETAIL.DMFPHM AS InvoiceNo,RETAIL.BSCLDM AS ModelCode,'' AS ModelName,RETAIL.XSYSDM AS ColorCode,'' AS ColorName," +
                    "RETAIL.BSNSYM AS InteriorCode,'' AS InteriorName,'' AS ModelYear,'' AS ModelVersion,RETAIL.BSPNXH AS PrList,RETAIL.VSLSSX AS SalesType,'' AS Series," +
                    "CHAR(RETAIL.VSLSDJ) AS Price,RETAIL.VSYHMC AS CustomerName,RETAIL.DMLXRM AS Contact,RETAIL.VSRYXB AS Gender,'' AS Provice,'' AS City,RETAIL.VSXJDM AS County," +
                    "RETAIL.VSLXDZ AS Address,RETAIL.DMYZBM AS ZipCode,RETAIL.DMCLYT AS VehiclePurpose,RETAIL.DMFPLX AS InvoiceType,RETAIL.DMKHLB AS CustomerType,RETAIL.DMGSXZ AS CompanyProperty," +
                    "RETAIL.DMZWZW AS Duty,RETAIL.DMDHHM AS Telephone,RETAIL.DMSJHM AS Mobile,RETAIL.VSDJYJ AS Email,RETAIL.VTZJLX AS CertificateType,RETAIL.VTZJHM AS CertificateNo," +
                    "RETAIL.DMCSHY AS CompanyIndustry,RETAIL.VSXSRY AS SalesPerson,RETAIL.VSXSLY AS SalesSource,RETAIL.VSPCCL AS OutStockStatus,'' AS MatchedDate,RETAIL.VSDDZT AS OrderStatus," +
                    "RETAIL.XSFDJH AS EngineNo,RETAIL.CYVINM AS Vin,CHAR(RETAIL.VSLSSL) AS Quantity,GET_DATE_STRING(RETAIL.VSZFRQ) AS DeleteDate,GET_DATE_STRING(RETAIL.VSLCRQ) AS OutStockDate," +
                    "RETAIL.VSLSBZ AS Remark,RETAIL.VSLPCX AS Prize,GET_DATE_STRING(RETAIL.DMFPRQ) AS InvoiceDate,RETAIL.VSSFFJ AS Accessory," +
                    "EXTEND1.VSYHMC AS Owner,EXTEND1.VSBBXR AS Insured,EXTEND1.VSGSDM AS InsuranceCompany,EXTEND1.SNSYBH AS InsuranceBillNo," +
                    "GET_DATE_STRING(EXTEND1.VSQSRQ) AS InsuranceBeginDate,GET_DATE_STRING(EXTEND1.VSZZRQ) AS InsuranceEndDate," +
                    "EXTEND.ZWZFDM AS PaymentType,EXTEND.ZWJJDM AS FinancialInstitution,EXTEND.ZWDLDM AS LoanType,EXTEND.ZWZFBZ AS PaymentRemark," +
                    "EXTEND.VSRSZH AS UsedCar,EXTEND.VSCKLC AS LongStorage,EXTEND.VSJLBZ AS Club,EXTEND.VSSBCL AS ThreeGuarantees,EXTEND.VSSFWS AS OutSource";
                tableName = "SJVDTALIB.IST15 RETAIL,SJVDTALIB.VST17 EXTEND,SJVDTALIB.VST16 EXTEND1";
                condition = "AND RETAIL.ZMDWDM='08' AND EXTEND.ZMDWDM='08' AND EXTEND1.ZMDWDM='08' AND RETAIL.VSLSDD=EXTEND.VSLSDD AND RETAIL.VSLSDD=EXTEND1.VSLSDD ";
                if (!string.IsNullOrWhiteSpace(queryString["customerName"]))
                {
                    condition += "AND RETAIL.VSYHMC LIKE '%" + queryString["customerName"] + "%' ";
                }
            }
            string orderFields = "RETAIL.VSLSDD";
            Dictionary<string, object> parms = new Dictionary<string, object>();
            parms.Add("DEALER_CODE", queryString["dealerCode"]);
            parms.Add("REGION_CODE", "9999999");
            if (!string.IsNullOrWhiteSpace(queryString["modelCode"]))
            {
                condition += "AND RETAIL.BSCLDM=@MODEL_CODE ";
                parms.Add("MODEL_CODE", queryString["modelCode"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["colorCode"]))
            {
                condition += "AND RETAIL.XSYSDM=@COLOR_CODE ";
                parms.Add("COLOR_CODE", queryString["colorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["interiorCode"]))
            {
                condition += "AND RETAIL.BSNSYM=@INTERIOR_CODE ";
                parms.Add("INTERIOR_CODE", queryString["interiorCode"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["vin"]))
            {
                condition += "AND RETAIL.CYVINM=@VIN ";
                parms.Add("VIN", queryString["vin"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["invoiceNo"]))
            {
                condition += "AND RETAIL.DMFPHM=@INVOICE_NO ";
                parms.Add("INVOICE_NO", queryString["invoiceNo"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["salesSource"]))
            {
                condition += "AND RETAIL.VSXSLY=@SALES_SOURCE ";
                parms.Add("SALES_SOURCE", queryString["salesSource"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["orderStatus"]))
            {
                condition += "AND RETAIL.VSDDZT=@ORDER_STATUS ";
                parms.Add("ORDER_STATUS", queryString["orderStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["outStockStatus"]))
            {
                condition += "AND VSPCCL=@OUTSTOCK_STATUS ";
                parms.Add("OUTSTOCK_STATUS", queryString["outStockStatus"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["startDateCreate"]))
            {
                condition += "AND RETAIL.VSDJRQ>=@START_DATE_CREATE ";
                parms.Add("START_DATE_CREATE", long.Parse(queryString["startDateCreate"].ToString().Replace("-", "")));
            }
            if (!string.IsNullOrWhiteSpace(queryString["endDateCreate"]))
            {
                condition += "AND RETAIL.VSDJRQ<=@END_DATE_CREATE ";
                parms.Add("END_DATE_CREATE", long.Parse(queryString["endDateCreate"].ToString().Replace("-", "")));
            }
            string sql = DB2Helper.GetSql(selectFields, tableName, condition,orderFields);

            return baseDao.nativeQuerySql(sql, parms);
        }
    }
}