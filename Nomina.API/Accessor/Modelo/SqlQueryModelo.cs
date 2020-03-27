using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.API.Accessor.Modelo
{
    public class SqlQueryModelo
    {
        public List<string> SqlQueryList { get; set; }
        public SqlQueryModelo()
        {
            SqlQueryList = new List<string>();
        }
    }
}
