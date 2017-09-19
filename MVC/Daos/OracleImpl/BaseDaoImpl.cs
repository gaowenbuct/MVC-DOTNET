using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Utils;

namespace MVC.Daos.OracleImpl
{
    public class BaseDaoImpl<T> : BaseDao<T>
    {
        public T FindByid(string sql, string parmName, string parmValue)
        {
            throw new NotImplementedException();
        }
        public List<T> FindAll(string sql)
        {
            throw new NotImplementedException();
        }
        public PageResult<T> nativeQuerySQL(string sql, string sqlCount, IDictionary<string, object> parms, int startIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public List<T> nativeQuerySQL(string sql, IDictionary<string, object> parms)
        {
            throw new NotImplementedException();
        }
    }
}