﻿using MVC.Daos;
using MVC.Daos.Db2Impl;
using MVC.Models.Retail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail.Impl
{
    public class RetailOrderServiceImpl : RetailOrderService
    {
        private static readonly BaseDao<RetailOrder> baseDao = DaoFactory<RetailOrder>.CreateBaseDao(typeof(RetailOrder));
        public RetailOrder Details(string orderNo)
        {
            String sql = "SELECT '1' AS Vin,'1' AS OrderNo,'1' AS RetailTime,'1' AS OutStockDate,'1' AS InvoiceDate," +
                "'1' AS ModelCode,'1' AS Color,'1' AS ModelYear,'1' AS ModelVersion,'1' AS Interior,'1' AS PrList," +
                "'1' AS CustomerName,'1' AS SaleSource,'1' AS OrderStatus,'1' AS Accessory,'1' AS Club" +
                " FROM RETAIL_ORDER WHERE ORDER_NO=@ORDER_NO";
            return baseDao.FindByid(sql, "OrderNo", orderNo);
        }
    }
}