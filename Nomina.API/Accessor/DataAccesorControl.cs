using BLToolkit.Data;
using BLToolkit.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;


namespace Nomina.API.Accessor
{
    public static class DataAccesorControl<T> where T : DataAccessorBase
    {
        private const int CommandTimeoutDefault = 240;

        public static T CrearDataManager(IDbTransaction transaction)
        {
            DbManager dbManager = new DbManager(transaction);
            T dataManager = DataAccessor.CreateInstance<T>(dbManager);
            dataManager.CommandTimeout = CommandTimeoutDefault;
            return dataManager;
        }
        public static T CrearDataManager(IDbConnection connection)
        {
            DbManager dbManager = new DbManager(connection);

            T dataManager = DataAccessor.CreateInstance<T>(dbManager);
            dataManager.CommandTimeout = CommandTimeoutDefault;
            return dataManager;
        }
        public static T CrearDataManager()
        {
            DbManager dbManager = new DbManager();
            T dataManager = DataAccessor.CreateInstance<T>(dbManager);
            dataManager.CommandTimeout = CommandTimeoutDefault;
            return dataManager;
        }
        public static T CrearDataManager(string connectionString)
        {
            IDbConnection conn = new SqlConnection(connectionString);
            DbManager dbManager = new DbManager(conn);
            T dataManager = DataAccessor.CreateInstance<T>(dbManager);
            dataManager.CommandTimeout = CommandTimeoutDefault;
            return dataManager;
        }
        public static T CrearDataManager(DbManager dbManager)
        {
            T dataManager = DataAccessor.CreateInstance<T>(dbManager);
            dataManager.CommandTimeout = CommandTimeoutDefault;
            return dataManager;
        }
    }
}
