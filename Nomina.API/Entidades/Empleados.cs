using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.DataAccess;

namespace Nomina.API.Entidades
{
    public class Empleados
    {
        [PrimaryKey, Identity]
        public int EmpleadoID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaIngreso { get; set; }
        public double SueldoBase { get; set; }
        public double Prestamos { get; set; }        
        public double DeduccionDesayuno { get; set; }
        public double DeduccionAhorro { get; set; }
        public double TarjetaGasolina { get; set; }        
        public bool Activo { get; set; }
        public int tipoEmpleadoID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
