using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models.Common
{
    public class ModelGroup
    {
        public string ModelGroupCode { get; set; }
        public string ModelCode { get; set; }
        public string ModelYear { get; set; }
        public string ModelVersion { get; set; }
        public string InteriorCode { get; set; }
        public string PrList { get; set; }
        public string ModelPrNo { get; set; }
    }
}