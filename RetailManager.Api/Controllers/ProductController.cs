using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RetailManager.DataManager.DataAccess;
using RetailManager.DataManager.Models;

namespace RetailManager.Api.Controllers
{
    //[Authorize]
    public class ProductController : ApiController
    {
        public List<ProductModel> Get()
        {
            ProductData productData = new ProductData();
            var res = productData.GetProducts();
            return res;
        }
    }
}
