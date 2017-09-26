using MVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Common
{
    public interface CommonService
    {
        List<County> doFindCountyAll();
    }
}