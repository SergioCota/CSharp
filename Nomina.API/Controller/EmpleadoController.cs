using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Nomina.API.Manager;
using Nomina.API.Interface;
using Nomina.API.Model;
using Nomina.API.Solicitudes;
using Nomina.API.Entidades;

namespace Nomina.API.Controller
{
    public class EmpleadoController: ApiController
    {
        [HttpPost]
        public IResult BuscarReciboEmpleadoPorPeriodo(EmpleadoSolicitud solicitud)
        {
            try
            {
                var resultado = new EmpleadosManager().BuscarReciboEmpleadoPorPeriodo(solicitud);

                if(resultado.Percepciones.Count == 0 && resultado.Deducciones.Count == 0)
                {
                    return new Resultado()
                    {
                        Data = null,
                        IsValid = false,
                        Message = "Periodo sin recibo",
                        Type = Enumeradores.ResultadoType.warning
                    };
                }

                return new Resultado()
                {
                    Data = resultado,
                    IsValid = true,
                    Message = "Conceptos del periodo",
                    Type = Enumeradores.ResultadoType.success
                };
            }
            catch (Exception ex)
            {
                return new Resultado()
                {
                    IsValid = false,
                    Message = ex.Message,
                    Type = Enumeradores.ResultadoType.error
                };
            }
        }

        public IResult BuscarEmpleado(EmpleadoSolicitud solicitud)
        {
            try
            {
                var resultado = new EmpleadosManager().BuscarEmpleado(solicitud.EmpleadoID);                

                return new Resultado()
                {
                    Data = resultado,
                    IsValid = true,
                    Message = "Empleado logueado",
                    Type = Enumeradores.ResultadoType.success
                };
            }
            catch (Exception ex)
            {
                return new Resultado()
                {
                    IsValid = false,
                    Message = ex.Message,
                    Type = Enumeradores.ResultadoType.error
                };
            }
        }
    }
}
