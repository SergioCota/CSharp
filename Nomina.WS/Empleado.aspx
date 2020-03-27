<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Empleado.aspx.cs" Inherits="Nomina.WS.Empleado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Empleado</title>
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

    <div id="tabMiNomina" style="margin-top: 30px; margin-left:20%">

        <div class="row-fluid"> 
            <div class="span8">
            <h1 id="NombreEmpleado" style="margin-bottom: 50px"></h1>
                </div>
            <div class="span4">
                <input id="btnLogOut" class="btn btn-danger" type="button" style="margin-top: 20px;" value="Log Out">
            </div>
        </div>

        <div id="" style="margin-top: 100px">
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

    <script src="Scripts/libs/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="Scripts/libs/toastr.min.js" type="text/javascript"></script>
    <script src="Scripts/Empleado.js" type="text/javascript"></script>

    <script src="Scripts/libs/bootstrap-multiselect.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.js" type="text/javascript"></script>
    <script src="Scripts/libs/bootstrap.min.js" type="text/javascript"></script>

    <script src="Scripts/libs/kendo.all.min.js" type="text/javascript"></script>

</body>
</html>
