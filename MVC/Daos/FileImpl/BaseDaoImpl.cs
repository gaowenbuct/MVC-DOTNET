using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Utils;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MVC.Services.Retail.Vo;
using System.Reflection;
using System.Diagnostics;

namespace MVC.Daos.FileImpl
{
    public class BaseDaoImpl<T> : BaseDao<T>
    {
        //private string fileNameAndPath = "E:\\RetailOrderQueryVo.txt";
        public T FindByid(string sql, string parmName, string parmValue)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(parmName));
            Debug.Assert(!string.IsNullOrWhiteSpace(parmValue));

            T result = default(T);
            List<T> list = null;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameBySql(sql), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                foreach (T item in list)
                {
                    foreach(PropertyInfo pi in item.GetType().GetProperties())
                    {
                        if(FileHelper.PropertyMatch(pi.Name, parmName))
                        {
                            string itemParmValue = item.GetType().GetProperty(pi.Name).GetValue(item, null) as string;
                            if (itemParmValue.Equals(parmValue))
                            {
                                result = item;
                            }
                            break;
                        }
                    }
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            return result;
        }
        public List<T> FindAll(string sql)
        {
            List<T> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameBySql(sql), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
            if (list == null)
            {
                return new List<T>();
            }
            else
            {
                return list;
            } 
        }
        public IDictionary<string,string> FindAllDic(string sql)
        {
            IDictionary<string, string> list;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameBySql(sql), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<IDictionary<string, string>>(sr.ReadToEnd());
            }
            if (list == null)
            {
                return new Dictionary<string, string>();
            }
            else
            {
                return list;
            }
        }
        public PageResult<T> nativeQuerySql(string sql,string sqlCount, IDictionary<string, object> parms, int startIndex, int pageSize)
        {
            Debug.Assert(startIndex>=0);
            Debug.Assert(pageSize >0&& pageSize<=100);
            List<T> list=null;
            List<T> result=null;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameBySql(sqlCount), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
            if (list != null)
            {
                if (startIndex == 0)
                {
                    result = list.Take(pageSize).ToList();
                    //pageRsult = new PageResult<T>(startIndex, pageSize, list.Count, list.Take(pageSize).ToList());
                }
                else if(startIndex< list.Count)
                {
                    result = list.Skip(startIndex).Take(pageSize).ToList();
                    //pageRsult = new PageResult<T>(startIndex, pageSize, list.Count, list.Skip(startIndex).Take(pageSize).ToList());
                }  
            }
            return new PageResult<T>(startIndex, pageSize, list.Count, result);
        }

        public List<T> nativeQuerySql(string sql, IDictionary<string, object> parms)
        {
            List<T> list = null;
            using (StreamReader sr = new StreamReader(FileHelper.GetFileNameBySql(sql), Encoding.UTF8))
            {
                list = JsonConvert.DeserializeObject<List<T>>(sr.ReadToEnd());
            }
            return list;
        }

        public string nativeQuerySqlReturnString(string sql, IDictionary<string, object> parms)
        {
            throw new NotImplementedException();
        }
    }
}