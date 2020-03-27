using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Enumeradores;

namespace Nomina.API.Model
{
    public class NominaModel
    {
        public int EmpleadoID { get; set; }
        public DateTime Fecha { get; set; }
        public double totalDeposito { get; set; }
        public List<Conceptos> conceptos { get; set; }
        
        public double SueldoBase { get; set; }
        public double Prestamos { get; set; }
        public double DeduccionDesayuno { get; set; }
        public double DeduccionAhorro { get; set; }
        public double TarjetaGasolina { get; set; }

    }

    public class Conceptos
    {
        public double importe { get; set; }
        public string descripcion { get; set; }
        public naturalezaConcepto naturaleza { get; set; }
    }
}
