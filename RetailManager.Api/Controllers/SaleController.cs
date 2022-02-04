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
        public void Post(SaleModel saleModel)
        {
            string cashId = RequestContext.Principal.Identity.GetUserId();
            SaleData data = new SaleData();
            data.SaveSale(saleModel, cashId);
        }
    }
}
