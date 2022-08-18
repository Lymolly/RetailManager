using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.DataManager.Internal.DataAccess;
using RetailManager.DataManager.Models;
using RetailManager.DataManager.Models.Sale;

namespace RetailManager.DataManager.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo,string cashierId)
        {
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                var productInfo = products.GetProductById(detail.ProductId);
                if (productInfo == null)
                {
                    throw new Exception($"Product with {detail.ProductId} identifier not found!");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }
                details.Add(detail);
            }

            SaleDBModel sale = new SaleDBModel
            {
                Subtotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };
            sale.Total = sale.Subtotal + sale.Tax;
            
            using (var sqlDataAccess = new SqlDataAccess())
            {
                try
                {
                    sqlDataAccess.StartTransaction("RMConnection");
                    sqlDataAccess.SaveDataInTransaction("dbo.spSale_Insert", sale);
                    sale.Id = sqlDataAccess.LoadDataInTransaction<int, dynamic>("dbo.spSale_Lookup",
                    new { sale.CashierId, sale.SaleDate })
                    .FirstOrDefault();
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;
                        sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }
                    sqlDataAccess.CommitTransaction();
                }
                catch
                {
                    sqlDataAccess.RollbackTransaction();
                    throw;
                }
            }
            
        }
        public List<SaleReportModel> GetSaleReport()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport", new { }, "RMConnection");
            return output;
        }

        
    }
}
