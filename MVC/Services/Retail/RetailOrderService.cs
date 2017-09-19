using MVC.Models.Retail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail
{
    public interface RetailOrderService
    {
        RetailOrder Details(string orderNo);
    }
}