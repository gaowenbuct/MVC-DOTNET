using MVC.Models.Retail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail.Impl
{
    public interface RetailService
    {
        List<SalesPerson> doFindSalesPersonAll();
        IDictionary<string, string> doFindInsuranceCompanyAll();
    }
}