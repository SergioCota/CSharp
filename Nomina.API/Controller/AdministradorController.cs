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
    public class AdministradorController : ApiController
    {
        [HttpPost]
        public IResult ObtenerTiposEmpleados()
        {
            try
            {
                var resultado = new TipoEmpleadoManager().ObtenerTiposEmpleados();

                return new Resultado()
                {
                    Data = resultado,
                    IsValid = true,
                    Message = "Tipos de empleados",
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
        [HttpPost]
        public IResult ObtenerEmpleados()
        {
            try
            {
                var resultado = new EmpleadosManager().ObtenerEmpleados();

                return new Resultado()
                {
                    Data = resultado,
                    IsValid = true,
                    Message = "Lista Empleados",
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

        [HttpPost]
        public IResult RegistarEmpleado(Empleados item)
        {
            try
            {
                new EmpleadosManager().RegistarEmpleado(item);

                return new Resultado()
                {
                    Data = { },
                    IsValid = true,
                    Message = "Empleado Registrado",
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
        [HttpPost]
        public IResult EditarEmpleado(Empleados item)
        {
            try
            {
                new EmpleadosManager().EditarEmpleado(item);

                return new Resultado()
                {
                    Data = { },
                    IsValid = true,
                    Message = "Empleado Registrado",
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
        [HttpPost]
        public IResult EliminarEmpleado(Empleados item)
        {
            try
            {
                if(item.EmpleadoID == 1)
                {
                    return new Resultado()
                    {
                        Data = { },
                        IsValid = false,
                        Message = "No se puede borrar al administrador",
                        Type = Enumeradores.ResultadoType.warning
                    };
                }
                new EmpleadosManager().EliminarEmpleado(item.EmpleadoID);

                return new Resultado()
                {
                    Data = { },
                    IsValid = true,
                    Message = "Empleado borrado",
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
        [HttpPost]
        public IResult GenerarNomina(GenerarNominaSolicitud solicitud)
        {
            try
            {
                
                new EmpleadosManager().GenerarNomina(solicitud);

                return new Resultado()
                {
                    Data = { },
                    IsValid = true,
                    Message = "Nominas Generadas",
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
