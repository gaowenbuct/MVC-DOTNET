using MVC.Models.Product;
using MVC.Services.Product;
using MVC.Services.Product.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Cache
{
    public class ProductCache
    {
        private static ProductService productService = new ProductServiceImpl();
        private static IDictionary<string, string> seriesDic;
        private static IDictionary<string, Model> modelDic;
        private static IDictionary<string, string> colorDic;
        private static IDictionary<string, string> interiorDic;
        private static IDictionary<string, ModelGroup> modelGroupDic;
        public static IDictionary<string, string> GetSeriesDic()
        {
            if (seriesDic == null)
            {
                List<Series> list = productService.doFindSeriesAll();
                seriesDic = list.ToDictionary(x => x.SeriesCode, x => x.SeriesName);
            }
            return seriesDic;
        }
        public static IDictionary<string, Model> GetModelDic()
        {
            if (modelDic == null)
            {
                List<Model> list = productService.doFindModelAll();
                modelDic = list.ToDictionary(x => x.ModelCode, x => x);
            }
            return modelDic;
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
                modelGroupDic = list.ToDictionary(x => x.ModelCode + x.InteriorCode + x.ModelPrNo, x => x);
            }
            return modelGroupDic;
        }
    }
}