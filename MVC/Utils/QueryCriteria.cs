using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MVC.Utils
{
    public class QueryCriteria
    {
        private int startIndex = 0;
        private int pageSize = 10;
        private NameValueCollection queryCondition = null; 

        public int StartIndex { get { return startIndex; } set { startIndex = value; } }
        public int PageSize { get { return pageSize; } set { pageSize = value; } }

        public NameValueCollection QueryCondition { get { return queryCondition; }  set { queryCondition=value; } }

        public QueryCriteria(NameValueCollection queryString)
        {
            this.queryCondition = queryString;
            if (!string.IsNullOrWhiteSpace(queryString["startIndex"]))
            {
                this.startIndex = Convert.ToInt32(queryString["startIndex"]);
            }
            if (!string.IsNullOrWhiteSpace(queryString["pageSize"]))
            {
                this.pageSize = Convert.ToInt32(queryString["pageSize"]);
            }
        }
    }
}