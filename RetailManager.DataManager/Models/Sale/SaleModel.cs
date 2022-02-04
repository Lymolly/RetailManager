using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.DataManager.Models.Sale;

namespace RetailManager.DataManager.Models.Sale
{
    public class SaleModel
    {
        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}
