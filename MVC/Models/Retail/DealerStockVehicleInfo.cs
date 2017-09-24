using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Retail
{
    public class DealerStockVehicleInfo
    {
        public string Vin { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public string DealerCode { get; set; }
        public string VehicleStatus { get; set; }
        public string VehicleProperties { get; set; }
        public string VehicleInStockDate { get; set; }
        public string VehicleOutStockDate { get; set; }
    }
}