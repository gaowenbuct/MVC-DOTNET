using MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Daos
{
    public interface BaseDao<T>
    {
        PageResult<T> nativeQuerySQL(string sql,string sqlCount, IDictionary<string, object> parms,int startIndex,int pageSize);
        List<T> nativeQuerySQL(string sql, IDictionary<string, object> parms);

        //T FindByid(string tableName,string keyFieldName,long keyValue);

        //T FindByid(string tableName, string keyFieldName, string keyValue);

        T FindByid(string sql, string parmName, string parmValue);
        List<T> FindAll(string sql);
    }
}