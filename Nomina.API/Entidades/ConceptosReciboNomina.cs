using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Enumeradores;
using BLToolkit.DataAccess;

namespace Nomina.API.Entidades
{
    public class ConceptosReciboNomina
    {
        [PrimaryKey,Identity]
        public int ConceptosReciboNominaID { get; set; }
        public int ReciboNominaID { get; set; }
        public double importe { get; set; }
        public string descripcion { get; set; }        
        public naturalezaConcepto naturaleza { get; set; }
    }
}
