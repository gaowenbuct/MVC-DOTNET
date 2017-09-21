using MVC.Services.Retail.Vo;
using MVC.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail
{
    public interface RetailQueryService : BaseService
    {
        PageResult<RetailOrderQueryVo> QueryRetailOrderList(QueryCriteria queryCriteria);
        byte[] ExportRetailOrderList(NameValueCollection queryString);
    }
}