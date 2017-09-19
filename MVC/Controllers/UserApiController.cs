using MVC.Models.System;
using MVC.Services.System;
using MVC.Services.System.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC.Controllers
{
    public class UserApiController : ApiController
    {
        private UserService userService = new UserServiceImpl();
        public IEnumerable<User> Get()
        {
            return userService.doFindAll();
        }
        
        public User Get(string username)
        {
            return userService.doGet(username);
        }

        public void Post([FromBody]User user)
        {
            userService.doCreate(user);
        }
        
        public void Put([FromBody]User user)
        {
            userService.doUpdate(user);
        }
        
        public void Delete(string username)
        {
            userService.doDelete(username);
        }
    }
}
