<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrador.aspx.cs" Inherits="Nomina.WS.Administrador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard administrativo</title>

    <link rel="stylesheet" type="text/css" href="css/main.css">
    <link rel="stylesheet" type="text/css" href="css/toastr.css">
    <link rel="stylesheet" type="text/css" href="css/bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/toastr.css">
    <link rel="stylesheet" type="text/css" href="css/kendo.bootstrap.min.css">
    <link rel="stylesheet" type="text/css" href="css/kendo.common.min.css">
    <link rel="stylesheet" type="text/css" href="css/kendo.rtl.min.css">
    <link rel="stylesheet" type="text/css" href="css/uniform.min.css">
    <link rel="stylesheet" type="text/css" href="css/dataviz.min.css">
    <link rel="stylesheet" type="text/css" href="css/dataviz.uniform.min.css">

</head>
<body>

    <div id="tabDashboardAdministrativo" style="margin-top: 30px; margin-left:20%">

        <div class="row-fluid"> 
            <div class="span8">
            <h1 id="NombreEmpleado" style="margin-bottom: 50px"></h1>
                </div>
            <div class="span4">
                <input id="btnLogOut" class="btn btn-danger" type="button" style="margin-top: 20px;" value="Log Out">
            </div>
        </div>        

        <div class="row-fluid">
            <div class="span12">
                <div class="row-fluid">
                    <div class="span1">
                        <div class="controls">
                            <input type="button" class="btn btn-primary" id="btnMiNomina" value="Mi nómina" />
                        </div>
                    </div>
                    <div class="span1">
                        <div class="controls">
                            <input type="button" class="btn btn-primary" id="btnEmpleados" value="Empleados" />
                        </div>
                    </div>
                    <div class="span1">
                        <div class="controls">
                            <input type="button" class="btn btn-primary" id="GenerarNominas" value="Generar Nóminas" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="tabMiNomina" style="margin-top: 30px">

        <div id="" style="margin-top: 100px; margin-left:20%">
            <h1>Lista de recibos de nómina</h1>

            <div class="controls">
                <div class="span4">
                    <label for="Periodo">Período</label>
                    <input id="txtPeriodoEmpleado" value="" style="width: 73%;">
                </div>
                <div class="span4">
                    <input id="btnBuscarRecibo" class="btn btn-primary" type="button" style="margin-top: 25px;" value="Buscar">
                </div>
            </div>
            <div id="Recibos" class="controls">
                <div class="controls">
                    <div class="span6">
                        <div id="TablaMisRecibosPercepciones">
                        </div>
                        <div id="TablaMisRecibosDeducciones">
                        </div>
                        <div id="TablaMisRecibosTotal">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--</div>--%>

    <div id="tabEmplados" hidden style="margin-top: 30px">

        <div id="ListaEmpleados" style="margin-top: 100px">
            <h1>Lista Empleados</h1>

            <div class="controls">
                <input id="btnAgregarEmpleado" class="btn btn-primary" type="button" style="margin-bottom: 5px;" value="Agregar">
            </div>
            <div id="TablaEmpleados">
            </div>
        </div>

        <div id="NuevoEmpleado" style="margin-left:20%" class="span12" hidden>

            <h1 id="NuevoEditarEmpleado">Datos nuevo empleado</h1>

            <div class="row-fluid">
                <input id="btnRegresar" class="btn" type="button" value="Regresar">
            </div>

            <div class="row-fluid" style="margin-top: 10px">
                <div class="span4">
                    <label for="Nombre">Capture el nombre</label>
                    <input id="txtNombre" type="text" placeholder="Enter nombre">
                </div>
                <div class="span4">
                    <label for="ApPaterno">Apellido paterno</label>
                    <input id="txtApPaterno" type="text" placeholder="Enter apellido paterno">
                </div>
                <div class="span4">
                    <label for="ApMaterno">Apellido materno</label>
                    <input id="txtApMaterno" type="text" placeholder="Enter apellido materno">
                </div>
            </div>

            <div class="row-fluid">
                <div class="span4">
                    <label for="Ingreso">Ingreso Base</label>
                    <input id="txtIngreso" type="number" title="currency" value="" min="0" style="width: 73%" placeholder="Enter ingreso" />
                </div>
                <div class="span4">
                    <label for="Prestamo">Prestamo</label>
                    <input id="txtPrestamos" type="number" title="currency" value="" min="0" style="width: 73%" placeholder="Enter prestamo" />
                </div>
                <div class="span4">
                    <label for="DeduccionDesayuno">Deduccion desayuno</label>
                    <input id="txtDeduccionDesayuno" type="number" title="currency" value="" min="0" style="width: 73%" placeholder="Enter deducion desayuno" />
                </div>
            </div>

            <div class="row-fluid">
                <div class="span4">
                    <label for="Deduccion Ahorro">Deduccion ahorro</label>
                    <input id="txtDeduccionAhorro" type="number" title="currency" value="" min="0" style="width: 73%" placeholder="Enter deduccion ahorro" />
                </div>
                <div class="span4">
                    <label for="txtGasolina">Tarjeta Gasolina</label>
                    <input id="txtGasolina" type="number" title="currency" value="" min="0" style="width: 73%" placeholder="Enter deducion tarjeta gasolina" />
                </div>
                <div class="span4">
                    <label for="Estatus">Estatus</label>
                    <input id="txtActivo" value="1" style="width: 73%;">
                </div>
            </div>

            <div class="row-fluid">
                <div class="span4">
                    <label for="Role">Rol(empleado/admin)</label>
                    <input id="txtRole" value="2" style="width: 73%;">
                </div>
                <div class="span4">
                    <label for="Email">Email</label>
                    <input id="txtEmail" type="text" placeholder="Enter Email">
                </div>

                <div class="span4">
                    <label for="Password">Password</label>
                    <input id="txtPassword" type="text" placeholder="Enter Password">
                </div>
            </div>
            <div class="row-fluid">
                <div class="span4">
                    <input id="btnRegistarEmpleado" style="margin-top: 23px;" class="btn" type="button" value="Registrar">
                </div>
            </div>
        </div>


    </div>

    <div id="tabGenerarNominas" hidden>

        <div id="" style="margin-top: 100px; margin-left:20%"">
            <h1>Generar Nóminas</h1>

            <div class="controls">
                <div class="span4">
                    <label for="Periodo">Período</label>
                    <input id="txtPeriodo" value="" style="width: 73%;">
                </div>
                <div class="span4">
                    <input id="btnGenerarNominas" class="btn btn-primary" type="button" style="margin-top: 28px;" value="Generar">
                </div>
            </div>
            <div id="TablaEmpleadosGenerarNominas">
            </div>
        </div>

    </div>



    <script src="Scripts/libs/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/libs/toastr.min.js" type="text/javascript"></script>
    <script src="Scripts/Administrador.js" type="text/javascript"></script>

    <script src="Scripts/libs/bootstrap-multiselect.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.min.js" type="text/javascript"></script>

    <script src="Scripts/libs/kendo.all.min.js" type="text/javascript"></script>
</body>
</html>
