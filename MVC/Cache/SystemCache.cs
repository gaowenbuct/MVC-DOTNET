using MVC.Models.System;
using MVC.Services.System;
using MVC.Services.System.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Cache
{
    public class SystemCache
    {
        private static DealerService dealerService = new DealerServiceImpl();
        private static IDictionary<string, Dealer> dealerDic;
        public static IDictionary<string, Dealer> GetDealerDic()
        {
            if (dealerDic == null)
            {
                List<Dealer> list = dealerService.doFindDealerAll();
                dealerDic = list.ToDictionary(x => x.DealerCode, x => x);
            }
            return dealerDic;
        }
    }
}