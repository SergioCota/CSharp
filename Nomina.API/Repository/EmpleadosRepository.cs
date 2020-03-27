using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomina.API.Accessor;
using Nomina.API.Entidades;
using Nomina.API.Model;

namespace Nomina.API.Repository
{
    public class EmpleadosRepository : RepositoryBase<DataAccessorEmpleados>
    {
        public LoginModel Login(string Email, string Password)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                var tEmpleados = this.DataAccesor.NoLock<Empleados>();

                var Login = (from e in tEmpleados
                             where e.Email == Email && e.Password == Password
                             select new LoginModel { 
                                 EmpleadoID = e.EmpleadoID,
                                 TipoEmpleadoID = e.tipoEmpleadoID
                             }).FirstOrDefault();

                return Login;
            }
        }

        public List<Empleados> ObtenerEmpleados()
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                return this.DataAccesor.NoLock<Empleados>().ToList();
            }
        }

        public int RegistarEmpleado(Empleados item)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                int id = 0;
                item.FechaIngreso = DateTime.Now;
                this.DataAccesor.Insert<Empleados>(item, out id);
                return id;
            }
        }
        public void EditarEmpleado(Empleados value)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                var Empleado = (from item in this.DataAccesor.GetQueryable()
                                where item.EmpleadoID == value.EmpleadoID
                                select new Empleados
                                {
                                    EmpleadoID = item.EmpleadoID,
                                    Nombre = value.Nombre,
                                    ApellidoPaterno = value.ApellidoPaterno,
                                    ApellidoMaterno = value.ApellidoMaterno,
                                    FechaIngreso = item.FechaIngreso,
                                    SueldoBase = value.SueldoBase,
                                    Prestamos = value.Prestamos,
                                    DeduccionDesayuno = value.DeduccionDesayuno,
                                    DeduccionAhorro = value.DeduccionAhorro,
                                    TarjetaGasolina = value.TarjetaGasolina,
                                    Activo = value.Activo,
                                    tipoEmpleadoID = value.tipoEmpleadoID,
                                    Email = value.Email,
                                    Password = value.Password
                                }).FirstOrDefault();


                this.DataAccesor.Update<Empleados>(Empleado);
            }
        }

        public void EliminarEmpleado(int EmpleadoID)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                var Empleado = (from item in this.DataAccesor.GetQueryable()
                                where item.EmpleadoID == EmpleadoID
                                select item).FirstOrDefault();

                this.DataAccesor.Delete(Empleado);
            }
        }

        public int GuardarReciboNomina(ReciboNomina item)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                int id = 0;
                this.DataAccesor.Insert<ReciboNomina>(item, out id);
                return id;  
            }
        }

        public void GuardarConceptosRecibo(List<ConceptosReciboNomina> items)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                this.DataAccesor.Insert<ConceptosReciboNomina>(items);                
            }
        }

        public EmpleadoModel BuscarEmpleado(int EmpleadoID)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                var Empleado = (from item in this.DataAccesor.GetQueryable()
                                where item.EmpleadoID == EmpleadoID
                                select new EmpleadoModel
                                {
                                    EmpleadoID = item.EmpleadoID,
                                    NombreCompleto = item.Nombre + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno
                                }).FirstOrDefault();

                return Empleado;
            }
        }

            public ReciboModel BuscarReciboEmpleadoPorPeriodo(int EmpleadoID, int PeriodoID)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {
                DateTime fecha = new DateTime(DateTime.Now.Year, PeriodoID, 1);

                List<ReciboNomina> ReciboNomina = this.DataAccesor.NoLock<ReciboNomina>().Where(x => x.Fecha == fecha && x.EmpleadoID == EmpleadoID).ToList();
                List<int> RecibosIDs = ReciboNomina.Select(s => s.ReciboNominaID).ToList();
                List<ConceptosReciboNomina> Conceptos = this.DataAccesor.NoLock<ConceptosReciboNomina>().Where(x =>
                                                        RecibosIDs.Contains(x.ReciboNominaID)).ToList();

                List<Conceptos> ConceptosPercepciones = (from percepciones in Conceptos
                                                         where percepciones.naturaleza == Enumeradores.naturalezaConcepto.percepciones
                                                         select new Conceptos
                                                         {
                                                             importe = percepciones.importe,
                                                             descripcion = percepciones.descripcion,
                                                             naturaleza = percepciones.naturaleza
                                                         }).ToList();

                double totalPercepciones = ConceptosPercepciones.Select(s => s.importe).Sum();
                Conceptos total = new Conceptos();

                if (ConceptosPercepciones.Count > 0)
                {
                    total = new Conceptos()
                    {
                        importe = totalPercepciones,
                        descripcion = "TOTAL PERCEPCIONES",
                        naturaleza = Enumeradores.naturalezaConcepto.percepciones
                    };
                    ConceptosPercepciones.Add(total);
                }

                List<Conceptos> ConceptosDeducciones = (from deducciones in Conceptos
                                                         where deducciones.naturaleza == Enumeradores.naturalezaConcepto.deducciones
                                                         select new Conceptos
                                                         {
                                                             importe = deducciones.importe,
                                                             descripcion = deducciones.descripcion,
                                                             naturaleza = deducciones.naturaleza
                                                         }).ToList();

                double totalDeducciones = ConceptosDeducciones.Select(s => s.importe).Sum();

                if (ConceptosPercepciones.Count > 0)
                {
                    total = new Conceptos()
                    {
                        importe = totalDeducciones,
                        descripcion = "TOTAL DEDUCCIONES",
                        naturaleza = Enumeradores.naturalezaConcepto.deducciones
                    };
                    ConceptosDeducciones.Add(total);
                }

                ReciboModel ReciboModel = new ReciboModel() {
                    Percepciones = ConceptosPercepciones,
                    Deducciones = ConceptosDeducciones,
                    TotalRecibo = totalPercepciones - totalDeducciones
                };

                return ReciboModel;

            }
        }        

        public List<NominaModel> ObtenerDatosGenerarNominas(int PeriodoID)
        {
            using (this.DataAccesor = this.CrearDataAccesor())
            {

                DateTime fecha = new DateTime(DateTime.Now.Year, PeriodoID, 1);
                List<Conceptos> Listconceptos = new List<Conceptos>();
                Conceptos conceptos = new Conceptos();

                /*Regenerar los recibos*/
                List<ReciboNomina> ReciboNomina = this.DataAccesor.NoLock<ReciboNomina>().Where(x => x.Fecha == fecha).ToList();
                List<int> RecibosIDs = ReciboNomina.Select(s => s.ReciboNominaID).ToList();
                List<ConceptosReciboNomina> Conceptos = this.DataAccesor.NoLock<ConceptosReciboNomina>().Where(x =>
                                                        RecibosIDs.Contains(x.ReciboNominaID)).ToList();
                if (RecibosIDs.Count > 0)
                {
                    this.DataAccesor.Delete<ConceptosReciboNomina>(Conceptos);
                    this.DataAccesor.Delete<ReciboNomina>(ReciboNomina);
                }


                var Empleados = (from item in this.DataAccesor.GetQueryable()
                                select new NominaModel { 
                                    EmpleadoID = item.EmpleadoID,
                                    Fecha = fecha,
                                    totalDeposito = 0,
                                    conceptos = new List<Conceptos>(),
                                    SueldoBase = item.SueldoBase,
                                    Prestamos = item.Prestamos,
                                    DeduccionDesayuno = item.DeduccionDesayuno,
                                    DeduccionAhorro = item.DeduccionAhorro,
                                    TarjetaGasolina = item.TarjetaGasolina
                                }).ToList();

                Empleados.ForEach(x =>
                {
                    Listconceptos = new List<Conceptos>();
                    conceptos = new Conceptos();

                    conceptos.importe = x.SueldoBase;
                    conceptos.descripcion = "Ingreso Base";
                    conceptos.naturaleza = Enumeradores.naturalezaConcepto.percepciones;
                    Listconceptos.Add(conceptos);

                    conceptos = new Conceptos();
                    conceptos.importe = x.Prestamos;
                    conceptos.descripcion = "Prestamo";
                    conceptos.naturaleza = Enumeradores.naturalezaConcepto.percepciones;
                    Listconceptos.Add(conceptos);

                    conceptos = new Conceptos();
                    conceptos.importe = x.DeduccionDesayuno;
                    conceptos.descripcion = "Deduccion Desayuno";
                    conceptos.naturaleza = Enumeradores.naturalezaConcepto.deducciones;
                    Listconceptos.Add(conceptos);

                    conceptos = new Conceptos();
                    conceptos.importe = x.DeduccionAhorro;
                    conceptos.descripcion = "Deduccion Ahorro";
                    conceptos.naturaleza = Enumeradores.naturalezaConcepto.deducciones;
                    Listconceptos.Add(conceptos);                    

                    conceptos = new Conceptos();
                    conceptos.importe = x.TarjetaGasolina;
                    conceptos.descripcion = "Tarjeta Gasolina";
                    conceptos.naturaleza = Enumeradores.naturalezaConcepto.deducciones;
                    Listconceptos.Add(conceptos);

                    x.conceptos = Listconceptos;

                });


                return Empleados;
            }
        }

    }

    public abstract class DataAccessorEmpleados : DataAccessorBase<Empleados>
    {

    }
}
