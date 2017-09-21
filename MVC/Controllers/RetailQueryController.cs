using MVC.Models.Retail;
using MVC.Models.System;
using MVC.Services.Retail;
using MVC.Services.Retail.Impl;
using MVC.Services.Retail.Vo;
using MVC.Services.System;
using MVC.Services.System.Impl;
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
        protected static log4net.ILog log = log4net.LogManager.GetLogger(typeof(RetailQueryController));
        private static readonly RetailQueryService retailQueryService = new RetailQueryServiceImpl();
        private static readonly RetailOrderService retailOrderService = new RetailOrderServiceImpl();
        private static readonly DealerUserService dealerUserService = new DealerUserServiceImpl();
        // GET: RetaiQuery
        public ActionResult RetailOrderQuery(string AHDCZXM,string AEBPSWD,string ALWEBQRYID)
        {
            if (string.IsNullOrWhiteSpace(ALWEBQRYID))
            {
                ViewData["errorMessage"] = "系统错误";
                log.Error(ViewData["errorMessage"] + Request.Form.ToString());
                return View("Error");
            }
            if (!AEBPSWD.Equals("PPPPPP"))
            {
                ViewData["errorMessage"] = "密码错误";
                log.Error(ViewData["errorMessage"] + Request.Form.ToString());
                return View("Error");
            }
            DealerUser dealerUser= dealerUserService.doGetDealerUserInfo(AHDCZXM);
            if (dealerUser==null)
            {
                ViewData["errorMessage"] = "经销商账号无效";
                log.Error(ViewData["errorMessage"] + Request.Form.ToString());
                return View("Error");
            }

            Session["username"] = dealerUser.Username.Trim(); ;
            Session["dealerCode"] = dealerUser.DealerCode.Trim();

            ViewData["username"] = dealerUser.Username.Trim();
            ViewData["dealerCode"] = dealerUser.DealerCode.Trim();
            ViewData["regionCode"] = dealerUser.RegionCode.Trim();
            ViewData["startIndex"] = 0;
            ViewData["pageSize"] = 5;
            log.Info("进入查询界面" + Request.Form.ToString());
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
            if(string.IsNullOrWhiteSpace(Session["username"].ToString())|| 
                string.IsNullOrWhiteSpace(Session["dealerCode"].ToString()))
            {
                ViewData["errorMessage"] = "登录超时";
                log.Error(ViewData["errorMessage"] + Request.Form.ToString());
                return View("Error");
            }
            if ((!Request.QueryString["username"].Equals(Session["username"])) || 
                (!Request.QueryString["dealerCode"].Equals(Session["dealerCode"])))
            {
                ViewData["errorMessage"] = "登录信息错误";
                log.Error(ViewData["errorMessage"] + Request.Form.ToString());
                return View("Error");
            }
            if (Request.QueryString["returnFlag"] !=null)
            {
                ViewData["pageResult"] = Session["pageResult"];
            }
            else {
                //Thread.Sleep(1000);
                QueryCriteria queryCriteria = new QueryCriteria(Request.QueryString);
                PageResult<RetailOrderQueryVo> pageResult = retailQueryService.QueryRetailOrderList(queryCriteria);
                ViewData["pageResult"] = pageResult;
                Session.Add("pageResult", pageResult);  
            }
            log.Info("查询数据" + Request.QueryString.ToString());
            return View("RetailOrderQueryList");
        }

        public ActionResult RetailOrderQueryDetails(string orderNo)
        {
            RetailOrder retailOrder = retailOrderService.Details(orderNo);
            ViewData["retailOrder"] = retailOrder;
            log.Info("查询明细数据" + Request.Form.ToString());
            return View("RetailOrderQueryDetails");
        }

        public ActionResult RetailOrderQueryExport()
        {
            //List<RetailOrderQueryVo> list = retailQueryService.doFindRetailOrderByCondition(Request.QueryString);
            byte[] result = retailQueryService.ExportRetailOrderList(Request.QueryString);
            log.Info("导出数据" + Request.QueryString.ToString());
            return File(result, "text/csv", "RetailOrderQueryExport.csv");
        }
    }
}