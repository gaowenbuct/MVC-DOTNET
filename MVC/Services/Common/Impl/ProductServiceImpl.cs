using MVC.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.Common;
using MVC.Daos;

namespace MVC.Services.Common.Impl
{
    public class ProductServiceImpl : BaseServiceImpl, ProductService
    {
        public List<Series> doFindAllSeries()
        {
            BaseDao<Series> baseDao = DaoFactory<Series>.CreateBaseDao(typeof(Series));
            return baseDao.FindAll("SELECT * FROM SERIES");
        }
        public List<Model> doFindModel(string seriesCode, string modelCode)
        {
            BaseDao<Model> baseDao = DaoFactory<Model>.CreateBaseDao(typeof(Model));
            IDictionary<string, object> parms = new Dictionary<string, object>();
            string sql = "SELECT * FROM MODEL WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(seriesCode))
            {
                sql = sql + " AND SERIES_CODE=@SERIES_CODE";
                parms.Add("SERIES_CODE", seriesCode);
            }
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                sql = sql + " AND MODEL_CODE LIKE '%@MODEL_CODE%'";
                parms.Add("MODEL_CODE", modelCode);
            }
            return baseDao.nativeQuerySQL(sql,parms);
        }
        public List<Color> doFindColor(string modelCode,string color)
        {
            BaseDao<Color> baseDao = DaoFactory<Color>.CreateBaseDao(typeof(Color));
            IDictionary<string, object> parms = new Dictionary<string, object>();
            string sql = "SELECT * FROM COLOR WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                sql = sql + " AND MODEL_CODE=@MODEL_CODE";
                parms.Add("MODEL_CODE", modelCode);
            }
            if (!string.IsNullOrWhiteSpace(color))
            {
                sql = sql + " AND COLOR_CODE LIKE '%@COLOR_CODE%'";
                parms.Add("COLOR_CODE", color);
            }
            return baseDao.nativeQuerySQL(sql, parms);
        }

        public List<Interior> doFindInterior(string modelCode,string interior)
        {
            BaseDao<Interior> baseDao = DaoFactory<Interior>.CreateBaseDao(typeof(Interior));
            IDictionary<string, object> parms = new Dictionary<string, object>();
            string sql = "SELECT * FROM INTERIOR WHERE 1=1";
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                sql = sql + " AND INTERIOR_CODE=@INTERIOR_CODE";
                parms.Add("INTERIOR_CODE", modelCode);
            }
            if (!string.IsNullOrWhiteSpace(interior))
            {
                sql = sql + " AND INTERIOR_CODE LIKE '%@INTERIOR_CODE%'";
                parms.Add("INTERIOR_CODE", interior);
            }
            return baseDao.nativeQuerySQL(sql, parms);
        }

        public Model doGetModelInfo(string modelCode)
        {
            BaseDao<Model> baseDao = DaoFactory<Model>.CreateBaseDao(typeof(Model));
            return baseDao.FindByid("SELECT * FROM MODEL", "ModelCode", modelCode);
        }
    }
}