﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.DataManager.Internal.DataAccess;
using RetailManager.DataManager.Models;

namespace RetailManager.DataManager.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            var output = sqlDataAccess.LoadData<ProductModel,dynamic>("dbo.spProduct_GetAll",new {},"RMConnection");
            return output;
        }
        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            var output =
                sqlDataAccess.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId },
                    "RMConnection").FirstOrDefault();
            return output;
        }
    }
}
