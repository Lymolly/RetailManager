using RetailManager.DataManager.DataAccess;
using RetailManager.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManager.Api.Controllers
{
    [Authorize]
    public class InventoryController : ApiController
    {
        [HttpGet]
        [Authorize(Roles ="Manager,Admin")]
        public List<InventoryModel> Get()
        {
            InventoryData data = new InventoryData();
            var res = data.GetInventory();
            return res;
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public void Post(InventoryModel model)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(model);
        }
    }
}
