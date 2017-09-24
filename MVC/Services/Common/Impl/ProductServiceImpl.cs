using MVC.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.Common;
using MVC.Daos;
using MVC.Cache;
using MVC.Utils;

namespace MVC.Services.Common.Impl
{
    public class ProductServiceImpl : BaseServiceImpl, ProductService
    {
        public List<Series> doFindSeriesAll()
        {
            BaseDao<Series> baseDao = DaoFactory<Series>.CreateBaseDao(typeof(Series));
            return baseDao.FindAll("SELECT XSCXFL AS SeriesCode,XSCXFC AS SeriesName FROM SJVDTALIB.XSM11 SERIES WHERE ZMDWDM='08'");
        }
        public List<Model> doFindModelAll()
        {
            BaseDao<Model> baseDao = DaoFactory<Model>.CreateBaseDao(typeof(Model));
            return baseDao.FindAll("SELECT BSCLDM AS ModelCode,XSCLMC AS ModelName,XSCXFL SeriesCode FROM SJVDTALIB.KCM09 MODEL WHERE ZMDWDM='08'");
        }
        public List<Color> doFindColorAll()
        {
            BaseDao<Color> baseDao = DaoFactory<Color>.CreateBaseDao(typeof(Color));
            return baseDao.FindAll("SELECT VW_COLOR.XSYSDM AS VwColorCode,BSYSDM AS ColorCode,XSYSMC AS ColorName,'' AS ModelCode " +
                "FROM SJVDTALIB.XSM04 VW_COLOR,SJVDTALIB.KCM14 COLOR WHERE VW_COLOR.ZMDWDM='08' AND COLOR.ZMDWDM='08' AND VW_COLOR.XSYSDM=COLOR.XSYSDM");
        }
        public List<Interior> doFindInteriorAll()
        {
            BaseDao<Interior> baseDao = DaoFactory<Interior>.CreateBaseDao(typeof(Interior));
            return baseDao.FindAll("SELECT BSNSYM AS InteriorCode, BSNSJC AS InteriorName, '' AS ModelCode FROM SJVDTALIB.KCM10 INTERIOR WHERE ZMDWDM = '08'");
        }
        public List<ModelGroup> doFindModelGroupAll()
        {
            //log.Info("doFindModelPrAll begin");
            getLogger(this).Info("doFindModelGroupAll begin");
            BaseDao<ModelGroup> baseDao = DaoFactory<ModelGroup>.CreateBaseDao(typeof(ModelGroup));
            List<ModelGroup> list=baseDao.FindAll("SELECT XSCLDM AS ModelGroupCode,BSCLDM AS ModelCode,CHAR(KCMDY1) AS ModelYear,KCVER1 AS ModelVersion,BSNSYM AS InteriorCode," +
                "CYCLP1 AS PrList, BSPNXH AS ModelPrNo FROM SJVDTALIB.KCM32 MODEL_GROUP WHERE ZMDWDM = '08' AND KCMDY1>="+ CfgReader.ModelGroupDataBeginYear);
            getLogger(this).Info("doFindModelGroupAll end");
            return list;
        }
        public IDictionary<string, string> doFindModelList(string seriesCode, string modelCode)
        {
            IDictionary<string, Model> modelDic = CommonCache.GetModelDic();
            if (!string.IsNullOrWhiteSpace(seriesCode))
            {
                return modelDic.Where(x => x.Value.SeriesCode== seriesCode).ToDictionary(x => x.Key, x => x.Value.ModelName);
            }
            else if (!string.IsNullOrWhiteSpace(modelCode))
            {
                return modelDic.Where(x => x.Key.Contains(modelCode)).ToDictionary(x => x.Key, x => x.Value.ModelName);
            }
            else
            {
                return modelDic.ToDictionary(x => x.Key, x => x.Value.ModelName);
            }
        }
        public List<Color> doFindModelColor(string modelCode,string color)
        {
            BaseDao<Color> baseDao = DaoFactory<Color>.CreateBaseDao(typeof(Color));
            IDictionary<string, object> parms = new Dictionary<string, object>();
            string sql = "SELECT XSYSDM AS ColorCode,XSYSMC AS ColorName,'' AS ModelCode FROM SJVDTALIB.XSM04 COLOR WHERE ZMDWDM='08'";
            /*if (!string.IsNullOrWhiteSpace(modelCode))
            {
                sql = sql + " AND MODEL_CODE=@MODEL_CODE";
                parms.Add("MODEL_CODE", modelCode);
            }*/
            if (!string.IsNullOrWhiteSpace(color))
            {
                sql = sql + " AND XSYSDM=@COLOR_CODE";
                parms.Add("COLOR_CODE", color);
            }
            return baseDao.nativeQuerySql(sql, parms);
        }

        public List<Interior> doFindModelInterior(string modelCode,string interior)
        {
            BaseDao<Interior> baseDao = DaoFactory<Interior>.CreateBaseDao(typeof(Interior));
            IDictionary<string, object> parms = new Dictionary<string, object>();
            string sql = "SELECT BSNSYM AS InteriorCode,BSNSJC AS InteriorName,'' AS ModelCode FROM SJVDTALIB.KCM10 INTERIOR WHERE ZMDWDM='08'";
            /*if (!string.IsNullOrWhiteSpace(modelCode))
            {
                sql = sql + " AND INTERIOR_CODE=@INTERIOR_CODE";
                parms.Add("INTERIOR_CODE", modelCode);
            }*/
            if (!string.IsNullOrWhiteSpace(interior))
            {
                sql = sql + " AND BSNSYM=@INTERIOR_CODE";
                parms.Add("INTERIOR_CODE", interior);
            }
            return baseDao.nativeQuerySql(sql, parms);
        }
        public Model doGetModelInfo(string modelCode)
        {
            IDictionary<string, Model> modelDic = CommonCache.GetModelDic();
            return modelDic[modelCode];
            //BaseDao<Model> baseDao = DaoFactory<Model>.CreateBaseDao(typeof(Model));
            //return baseDao.FindByid("SELECT BSCLDM AS ModelCode,XSCLMC AS ModelName,XSCXFL SeriesCode FROM SJVDTALIB.KCM09 MODEL WHERE ZMDWDM='08' AND BSCLDM =@MODEL_CODE", "MODEL_CODE", modelCode);
        }
        public Color doGetColorInfo(string colorCode)
        {
            IDictionary<string, string> colorDic = CommonCache.GetColorDic();
            Color color = null;
            if (colorDic.ContainsKey(colorCode))
            {
                color = new Color();
                color.ColorCode = colorCode;
                color.ColorName = colorDic[colorCode];
            }
            return color;
        }
        public Interior doGetInteriorInfo(string interiorCode)
        {
            IDictionary<string, string> interiorDic = CommonCache.GetInteriorDic();
            Interior interior = null;
            if (interiorDic.ContainsKey(interiorCode))
            {
                interior = new Interior();
                interior.InteriorCode = interiorCode;
                interior.InteriorName = interiorDic[interiorCode];
            }
            return interior;
        }
        public IDictionary<string, string> doFindColorList(string colorCode, string colorName)
        {
            IDictionary<string, string> colorDic = CommonCache.GetColorDic();
            if (!string.IsNullOrWhiteSpace(colorCode))
            {
                return colorDic.Where(x => x.Key.Contains(colorCode)).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (!string.IsNullOrWhiteSpace(colorName))
            {
                return colorDic.Where(x => x.Value.Contains(colorName)).ToDictionary(x=>x.Key,x=>x.Value);
            }
            else
            {
                return colorDic;
            }
        }
        public IDictionary<string, string> doFindInteriorList(string interiorCode, string interiorName)
        {
            IDictionary<string, string> interiorDic = CommonCache.GetInteriorDic();
            if (!string.IsNullOrWhiteSpace(interiorCode))
            {
                return interiorDic.Where(x => x.Key.Contains(interiorCode)).ToDictionary(x => x.Key, x => x.Value);
            }
            else if (!string.IsNullOrWhiteSpace(interiorName))
            {
                return interiorDic.Where(x => x.Key.Contains(interiorName)).ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                return CommonCache.GetInteriorDic();
            }
        }
    }
}