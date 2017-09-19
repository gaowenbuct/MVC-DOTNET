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
            List<Series> seriesList = productService.doFindAllSeries();
            ViewData["modelCode"] = modelCode;
            ViewData["seriesList"] = seriesList;
            return View("ModelQuery");
        }
        public JsonResult ModelQuerySearch(string seriesCode,string modelCode)
        {
            List<Model> modelList = productService.doFindModel(seriesCode,modelCode);
            ViewData["modelList"] = modelList;
            //return View("ModelQueryList");
            //return modelList;
            return Json(modelList);
        }
        public JsonResult GetModelInfo(string modelCode)
        {
            Model model = productService.doGetModelInfo(modelCode);
            return Json(model);
        }
        public ActionResult ColorQuery(string modelCode,string color)
        {
            ViewData["modelCode"] = modelCode;
            ViewData["color"] = color;
            return View("ColorQuery");
        }
        public ActionResult InteriorQuery(string modelCode,string interior)
        {
            ViewData["modelCode"] = modelCode;
            ViewData["interior"] = interior;
            return View("InteriorQuery");
        }
        public JsonResult ColorQuerySearch(string modelCode, string color)
        {
            List<Color> colorList = productService.doFindColor(modelCode,color);
            ViewData["colorList"] = colorList;
            return Json(colorList);
        }
        public JsonResult InteriorQuerySearch(string modelCode, string interior)
        {
            List<Interior> interiorList = productService.doFindInterior(modelCode, interior);
            ViewData["interiorList"] = interiorList;
            return Json(interiorList);
        }
    }
}