using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Services.Retail.Vo
{
    public class RetailOrderQueryListVo
    {
        public string Vin { get; set; }
        public string OrderNo { get; set; }
        public string RetailDateTime { get; set; }
        public string OutStockDate { get; set; }
        public string InvoiceDate { get; set; }
        public string ModelCode { get; set; }
        public string Color { get; set; }
        public string Color1 { get; set; }
        public string ModelYear { get; set; }
        public string ModelVersion { get; set; }
        public string Interior { get; set; }
        public string PrList { get; set; }
        public string CustomerName { get; set; }
        public string SalesSource { get; set; }
        public string OrderStatus { get; set; }
        public string Accessory { get; set; }
        public string Club { get; set; }
    }
}