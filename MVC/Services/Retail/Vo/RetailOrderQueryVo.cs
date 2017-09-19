using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail.Vo
{
    public class RetailOrderQueryVo
    {
        public string Vin { get; set; }
        public string OrderNo { get; set; }
        public string RetailTime { get; set; }
        public string OutStockDate { get; set; }
        public string InvoiceDate { get; set; }
        public string ModelCode { get; set; }
        public string Color { get; set; }
        public string ModelYear { get; set; }
        public string ModelVersion { get; set; }
        public string Interior { get; set; }
        public string PrList { get; set; }
        public string CustomerName { get; set; }
        public string SaleSource { get; set; }
        public string OrderStatus { get; set; }
        public string Accessory { get; set; }
        public string Club { get; set; }
    }
}