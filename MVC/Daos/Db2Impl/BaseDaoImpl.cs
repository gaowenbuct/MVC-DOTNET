using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Utils;
using IBM.Data.DB2.iSeries;
using System.Data;
using System.Reflection;
using System.Diagnostics;

namespace MVC.Daos.Db2Impl
{
    public class BaseDaoImpl<T> : BaseDao<T>
    {
        public T FindByid(string sql, string parmName, string parmValue)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(parmName));
            Debug.Assert(!string.IsNullOrWhiteSpace(parmValue));

            T result = default(T);
            iDB2Parameter[] db2Parms = new iDB2Parameter[] { new iDB2Parameter('@'+parmName, parmValue) };
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
            {
                if (rdr.Read())
                {
                    T t = Activator.CreateInstance<T>();
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                    int i = 0;
                    foreach (PropertyInfo pi in propertyInfo)
                    {
                        pi.SetValue(t, rdr.GetValue(i++));
                    }
                    result= t;
                }
            }
            return result;
        }
        public List<T> FindAll(string sql)
        {
            List<T> list = null;
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql))
            {
                if (rdr.Read())
                {
                    T t = Activator.CreateInstance<T>();
                    PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                    int i = 0;
                    foreach (PropertyInfo pi in propertyInfo)
                    {
                        pi.SetValue(t, rdr.GetValue(i++));
                    }
                    list.Add(t);
                }
            }
            return list;
        }
        public PageResult<T> nativeQuerySQL(string sql,string sqlCount, IDictionary<string, object> parms, int startIndex, int pageSize)
        {
            PageResult<T> pageResult = null;// new PageResult<T>();
            List<T> list = null;
            iDB2Parameter[] db2Parms=null;
            if (parms != null)
            {
                List<iDB2Parameter> paramlist=new List<iDB2Parameter>();
                foreach(var item in parms)
                {
                    Debug.Assert(item.Value!=null);
                    paramlist.Add(new iDB2Parameter(item.Key,item.Value));
                }
                db2Parms = paramlist.ToArray();
            }
            int count=Convert.ToInt32(DB2Helper.ExecuteScalar(DB2Helper.ConnectionString, CommandType.Text, sqlCount, db2Parms));
            if (count > 0)
            {
                using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
                {
                    if (rdr.Read())
                    {
                        list= new List<T>();
                        do
                        {
                            T t = Activator.CreateInstance<T>();
                            PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                            int i = 0;
                            foreach (PropertyInfo pi in propertyInfo)
                            {
                                pi.SetValue(t, rdr.GetValue(i++));
                            }
                            list.Add(t);
                        } while (rdr.Read());
                        pageResult = new PageResult<T>(startIndex,pageSize,count, list);
                    }
                }
            }
            return pageResult;
        }

        public List<T> nativeQuerySQL(string sql, IDictionary<string, object> parms)
        {
            iDB2Parameter[] db2Parms = null;
            List<T> list = null;
            if (parms != null)
            {
                List<iDB2Parameter> parmsList = new List<iDB2Parameter>();
                foreach (var item in parms)
                {
                    Debug.Assert(item.Value != null);
                    parmsList.Add(new iDB2Parameter(item.Key, item.Value));
                }
                db2Parms = parmsList.ToArray();
            }
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
            {
                if (rdr.Read())
                {
                    list = new List<T>();
                    do
                    {
                        T t = Activator.CreateInstance<T>();
                        PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                        int i = 0;
                        foreach (PropertyInfo pi in propertyInfo)
                        {
                            pi.SetValue(t, rdr.GetValue(i++));
                        }
                        list.Add(t);
                    } while (rdr.Read());
                }
            }
            return list;
        }
    }
}