using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Enumeradores;

namespace Nomina.API.Interface
{
    public interface IResult
    {
        bool IsValid { get; set; }
        string Message { get; set; }
        ResultadoType Type { get; set; }        
    }    
   
}
