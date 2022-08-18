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
    internal class SqlDataAccess : IDisposable
    {
        public string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProc, U parameters, string connectionStrName)
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
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        public void SaveDataInTransaction<T>(string storedProc, T parameters)
        {
            _connection.Execute(storedProc, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }
        public List<T> LoadDataInTransaction<T, U>(string storedProc, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProc, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction)
                .ToList();
            return rows;
        }
        public void StartTransaction(string connStrName)
        {
            string connectionString = GetConnectionString(connStrName);
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        private bool isClosed = false;
        public void CommitTransaction()
        {

            _transaction?.Commit();
            _connection?.Close();
            isClosed = true;
        }
        public void RollbackTransaction()
        {

            _transaction?.Rollback();
            _connection?.Close();
            isClosed = true;
        }

        public void Dispose()
        {
            if (!isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch
                {
                    //Logg this shit
                }
            }
            _connection = null;
            _transaction = null;
        }
    }
}
