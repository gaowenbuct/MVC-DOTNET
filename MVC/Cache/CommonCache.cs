using MVC.Models.Common;
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
        private static ProductService productService = new ProductServiceImpl();
        private static IDictionary<string,string> seriesDic;
        private static IDictionary<string, string> colorDic;
        private static IDictionary<string, string> interiorDic;
        private static IDictionary<string, ModelGroup> modelGroupDic;
        public static IDictionary<string, string> GetSeriesDic()
        {
            if (seriesDic == null)
            {
                List<Series> list = productService.doFindSeriesAll();
                seriesDic=list.ToDictionary(x => x.SeriesCode, x => x.SeriesName);
            }
            return seriesDic;
        }
        public static IDictionary<string, string> GetColorDic()
        {
            if (colorDic == null)
            {
                List<Color> list = productService.doFindColorAll();
                colorDic = list.ToDictionary(x => x.ColorCode, x => x.ColorName);
            }
            return colorDic;
        }
        public static IDictionary<string, string> GetInteriorDic()
        {
            if (interiorDic == null)
            {
                List<Interior> list = productService.doFindInteriorAll();
                interiorDic = list.ToDictionary(x => x.InteriorCode, x => x.InteriorName);
            }
            return interiorDic;
        }
        public static IDictionary<string, ModelGroup> GetModelGroupDic()
        {
            if (modelGroupDic == null)
            {
                List<ModelGroup> list = productService.doFindModelGroupAll();
                modelGroupDic = list.ToDictionary(x => x.ModelCode+x.InteriorCode+x.ModelPrNo, x=>x );
            }
            return modelGroupDic;
        }
        public static string GetSaleSource(string code)
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
        public static string GetOrderStatus(string code)
        {
            if (code.Equals("Y"))
            {
                return "有效";
            }
            else
            {
                return "作废";
            }
        }
        public static string GetAccessory(string code)
        {
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
        public static string GetClub(string code)
        {
            if (code.Equals("Y"))
            {
                return "是";
            }
            else if(code.Equals("N"))
            {
                return "否";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}