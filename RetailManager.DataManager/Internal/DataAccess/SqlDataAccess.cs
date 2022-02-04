using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataManager.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProc,U parameters,string connectionStrName)
        {
            string connectionStr = GetConnectionString(connectionStrName);
            using (IDbConnection connection = new SqlConnection(connectionStr))
            {
                List<T> rows = connection.Query<T>(storedProc, parameters, commandType: CommandType.StoredProcedure)
                    .ToList();
                return rows;
            }
        }
        public void SaveData<T>(string storedProc, T parameters, string connectionStrName)
        {
            string connectionStr = GetConnectionString(connectionStrName);
            using (IDbConnection connection = new SqlConnection(connectionStr))
            {
                connection.Execute(storedProc, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
