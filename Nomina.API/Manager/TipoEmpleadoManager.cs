using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Accessor;
using Nomina.API.Repository;
using Nomina.API.Entidades;

namespace Nomina.API.Manager
{
    public class TipoEmpleadoManager : ManagerBase<TipoEmpleadoRepository>
    {
        public TipoEmpleadoManager()
        {
            this.repositorio = new TipoEmpleadoRepository();
            this.PrepareRepository();
        }
        

        public List<TipoEmpleado> ObtenerTiposEmpleados()
        {
            return this.repositorio.ObtenerTiposEmpleados();
        }


    }
}
