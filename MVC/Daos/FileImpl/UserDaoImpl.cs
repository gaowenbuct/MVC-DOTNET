using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.System;
using MVC.Utils;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MVC.Daos.FileImpl
{
    public class UserDaoImpl : BaseDaoImpl<User>, UserDao
    {
        private string tableName = "USER";
        public User get(string username)
        {
            Trace.Assert(!string.IsNullOrEmpty(username));
            List<User> list;
            User result = null;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameByTable(tableName), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                result=list.First<User>(p => p.username == username);
            }
            return result;
        }
        public void create(User user)
        {
            Trace.Assert(user!=null&&(!string.IsNullOrEmpty(user.username)) && (!string.IsNullOrEmpty(user.password)));
            List<User> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameByTable(tableName), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                if(list.FirstOrDefault<User>(p => p.username == user.username) == null)
                {
                    list.Add(user);
                    using ( StreamWriter sw=new StreamWriter(FileHelper.GetFileNameByTable(tableName),false, Encoding.UTF8)){
                        sw.Write(JsonConvert.SerializeObject(list));
                    }
                }
                else
                {
                    throw new ArgumentException("Data key exists");
                }
            }
        }
        public void delete(string username)
        {
            Trace.Assert(!string.IsNullOrEmpty(username));
            List<User> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameByTable(tableName), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                list.RemoveAll(p => p.username == username);
                using (StreamWriter sw = new StreamWriter(FileHelper.GetFileNameByTable(tableName), false, Encoding.UTF8))
                {
                    sw.Write(JsonConvert.SerializeObject(list));
                }
            }
        }
        public List<User> findAll()
        {
            List<User> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameByTable(tableName), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                return list.OrderBy(p => p.username).ToList();
            }
            else
            {
                return null;
            }
        }
        public void update(User user)
        {
            Trace.Assert(user != null && (!string.IsNullOrEmpty(user.username)) && (!string.IsNullOrEmpty(user.password)));
            List<User> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameByTable(tableName), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<User>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                if (list.RemoveAll(p => p.username == user.username) == 1)
                {
                    list.Add(user);
                    using (StreamWriter sw = new StreamWriter(FileHelper.GetFileNameByTable(tableName), false, Encoding.UTF8))
                    {
                        sw.Write(JsonConvert.SerializeObject(list));
                    }
                }
                else
                {
                    throw new ArgumentException("Data key not exists");
                }
            }
        }
    }
}