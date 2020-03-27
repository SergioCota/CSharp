using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Interface;
using Nomina.API.Enumeradores;

namespace Nomina.API.Model
{
    public class Resultado : IResult
    {
        public object Data { get; set; }
        public Resultado()
        {
            this.Data = new object();
            this.IsValid = false;
            this.Type = ResultadoType.error;
            this.Message = string.Empty;
        }
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public ResultadoType Type { get; set; }
       
    }
}
