using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.DataAccess;

namespace Nomina.API.Entidades
{
    public class TipoEmpleado
    {
        [PrimaryKey,Identity]
        public int tipoEmpleadoID { get; set; }
        public string Descripcion { get; set; }
    }
}
