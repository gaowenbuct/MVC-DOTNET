using MVC.Models.Retail;
using MVC.Services.Retail;
using MVC.Services.Retail.Impl;
using MVC.Services.Retail.Vo;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVC.Controllers
{
    public class RetailQueryController : Controller
    {
        private RetailQueryService retailQueryService = new RetailQueryServiceImpl();
        private RetailOrderService retailOrderService = new RetailOrderServiceImpl();
        // GET: RetaiQuery
        public ActionResult RetailOrderQuery()
        {
            return View("RetailOrderQuery");
        }
        /*public JsonResult RetailOrderQueryList()
        {
            QueryCriteria queryCriteria = new QueryCriteria();
            queryCriteria.QueryCondition = Request.QueryString;
            PageResult<RetailOrderQueryVo> result= retailQueryService.doFindRetailOrderByCondition(queryCriteria);
            return Json(result,JsonRequestBehavior.AllowGet);
        }*/
        public ActionResult RetailOrderQueryList()
        {
            if (Request.QueryString["returnFlag"] !=null)
            {
                ViewData["pageResult"] = Session["pageResult"];
            }
            else {
                //Thread.Sleep(1000);
                QueryCriteria queryCriteria = new QueryCriteria(Request.QueryString);
                PageResult<RetailOrderQueryVo> pageResult = retailQueryService.doFindRetailOrderByCondition(queryCriteria);
                ViewData["pageResult"] = pageResult;
                Session.Add("pageResult", pageResult);
            }
            return View("RetailOrderQueryList");
        }

        public ActionResult RetailOrderQueryDetails(string orderNo)
        {
            RetailOrder retailOrder = retailOrderService.Details(orderNo);
            ViewData["retailOrder"] = retailOrder;
            return View("RetailOrderQueryDetails");
        }

        public ActionResult RetailOrderQueryExport()
        {
            List<RetailOrderQueryVo> list = retailQueryService.doFindRetailOrderByCondition(Request.QueryString);
            byte[] result = retailQueryService.doExportRetailOrder(list);
            return File(result, "text/csv", "RetailOrderQueryExport.csv");
        }
    }
}