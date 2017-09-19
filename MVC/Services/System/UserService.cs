using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.System
{
    public interface UserService:BaseService
    {
        bool doCheckLogin(string username,string password);
        User doGet(string username);
        bool doCreate(User user);
        bool doDelete(string username);
        bool doUpdate(User user);
        List<User> doFindAll();
    }
}