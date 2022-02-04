using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataManager
{
    public class ConfigHelper 
    {
        public static decimal GetTaxRate()
        {

            string rateText = ConfigurationManager.AppSettings.Get("taxRate");
            var isSucced = decimal.TryParse(rateText, out decimal output);
            if (!isSucced)
            {
                throw new Exception("No tax value!");
            }
            return output;
        }
    }
}
