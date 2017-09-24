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
        protected static log4net.ILog log = log4net.LogManager.GetLogger("MVC.Daos.Db2Impl.BaseDaoImpl");
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
                        try
                        {
                            pi.SetValue(t, rdr.GetValue(i++));
                        }
                        catch (Exception ex)
                        {
                            log.Error("字段类型错误，" + pi.Name + ":" + ex.Message);
                        }
                    }
                    result= t;
                }
            }
            return result;
        }
        public List<T> FindAll(string sql)
        {
            List<T> list = new List<T>();
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql))
            {
                if (rdr.Read())
                {
                    do
                    {
                        T t = Activator.CreateInstance<T>();
                        PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                        int i = 0;
                        foreach (PropertyInfo pi in propertyInfo)
                        {
                            try
                            {
                                pi.SetValue(t, rdr.GetValue(i++));
                            }
                            catch (Exception ex)
                            {
                                log.Error("字段类型错误，" + pi.Name + ":" + ex.Message);
                            }
                        }
                        list.Add(t);
                    } while (rdr.Read());
                }
            }
            return list;
        }
        public IDictionary<string,string> FindAllDic(string sql)
        {
            IDictionary<string, string> list = new Dictionary<string, string>();
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql))
            {
                if (rdr.Read())
                {
                    do
                    {
                        try
                        {
                            list.Add(rdr.GetString(0), rdr.GetString(1));
                        }
                        catch (Exception ex)
                        {
                            log.Error("字段类型错误，" + rdr.GetValue(0).ToString() + ":"+ rdr.GetValue(1).ToString() +":"+ ex.Message);
                        }
                    } while (rdr.Read());
                }
            }
            return list;
        }
        public PageResult<T> nativeQuerySql(string sql,string sqlCount, IDictionary<string, object> parms, int startIndex, int pageSize)
        {
            Debug.Assert(startIndex >= 0);
            Debug.Assert(pageSize > 0 && pageSize <= 100);
            List<T> list = list = new List<T>();
            int count = 0;
            iDB2Parameter[] db2Parms=null;
            if (parms != null)
            {
                List<iDB2Parameter> paramlist=new List<iDB2Parameter>();
                foreach(var item in parms)
                {
                    Debug.Assert(item.Value!=null);
                    paramlist.Add(new iDB2Parameter('@' + item.Key,item.Value));
                }
                db2Parms = paramlist.ToArray();
            }
            try
            {
                count= Convert.ToInt32(DB2Helper.ExecuteScalar(DB2Helper.ConnectionString, CommandType.Text, sqlCount, db2Parms));
            }
            catch (Exception ex)
            {
                log.Error("SQL错误:" + ex.Message);
                log.Error("SQL错误:" + sqlCount);
                if (parms != null)
                {
                    log.Error("SQL错误:" + parms.toJson());
                }
                throw ex;
            }

            if (count > 0)
            {
                try { 
                using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
                {
                    if (rdr.Read())
                    {
                        do
                        {
                            T t = Activator.CreateInstance<T>();
                            PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                            int i = 0;
                            foreach (PropertyInfo pi in propertyInfo)
                            {
                                try {
                                    pi.SetValue(t, rdr.GetValue(i++));
                                }catch(Exception ex)
                                {
                                    log.Error("字段类型错误，"+pi.Name+":"+ex.Message);
                                }
                            }
                            list.Add(t);
                        } while (rdr.Read());
                    }
                }
                }
                catch (Exception ex)
                {
                    log.Error("SQL错误:" + ex.Message);
                    log.Error("SQL错误:" + sql);
                    if (parms != null)
                    {
                        log.Error("SQL错误:" + parms.toJson());
                    }
                    throw ex;
                }
            }
            return new PageResult<T>(startIndex, pageSize, count, list);
        }

        public List<T> nativeQuerySql(string sql, IDictionary<string, object> parms)
        {
            iDB2Parameter[] db2Parms = null;
            List<T> list = new List<T>();
            if (parms != null)
            {
                List<iDB2Parameter> parmsList = new List<iDB2Parameter>();
                foreach (var item in parms)
                {
                    Debug.Assert(item.Value != null);
                    parmsList.Add(new iDB2Parameter('@' + item.Key, item.Value));
                }
                db2Parms = parmsList.ToArray();
            }
            try { 
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
            {
                if (rdr.Read())
                {
                    do
                    {
                        T t = Activator.CreateInstance<T>();
                        PropertyInfo[] propertyInfo = t.GetType().GetProperties();
                        int i = 0;
                        foreach (PropertyInfo pi in propertyInfo)
                        {
                            try
                            {
                                pi.SetValue(t, rdr.GetValue(i++));
                            }
                            catch (Exception ex)
                            {
                                log.Error("字段类型错误，" + pi.Name + ":" + ex.Message);
                            }
                        }
                        list.Add(t);
                    } while (rdr.Read());
                }
            }
            }
            catch (Exception ex)
            {
                log.Error("SQL错误:" + ex.Message);
                log.Error("SQL错误:" + sql);
                if (parms != null)
                {
                    log.Error("SQL错误:" + parms.toJson());
                }
                throw ex;
            }
            return list;
        }
        public string nativeQuerySqlReturnString(string sql, IDictionary<string, object> parms)
        {
            string result = string.Empty;
            iDB2Parameter[] db2Parms = null;
            if (parms != null)
            {
                List<iDB2Parameter> parmsList = new List<iDB2Parameter>();
                foreach (var item in parms)
                {
                    Debug.Assert(item.Value != null);
                    parmsList.Add(new iDB2Parameter('@' + item.Key, item.Value));
                }
                db2Parms = parmsList.ToArray();
            }
            using (iDB2DataReader rdr = DB2Helper.ExecuteReader(DB2Helper.ConnectionString, CommandType.Text, sql, db2Parms))
            {
                if (rdr.Read())
                {
                    result = rdr.GetString(0);
                }
            }
            return result;
        }
    }
}