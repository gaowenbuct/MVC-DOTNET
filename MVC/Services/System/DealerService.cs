using MVC.Models.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.System
{
    public interface DealerService
    {
        string doGetDealerCodeByUsername(string username);
        DealerUser doGetDealerUserInfo(string username);
        List<Dealer> doFindDealerAll();
    }
}