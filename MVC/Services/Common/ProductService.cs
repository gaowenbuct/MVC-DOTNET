using MVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Common
{
    public interface ProductService:BaseService
    {
        List<Series> doFindAllSeries();
        List<Model> doFindModel(string seriesCode,string modelCode);
        List<Color> doFindColor(string modelCode,string color);
        List<Interior> doFindInterior(string modelCode,string interior);
        Model doGetModelInfo(string modelCode);
    }
}