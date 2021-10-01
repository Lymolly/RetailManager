using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RetailManager.DataManager.Internal.DataAccess;
using RetailManager.DataManager.Models;

namespace RetailManager.DataManager.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();
            var parameters = new { Id = id };

            var output = sqlDataAccess.LoadData<UserModel,dynamic>("dbo.spUserLookup", parameters, "RMConnection");
            return output;
        }
    }
}
