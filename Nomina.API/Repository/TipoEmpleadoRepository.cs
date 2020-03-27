using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Accessor;
using Nomina.API.Entidades;

namespace Nomina.API.Repository
{
    public class TipoEmpleadoRepository : RepositoryBase<DataAccesorTipoEmpleado>
    {
        public List<TipoEmpleado> ObtenerTiposEmpleados()
        {
            using(this.DataAccesor = this.CrearDataAccesor())
            {
                return this.DataAccesor.NoLock<TipoEmpleado>().ToList(); 
            }           
        }
    }

    public abstract class DataAccesorTipoEmpleado : DataAccessorBase<TipoEmpleado>
    {

    }
}
