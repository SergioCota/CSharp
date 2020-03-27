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

namespace Nomina.API.Controller
{
    public class MainController : ApiController
    {
        

        [HttpPost]
        public IResult Login (LoginSolicitud solicitud)
        {
            try
            {
                var Login = new EmpleadosManager().Login(solicitud.Email, solicitud.Password);

                if (Login != null )
                {
                    return new Resultado(){Data = Login, IsValid = true, Message = "Login Correcto", Type = Enumeradores.ResultadoType.success };
                }
                else
                {
                    return new Resultado() {IsValid = false, Message = "Usuario o contraseña incorrecta", Type = Enumeradores.ResultadoType.warning };
                }
            }
            catch(Exception ex)
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
