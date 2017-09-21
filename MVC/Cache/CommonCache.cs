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
        public static IDictionary<string, string> GetSeriesDic()
        {
            if (seriesDic == null)
            {
                List<Series> list = productService.doFindSeriesAll();
                seriesDic=list.ToDictionary(series => series.SeriesCode, series => series.SeriesName);
            }
            return seriesDic;
        }
        public static IDictionary<string, string> GetColorDic()
        {
            if (colorDic == null)
            {
                List<Color> list = productService.doFindColorAll();
                colorDic = list.ToDictionary(color => color.ColorCode, color => color.ColorName);
            }
            return colorDic;
        }
        public static IDictionary<string, string> GetInteriorDic()
        {
            if (interiorDic == null)
            {
                List<Interior> list = productService.doFindInteriorAll();
                interiorDic = list.ToDictionary(interior => interior.InteriorCode, interior => interior.InteriorName);
            }
            return interiorDic;
        }
    }
}