using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.DataAccess;

namespace Nomina.API.Entidades
{
    public class ReciboNomina
    {
        [PrimaryKey,Identity]
        public int ReciboNominaID { get; set; }
        public int EmpleadoID { get; set; }
        public DateTime Fecha { get; set; }
        public double totalDeposito { get; set; }
    }
}
