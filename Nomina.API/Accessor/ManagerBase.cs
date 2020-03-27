using BLToolkit.Data;
using System.Data;

namespace Nomina.API.Accessor
{
    public class ManagerBase<T> where T : RepositoryBase
    {
        public T repositorio { get; set; }
        public IDbConnection connection { get; set; }
        public string ConnectionString { get; set; }
        public DbManager dbManager { get; set; }
        public int TimeOutCommand { get; set; }
        public void PrepareRepository()
        {
            this.repositorio.connection = this.connection;
            this.repositorio.ConnectionString = this.ConnectionString;
            this.repositorio.dbManager = this.dbManager;
            this.repositorio.TimeOutCommand = this.TimeOutCommand;
        }
        public void PrepareRepository<T>(T RepositorioAdicional) where T : RepositoryBase
        {
            RepositorioAdicional.connection = this.connection;
            RepositorioAdicional.ConnectionString = this.ConnectionString;
            RepositorioAdicional.dbManager = this.dbManager;
            RepositorioAdicional.TimeOutCommand = this.TimeOutCommand;
        }

    }
}

