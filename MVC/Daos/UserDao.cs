using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Daos
{
    public interface UserDao:BaseDao<User>
    {
        User get(string username);
        void create(User user);
        void delete(string username);
        void update(User user);
        List<User> findAll();
    }
}