using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Accessor;
using Nomina.API.Repository;
using Nomina.API.Entidades;
using Nomina.API.Solicitudes;
using Nomina.API.Model;

namespace Nomina.API.Manager
{
    public class EmpleadosManager : ManagerBase<EmpleadosRepository>
    {
        public EmpleadosManager()
        {
            this.repositorio = new EmpleadosRepository();
            this.PrepareRepository();
        }

        public LoginModel Login(string Email, string Password)
        {
            return this.repositorio.Login(Email, Password);
        }
        public EmpleadoModel BuscarEmpleado(int EmpleadoID)
        {
            return this.repositorio.BuscarEmpleado(EmpleadoID);
        }
        
        public List<Empleados> ObtenerEmpleados()
        {
            return this.repositorio.ObtenerEmpleados();
        }
        public void RegistarEmpleado(Empleados item)
        {
            this.repositorio.RegistarEmpleado(item);
        }


        public void EditarEmpleado(Empleados item)
        {
            this.repositorio.EditarEmpleado(item);
        }

        public void EliminarEmpleado(int EmpleadoID)
        {
            this.repositorio.EliminarEmpleado(EmpleadoID);
        }

        public void GenerarNomina(GenerarNominaSolicitud solicitud)
        {
            ReciboNomina recibo = new ReciboNomina();
            double totalDeposito = 0;
            double totalPercepciones = 0;
            double totalDeducciones = 0;

            var Empleados = this.repositorio.ObtenerDatosGenerarNominas(solicitud.periodoID);

            Empleados.ForEach(x =>
            {

                totalPercepciones = Math.Round(x.conceptos.Where(w => w.naturaleza == Enumeradores.naturalezaConcepto.percepciones).Select(s => s.importe).Sum(), 2);
                totalDeducciones = Math.Round(x.conceptos.Where(w => w.naturaleza == Enumeradores.naturalezaConcepto.deducciones).Select(s => s.importe).Sum(), 2);

                totalDeposito = totalPercepciones - totalDeducciones;

                recibo = new ReciboNomina()
                {
                    ReciboNominaID = 0,
                    EmpleadoID = x.EmpleadoID,
                    Fecha = x.Fecha,
                    totalDeposito = totalDeposito
                };

                int ReciboID = this.repositorio.GuardarReciboNomina(recibo);

                var ConceptosRecibo = (from c in x.conceptos
                                 select new ConceptosReciboNomina
                                 {
                                     ConceptosReciboNominaID = 0,
                                     ReciboNominaID = ReciboID,
                                     descripcion = c.descripcion,
                                     importe = c.importe,
                                     naturaleza = c.naturaleza
                                 }).ToList();

                this.repositorio.GuardarConceptosRecibo(ConceptosRecibo);           

            });

        }

        public ReciboModel BuscarReciboEmpleadoPorPeriodo(EmpleadoSolicitud solicitud)
        {
            return this.repositorio.BuscarReciboEmpleadoPorPeriodo(solicitud.EmpleadoID, solicitud.PeriodoID);
        }

    }
}
