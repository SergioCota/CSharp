using BLToolkit.Data;
using System.Data;

namespace Nomina.API.Accessor
{
    public abstract class RepositoryBase<T> : RepositoryBase where T : DataAccessorBase
    {
        public T DataAccesor { get; set; }

        public virtual T CrearDataAccesor()
        {

            var da = default(T);
            if (!string.IsNullOrEmpty(this.ConnectionString))
            {
                da = DataAccesorControl<T>.CrearDataManager(this.ConnectionString);
            }
            else if (this.connection != null)
            {
                da = DataAccesorControl<T>.CrearDataManager(this.connection);
            }
            else if (this.dbManager != null)
            {
                da = DataAccesorControl<T>.CrearDataManager(this.dbManager);
            }
            else
            {
                da = DataAccesorControl<T>.CrearDataManager();
            }

            if (this.TimeOutCommand > 30)
            {
                da.CommandTimeout = this.TimeOutCommand;
            }
            return da;

        }
       

        public virtual IDbTransaction IniciarTransaccion()
        {
            DataAccesor = CrearDataAccesor();
            dbManager = DataAccesor.GetDbManager();
            if (this.EndTransaction)
                dbManager.BeginTransaction();
            transaction = dbManager.Transaction;
            return transaction;
        }

        public virtual void AnularTransaccion()
        {
            if (this.transaction != null && this.EndTransaction)
            {
                dbManager.RollbackTransaction();
                DataAccesor.Dispose();
            }
        }
        public virtual void TerminarTransaccion()
        {
            if (this.transaction != null && this.EndTransaction)
            {
                dbManager.CommitTransaction();
                DataAccesor.Dispose();
            }

        }
    }
    public abstract class RepositoryBase
    {
        bool endTransaction = true;
        public IDbConnection connection { get; set; }
        public IDbTransaction transaction { get; set; }
        public string ConnectionString { get; set; }
        public DbManager dbManager { get; set; }
        public int TimeOutCommand { get; set; }
        public bool EndTransaction
        {
            get { return endTransaction; }
            set { endTransaction = value; }
        }
    }



}
