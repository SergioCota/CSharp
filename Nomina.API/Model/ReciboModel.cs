using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.API.Model
{
    public class ReciboModel
    {
        public List<Conceptos> Percepciones { get; set; }
        public List<Conceptos> Deducciones { get; set; }
        public double TotalRecibo { get; set; }
    }
}
