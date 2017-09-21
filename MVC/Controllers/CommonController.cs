using MVC.Cache;
using MVC.Models.Common;
using MVC.Services.Common;
using MVC.Services.Common.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class CommonController : Controller
    {
        private ProductService productService = new ProductServiceImpl();
        // GET: Common
        public ActionResult ModelQuery(string modelCode)
        {
            /*List<Series> seriesList = productService.doFindAllSeries();
            ViewData["seriesList"] = seriesList;*/
            ViewData["modelCode"] = modelCode;
            ViewData["seriesList"] = CommonCache.GetSeriesDic();
            return View("ModelQuery");
        }
        public JsonResult ModelQuerySearch(string seriesCode,string modelCode)
        {
            List<Model> modelList = productService.doFindModel(seriesCode,modelCode);
            return Json(modelList);
        }
        public JsonResult GetModelInfo(string modelCode)
        {
            Model model = productService.doGetModelInfo(modelCode);
            return Json(model);
        }
        public ActionResult ColorQuery(string modelCode,string color)
        {
            //ViewData["modelCode"] = modelCode;
            ViewData["color"] = color;
            return View("ColorQuery");
        }
        public ActionResult InteriorQuery(string modelCode,string interior)
        {
            //ViewData["modelCode"] = modelCode;
            ViewData["interior"] = interior;
            return View("InteriorQuery");
        }
        public JsonResult ColorQuerySearch(string colorCode,string colorName)
        {
            IDictionary<string, string> colorList = productService.doFindColorList(colorCode, colorName);
            return Json(colorList);
        }
        public JsonResult InteriorQuerySearch(string interiorCode,string interiorName)
        {
            IDictionary<string,string> interiorList = productService.doFindInteriorList(interiorCode, interiorName);
            return Json(interiorList);
        }
    }
}