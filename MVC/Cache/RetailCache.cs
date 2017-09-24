using MVC.Models.Retail;
using MVC.Services.Retail.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Cache
{
    public class RetailCache
    {
        private static RetailService retailService = new RetailServiceImpl();
        public static string GetYesNo(string code)
        {
            if (code == null) return string.Empty;
            if (code.Equals("Y"))
            {
                return "是";
            }
            else if (code.Equals("N"))
            {
                return "否";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetOrderStatus(string code)
        {
            if (code == null) return string.Empty;
            if (code.Equals("Y"))
            {
                return "有效";
            }
            else if (code.Equals("S"))
            {
                return "作废";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetOutStockStatus(string code)
        {
            if (code == null) return string.Empty;
            if (code.Equals("Y"))
            {
                return "已出库";
            }
            else if (code.Equals("N"))
            {
                return "未出库";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetSalesSource(string code)//ISM39
        {
            switch (code)
            {
                case "A":
                    return "录入零售";
                case "B":
                    return "提单零售";
                case "C":
                    return "意向订单零售";
                default:
                    return string.Empty;
            }
        }
        public static string GetInvoiceType(string code)//ISM13
        {
            if (code == null) return string.Empty;
            if (code.Equals("A"))
            {
                return "一般发票";
            }
            else if (code.Equals("B"))
            {
                return "增值税";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetGender(string code)
        {
            if (code == null) return string.Empty;
            if (code.Equals("A"))
            {
                return "男";
            }
            else if (code.Equals("B"))
            {
                return "女";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetCustomerType(string code)//ISM15
        {
            if (code == null) return string.Empty;
            if (code.Equals("A"))
            {
                return "个人";
            }
            else if (code.Equals("B"))
            {
                return "公司";
            }
            else if (code.Equals("C"))
            {
                return "经销商";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetVehiclePurpose(string code)//ISM11
        {
            if (code == null) return string.Empty;
            switch (code)
            {
                case "01":
                    return "没有回答";
                case "02":
                    return "交通车";
                case "03":
                    return "客运车";
                case "04":
                    return "私人车";
                case "05":
                    return "货运车";
                case "06":
                    return "公司车";
                case "07":
                    return "客货两用车";
                case "08":
                    return "家庭商务用车";
                default:
                    return string.Empty;
            }
        }
        public static string GetCertificateType(string code)//VSM09
        {
            if (code == null) return string.Empty;
            switch (code)
            {
                case "A":
                    return "身份证";
                case "B":
                    return "军官证";
                case "C":
                    return "护照";
                case "D":
                    return "其他";
                default:
                    return string.Empty;
            }
        }
        public static string GetDuty(string code)//ISM17
        {
            if (code == null) return string.Empty;
            switch (code)
            {
                case "01":
                    return "没有回答";
                case "02":
                    return "高层管理人员";
                case "03":
                    return "中层管理人员";
                case "04":
                    return "基层管理人员";
                case "05":
                    return "一般职员";
                case "06":
                    return "私营公司老板/自由职业者/个体户";
                case "07":
                    return "专业人士 (律师/医生等)";
                case "08":
                    return "公务员／管理员／教师／护士／警察／军人";
                case "09":
                    return "其它";
                case "10":
                    return "自由职业者";
                case "11":
                    return "政府/国家事业单位人员";
                case "12":
                    return "国企或私/民营企业经营管理人员";
                case "13":
                    return "外资或合资企业专业技术人员";
                case "14":
                    return "学生、退休、家庭主妇/主男";
                case "15":
                    return "国企或私/民营企业专业技术人员";
                case "16":
                    return "自己或与人合伙开办企业或公司";
                default:
                    return string.Empty;
            }
        }
        public static string GetCompanyProperty(string code)//ISM16
        {
            if (code == null) return string.Empty;
            switch (code)
            {
                case "00":
                    return "没有回答";
                case "01":
                    return "国有";
                case "02":
                    return "集体";
                case "03":
                    return "乡镇";
                case "04":
                    return "三资";
                case "05":
                    return "私营";
                case "06":
                    return "自雇";
                case "07":
                    return "股份制";
                case "08":
                    return "中小型公司";
                case "09":
                    return "有限责任公司";
                case "10":
                    return "非赢利公司";
                case "11":
                    return "跨国公司";
                default:
                    return string.Empty;
            }
        }
        private static IDictionary<string, string> companyIndustryDic;
        public static IDictionary<string, string> GetCompanyIndustryDic()//ISM12
        {
            if (companyIndustryDic == null)
            {
                companyIndustryDic = new Dictionary<string, string>
                {
                    {"01","没有回答"},
                    {"02","经营管理类"},
                    {"03","计算机/IT人员"},
                    {"04","高层管理人员"},
                    {"05","电子/通讯类专业人员"},
                    {"06","中层管理人员"},
                    {"07","销售/业务类"},
                    {"08","基层管理人员"},
                    {"09","市场营销/公关"},
                    {"10","一般职员"},
                    {"11","客户服务类"},
                    {"12","私营公司老板/自由职业者/个体户"},
                    {"13","行政/人力资源类"},
                    {"14","专业人员（律师/医生等）"},
                    {"15","文职类"},
                    {"16","公务员／管理员／教师／护士／警察／军人等" },
                    {"17","翻译类"},
                    {"18","其他"},
                    {"19","自由职业者"},
                    {"20","政府/国家事业单位人员"},
                    {"21","国企或私/民营企业经营管理人员"},
                    {"22","外企或合资企业专业技术人员"},
                    {"23","自己或与人合伙开办企业或公司"},
                    {"24","学生、退休、家庭主妇/主男"},
                    {"25","国企或私/民营企业经营专业技术人员"},
                    {"26","演员、艺术家、文化界或体育界人士"},
                    {"27","咨询/顾问"},
                    {"28","医疗/护理/保健类"},
                    {"29","技工类"},
                    {"30","文教类"},
                    {"31","服务业专业人员"},
                    {"32","其他专业人员"}
                };
            }
            return companyIndustryDic;
        }
        public static string GetSalesType(string code)//ISM14
        {
            if (code == null) return string.Empty;
            if (code.Equals("A"))
            {
                return "正常";
            }
            else if (code.Equals("B"))
            {
                return "转卖";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetUserPoint(string code)
        {
            if (code == null) return string.Empty;
            if (code.Equals("Y"))
            {
                return "使用";
            }
            else if (code.Equals("N"))
            {
                return "未使用";
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetPaymentType(string code)//ZWM36
        {
            if (code == null) return string.Empty;
            if (code.Equals("01"))
            {
                return "全款";
            }
            else if (code.Equals("02"))
            {
                return "贷款";
            }
            else
            {
                return string.Empty;
            }
        }
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
        private static IDictionary<string, County> countyDic;
        private static IDictionary<string, Dealer> dealerDic;
        private static IDictionary<string, string> salesPersonDic;
        private static IDictionary<string, string> insuranceCompanyDic;
        public static IDictionary<string, County> GetCountyDic()//ISM10,09,08
        {
            if (countyDic == null)
            {
                List<County> list = retailService.doFindCountyAll();
                countyDic = list.ToDictionary(x => x.CountyCode, x => x);
            }
            return countyDic;
        }
        public static IDictionary<string, Dealer> GetDealerDic()
        {
            if (dealerDic == null)
            {
                List<Dealer> list = retailService.doFindDealerAll();
                dealerDic = list.ToDictionary(x => x.DealerCode, x => x);
            }
            return dealerDic;
        }
        public static IDictionary<string, string> GetSalesPersonDic()//ISM40
        {
            if (salesPersonDic == null)
            {
                List<SalesPerson> list = retailService.doFindSalesPersonAll();
                salesPersonDic = list.ToDictionary(x => x.DealerCode+x.SalesPersonCode, x => x.SalesPersonName);
            }
            return salesPersonDic;
        }
        public static IDictionary<string, string> GetInsuranceCompanyDic()//VSM29
        {
            if (insuranceCompanyDic == null) 
            {
                insuranceCompanyDic = retailService.doFindInsuranceCompanyAll();
            }
            return insuranceCompanyDic;
        }
    }
}