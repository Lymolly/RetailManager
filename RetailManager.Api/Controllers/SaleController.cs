using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using RetailManager.DataManager.DataAccess;
using RetailManager.DataManager.Models;
using RetailManager.DataManager.Models.Sale;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel saleModel)
        {
            string cashId = RequestContext.Principal.Identity.GetUserId();
            SaleData data = new SaleData();
            data.SaveSale(saleModel, cashId);
        }
        [Authorize(Roles ="Admin,Manager")]
        [HttpGet]
        [Route("GetSalesReport")]
        public List<SaleReportModel> GetSalesReport()
        {
            if (RequestContext.Principal.IsInRole("Admin"))
            {
                //Do admin stuff
            }
            else
            {
                //Do manager stuff 
            }
            SaleData data = new SaleData();
            var res = data.GetSaleReport();
            return res;
        }
    }
}
