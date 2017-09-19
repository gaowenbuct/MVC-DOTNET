using MVC.Models.System;
using MVC.Services.System;
using MVC.Services.System.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        private UserService userService = new UserServiceImpl();
        private UserApiService userApiService = new UserApiServiceImpl();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult gotoLogin()
        {
            return View("Login");
        }
        public ActionResult gotoCreate()
        {
            return View("create");
        }
        [HttpPost]
        public ActionResult checkLogin(User user)
        {
            if ((user.username.Equals("admin")&&user.password.Equals("admin")
                ||userService.doCheckLogin(user.username,user.password))){ 
                ViewData["user"] = user.username;
                return View("Welcome");
            }
            else
            {
                return View("Error");
            }  
        }
        public ActionResult list()
        {
            List<User> list = userService.doFindAll();
            ViewData["list"] = list;
            return View("List");
        }
        public ActionResult details(string username)
        {
            userService.doGet(username);
            return RedirectToAction("list");
        }
        public ActionResult delete(string username)
        {
            userService.doDelete(username);
            return RedirectToAction("list");
        }
        public ActionResult create(User user)
        {
            userService.doCreate(user);
            return RedirectToAction("list");
        }
        public ActionResult update(User user)
        {
            userService.doUpdate(user);
            return RedirectToAction("list");
        }
        public ActionResult listBoth()
        {
            List<User> list = userService.doFindAll();
            List<User> listRemote = userApiService.doFindAll();
            ViewData["list"] = list;
            ViewData["listRemote"] = listRemote;
            return View("List");
        }
        public ActionResult detailsBoth(string username)
        {
            userService.doGet(username);
            userApiService.doGet(username);
            return RedirectToAction("list");
        }
        public ActionResult deleteBoth(string username)
        {
            userService.doDelete(username);
            userApiService.doDelete(username);
            return RedirectToAction("list");
        }
        public ActionResult createBoth(User user)
        {
            userService.doCreate(user);
            userApiService.doCreate(user);
            return RedirectToAction("list");
        }
        public ActionResult updateBoth(User user)
        {
            userService.doUpdate(user);
            userApiService.doUpdate(user);
            return RedirectToAction("list");
        }
    }
}