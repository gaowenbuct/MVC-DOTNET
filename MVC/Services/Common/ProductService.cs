using MVC.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Common
{
    public interface ProductService:BaseService
    {
        List<Series> doFindSeriesAll();
        List<Model> doFindModelAll();
        List<Color> doFindColorAll();
        List<Interior> doFindInteriorAll();
        List<ModelGroup> doFindModelGroupAll();
        IDictionary<string, string> doFindModelList(string seriesCode,string modelCode);
        List<Color> doFindModelColor(string modelCode,string color);
        List<Interior> doFindModelInterior(string modelCode,string interior);
        Model doGetModelInfo(string modelCode);
        IDictionary<string, string> doFindColorList(string colorCode, string colorName);
        IDictionary<string, string> doFindInteriorList(string interiorCode, string interiorName);
    }
}