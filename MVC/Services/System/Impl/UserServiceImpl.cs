using MVC.Daos;
using MVC.Daos.OracleImpl;
using MVC.Models.System;
using MVC.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.System.Impl
{
    public class UserServiceImpl : BaseServiceImpl,UserService
    {
        //private UserDao userDao = new UserDaoImpl();
        private static readonly UserDao userDao = DaoFactory<User>.CreateUserDao();
        public bool doCheckLogin(string username,string password)
        {
            return userDao.get(username) != null
                && userDao.get(username).password.Equals(password) ? true : false;
        }
        public User doGet(string username)
        {
            return userDao.get(username);
        }

        public bool doCreate(User user)
        {
            getLogger(this).Info("doCreate");
            userDao.create(user);
            return true;
        }

        public bool doDelete(string username)
        {
            userDao.delete(username);
            return true;
        }

        public bool doUpdate(User user)
        {
            userDao.update(user);
            return true;
        }

        public List<User> doFindAll()
        {
            return userDao.findAll();
        }
    }
}