using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomina.API.Enumeradores
{
    public enum ResultadoType : byte
    {
        success = 4,
        error = 1,
        info = 2,
        warning = 3,
        session = 0,
        permiso = 5,
        noMessage = 6,
        pregunta = 7
    }
}