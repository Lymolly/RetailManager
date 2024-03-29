﻿using RetailManager.DataManager.Internal.DataAccess;
using RetailManager.DataManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataManager.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess();
            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll", new { }, "RMConnection");
            return output;
        }
        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess();
            sql.SaveData<InventoryModel>("dbo.spInventory_Insert", item, "RMConnection");
        }
    }
}
