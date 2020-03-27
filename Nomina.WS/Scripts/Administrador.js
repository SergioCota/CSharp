var UrlPrefix = '../../api/'
var EmpleadoID = 0;
var EmpleadoLogeadoID = 0;

$(document).ready(function () {

    EmpleadoLogeadoID = obtenerValorParametro("ID");
    buscarEmpladoLogeado();
    cargarControles();
    CargarEventos();


});
buscarEmpladoLogeado = function () {

    llamarServicio('Empleado/BuscarEmpleado', { EmpleadoID: EmpleadoLogeadoID }, function (datos) {

        $("#NombreEmpleado").text("EMPLEADO: " + datos.NombreCompleto);

    });
}

cargarControles = function () {
    $("#txtIngreso").kendoNumericTextBox({
        decimals: 2
    });
    $("#txtPrestamos").kendoNumericTextBox({
        decimals: 2
    });
    $("#txtDeduccionDesayuno").kendoNumericTextBox({
        decimals: 2
    });

    $("#txtDeduccionAhorro").kendoNumericTextBox({
        decimals: 2
    });
    $("#txtGasolina").kendoNumericTextBox({
        decimals: 2
    });

    var dataEstatus = [
        { text: "Inactivo", value: "0" },
        { text: "Activo", value: "1" },
    ];

    // create DropDownList from input HTML element
    $("#txtActivo").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dataEstatus,
        index: 1
    });

    var dataRol = [
        { text: "Administrador", value: "1" },
        { text: "Empleado", value: "2" }
    ];

    // create DropDownList from input HTML element
    $("#txtRole").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dataRol,
        index: 1
    });

    var dataPeriodo = [
        { text: "Enero", value: 1 },
        { text: "Febrero", value: 2 },
        { text: "Marzo", value: 3 },
        { text: "Abril", value: 4 },
        { text: "Mayo", value: 5 },
        { text: "Junio", value: 6 },
        { text: "Julio", value: 7 },
        { text: "Agosto", value: 8 },
        { text: "Septiembre", value: 9 },
        { text: "Octubre", value: 10 },
        { text: "Noviembre", value: 11 },
        { text: "Diciembre", value: 12 }
    ];

    var fecha = new Date();
    var mesActual = fecha.getMonth();

    // create DropDownList from input HTML element
    $("#txtPeriodoEmpleado").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dataPeriodo,
        change: BuscarReciboEmpleado,
        index: mesActual
    });

    // create DropDownList from input HTML element
    $("#txtPeriodo").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: dataPeriodo,
        index: mesActual
    });
}

CargarEventos = function () {

    $(document).on("click", '#btnMiNomina', function () {

        $('#tabMiNomina').show(500);
        $('#tabEmplados').hide(500);
        $('#tabGenerarNominas').hide(500);

    });
    $(document).on("click", '#btnEmpleados', function () {

        $('#tabMiNomina').hide(500);
        $('#tabEmplados').show(500);
        $('#tabGenerarNominas').hide(500);

        obtenerEmpleados();

    });
    $(document).on("click", '#GenerarNominas', function () {


        $('#tabMiNomina').hide(500);
        $('#tabEmplados').hide(500);
        $('#tabGenerarNominas').show(500);

    });

    $(document).off("click", "#btnAgregarEmpleado").on("click", "#btnAgregarEmpleado", function (e) {
        limpiar();
        $("#btnRegistarEmpleado").val("Registrar")
        $('#NuevoEmpleado').show(500);
        $('#ListaEmpleados').hide(500);

    });
    $(document).off("click", "#btnRegresar").on("click", "#btnRegresar", function (e) {

        $('#NuevoEmpleado').hide(500);
        $('#ListaEmpleados').show(500);


    });
    $(document).off("click", "#btnRegistarEmpleado").on("click", "#btnRegistarEmpleado", function (e) {

        $("#NuevoEditarEmpleado").text("Datos nuevo empleado");

        if (validarDatosEmpleado()) {

            var Item = {
                EmpleadoID: EmpleadoID,
                Nombre: $("#txtNombre").val().trim(),
                ApellidoPaterno: $("#txtApPaterno").val().trim(),
                ApellidoMaterno: $("#txtApMaterno").val().trim(),
                //FechaIngreso: $("#txtDeduccionDesayuno").val().trim(),
                SueldoBase: $("#txtIngreso").val().trim(),
                Prestamos: $("#txtPrestamos").val().trim(),
                DeduccionDesayuno: $("#txtDeduccionDesayuno").val().trim(),
                DeduccionAhorro: $("#txtDeduccionAhorro").val().trim(),
                TarjetaGasolina: $("#txtGasolina").val().trim(),
                Activo: parseInt($("#txtActivo").val()),
                tipoEmpleadoID: $("#txtRole").val(),
                Email: $("#txtEmail").val().trim(),
                Password: $("#txtPassword").val().trim()
            }
            var url = "";

            if ($("#btnRegistarEmpleado").val() == "Registrar") {
                url = "Administrador/RegistarEmpleado"
            }
            else if ($("#btnRegistarEmpleado").val() == "Editar") {
                url = "Administrador/EditarEmpleado"
            }

            llamarServicio(url, Item, function () {

                obtenerEmpleados();
                limpiar();
                $('#NuevoEmpleado').hide(500);
                $('#ListaEmpleados').show(500);
            });
        }

    });

    $(document).off("click", "#btnGenerarNominas").on("click", "#btnGenerarNominas", function (e) {

        llamarServicio('Administrador/GenerarNomina', { periodoID: parseInt($("#txtPeriodo").val()) }, function () {

            obtenerEmpleados();
            limpiar();
            $('#tabGenerarNominas').hide(500);
            $('#tabEmplados').show(500);
        });

    });

    $(document).off("click", "#btnBuscarRecibo").on("click", "#btnBuscarRecibo", function (e) {

        var solicitud = {
            EmpleadoID: EmpleadoLogeadoID,
            PeriodoID: parseInt($("#txtPeriodoEmpleado").val())
        }

        llamarServicio('Empleado/BuscarReciboEmpleadoPorPeriodo', solicitud, function (datos) {

            if (datos) {
                TablaRecibo(datos);
                $("#Recibos").show(500);
            } else {
                $("#Recibos").hide(500);
            }           

        });

    });

    $(document).off("click", "#btnLogOut").on("click", "#btnLogOut", function (e) {

        parent.location = "Login.aspx";

    });

}
obtenerEmpleados = function () {

    llamarServicio('Administrador/ObtenerEmpleados', {}, function (datos) {

        ListarEmpleados(datos);

    });
}

TablaRecibo = function (Conceptos) {

    var conceptosPercepciones = Conceptos["Percepciones"];
    var conceptosDeducciones = Conceptos["Deducciones"];
    var totalRecibo = [];
    var itemTotal = {
        descripcion: "DEPOSITADO",
        importe: Conceptos.TotalRecibo
    }
    totalRecibo.push(itemTotal);


    if ($('#TablaMisRecibosPercepciones').data('kendoGrid')) {
        $('#TablaMisRecibosPercepciones').data('kendoGrid').destroy();
        $('#TablaMisRecibosPercepciones').html('');
    }
    if ($('#TablaMisRecibosDeducciones').data('kendoGrid')) {
        $('#TablaMisRecibosDeducciones').data('kendoGrid').destroy();
        $('#TablaMisRecibosDeducciones').html('');
    }
    if ($('#TablaMisRecibosTotal').data('kendoGrid')) {
        $('#TablaMisRecibosTotal').data('kendoGrid').destroy();
        $('#TablaMisRecibosTotal').html('');
    }


    //var dataSource = new kendo.data.DataSource({
    //    data: conceptosPercepciones,
    //})

    $('#TablaMisRecibosPercepciones').kendoGrid({
        dataSource: conceptosPercepciones,
        sortable: false,
        scrollable: true,
        resizable: true,
        columns:
            [
                {
                    field: 'descripcion',
                    title: 'Concepto',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'importe',
                    title: 'Importe',
                    width: 20,
                    template: "#:kendo.format('{0:c}', importe) #",
                    attributes:
                    {
                        style: "text-align: right;"
                    }
                }]
    });
    $('#TablaMisRecibosDeducciones').kendoGrid({
        dataSource: conceptosDeducciones,
        sortable: false,
        scrollable: true,
        resizable: true,
        columns:
            [
                {
                    field: 'descripcion',
                    title: 'Concepto',
                    width: 20,

                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'importe',
                    title: 'Importe',
                    width: 20,
                    template: "#:kendo.format('{0:c}', importe) #",
                    attributes:
                    {
                        style: "text-align: right;"
                    }
                }]
    });
    $('#TablaMisRecibosTotal').kendoGrid({
        dataSource: totalRecibo,
        sortable: false,
        scrollable: true,
        resizable: true,
        columns:
            [
                {
                    field: 'descripcion',
                    title: "",
                    width: 20,

                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'importe',
                    title: "",
                    width: 20,
                    template: "#:kendo.format('{0:c}', importe) #",
                    attributes:
                    {
                        style: "text-align: right;"
                    }
                }]
    });
}

ListarEmpleados = function (Lista) {

    if ($('#TablaEmpleados').data('kendoGrid')) {
        $('#TablaEmpleados').data('kendoGrid').destroy();
        $('#TablaEmpleados').html('');
    }

    var dataSource = new kendo.data.DataSource({
        data: Lista,
    })

    $('#TablaEmpleados').kendoGrid({
        dataSource: dataSource,
        sortable: false,
        scrollable: true,
        resizable: true,
        pageable: {
            pageSizes: true,
            pageSize: 10
        },
        columns:
            [
                {
                    field: 'Nombre',
                    title: 'Nombre',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'ApellidoPaterno',
                    title: 'Apellido paterno',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'ApellidoMaterno',
                    title: 'Apellido materno',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'FechaIngreso',
                    title: 'Fecha ingreso',
                    template: "#= kendo.toString(kendo.parseDate(FechaIngreso, 'yyyy-MM-dd'), 'dd/MM/yyyy') #",
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'Activo',
                    title: 'Estatus',
                    template: "#=Activo? 'Activo': 'Inactivo'#",
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'SueldoBase',
                    title: 'Sueldo base',
                    width: 20,
                    template: "#:kendo.format('{0:c}', SueldoBase) #",
                    attributes:
                    {
                        style: "text-align: right;"
                    }
                },
                {
                    field: 'tipoEmpleadoID',
                    title: 'Tipo empleado',
                    template: "#=tipoEmpleadoID == 1 ? 'Administrador': 'Empleado'#",
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'Email',
                    title: 'Email',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'Password',
                    title: 'Password',
                    width: 20,
                    attributes:
                    {
                        style: "text-align: left;"
                    }
                },
                {
                    field: 'EmpleadoID',
                    title: 'EmpleadoID',
                    hidden: true
                },
                {
                    title: "Acciones"
                    , template: kendo.template("#=ButtonsAccionesGridTotalAdeudo()#")
                    , width: 20
                    , attributes:
                    {
                        style: "text-align: center;"
                    }
                }
            ]
    });

    $(document).off("click", "#btnEditar").on("click", "#btnEditar", function (e) {
        $("#btnRegistarEmpleado").val("Editar")
        e.preventDefault();
        var Grid = $('#TablaEmpleados').data('kendoGrid');
        var dataItem = Grid.dataItem($(this).closest('tr'));

        EmpleadoID = dataItem.EmpleadoID;

        EditarEmpleado(dataItem);

    });

    $(document).off("click", "#btnRemove").on("click", "#btnRemove", function (e) {

        e.preventDefault();
        var Grid = $('#TablaEmpleados').data('kendoGrid');
        var dataItem = Grid.dataItem($(this).closest('tr'));

        EliminarEmpleado(dataItem.EmpleadoID);
    });

}

EditarEmpleado = function (DatosEmpleado) {

    $("#txtNombre").val(DatosEmpleado.Nombre);
    $("#txtApPaterno").val(DatosEmpleado.ApellidoPaterno);
    $("#txtApMaterno").val(DatosEmpleado.ApellidoMaterno);
    $('#txtIngreso').data('kendoNumericTextBox').value(DatosEmpleado.SueldoBase);
    $('#txtPrestamos').data('kendoNumericTextBox').value(DatosEmpleado.Prestamos);
    $('#txtDeduccionDesayuno').data('kendoNumericTextBox').value(DatosEmpleado.DeduccionDesayuno);
    $('#txtDeduccionAhorro').data('kendoNumericTextBox').value(DatosEmpleado.DeduccionAhorro);
    $('#txtGasolina').data('kendoNumericTextBox').value(DatosEmpleado.TarjetaGasolina);
    $("#txtActivo").data("kendoDropDownList").select(DatosEmpleado.Activo);
    $("#txtRole").data("kendoDropDownList").select(DatosEmpleado.tipoEmpleadoID - 1);
    $("#txtEmail").val(DatosEmpleado.Email);
    $("#txtPassword").val(DatosEmpleado.Password);

    $("#NuevoEditarEmpleado").text("Actualizar datos empleado");


    $('#NuevoEmpleado').show(500);
    $('#ListaEmpleados').hide(500);
}

EliminarEmpleado = function (EmpleadoID) {

    llamarServicio('Administrador/EliminarEmpleado', { EmpleadoID: EmpleadoID }, function (datos) {

        obtenerEmpleados();

    });
}

BuscarReciboEmpleado = function (periodoID, empleadoID) {

    llamarServicio('Empleado/BuscarReciboEmpleadoPorPeriodo', { EmpleadoID: empleadoID, PeriodoID: periodoID }, function (datos) {

        obtenerEmpleados();

    });
}


ButtonsAccionesGridTotalAdeudo = function () {
    var lHtml = '';
    lHtml += '<div class="span2" style="margin-bottom: 5px;">'
    lHtml += '<input type="button" class="btn btn-success" id="btnEditar" value="Actualizar" />'
    lHtml += '</div>'
    lHtml += '<div class="span2">'
    lHtml += '<input type="button" class="btn btn-danger" id="btnRemove" value="Eliminar" />'
    lHtml += '</div>'
    return lHtml;
}


llamarServicio = function (url, data, p_callBack) {

    $.ajax({
        //async: o.async,
        type: 'POST',
        url: UrlPrefix + url,
        dataType: 'json',
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {

            if (data.IsValid) {

                if (p_callBack) {

                    toastr.success(data.Message);
                    p_callBack(data.Data);

                }

            } else {

                if (p_callBack) {

                    toastr.warning(data.Message)
                    p_callBack(data.Data);
                }
            }

        },
        error: function (data) {
            toastr.error(data.responseText)
        }
    });
}

validarDatosEmpleado = function () {

    if ($("#txtNombre").val().trim() == "") {
        toastr.warning("Favor de capturar nombre")
        $("#txtNombre").focus();
        return false;
    }
    
    if ($("#txtIngreso").val().trim() == "") {
        toastr.warning("Favor de capturar ingreso")
        $("#txtIngreso").focus();
        return false;
    }

    if ($("#txtActivo").val().trim() == "") {
        toastr.warning("Favor de capturar activo")
        $("#txtActivo").focus();
        return false;
    }
    if ($("#txtRole").val().trim() == "") {
        toastr.warning("Favor de capturar rol")
        $("#txtRole").focus();
        return false;
    }
    if ($("#txtEmail").val().trim() == "") {
        toastr.warning("Favor de capturar email")
        $("#txtEmail").focus();
        return false;
    }
    if ($("#txtPassword").val().trim() == "") {
        toastr.warning("Favor de capturar password")
        $("#txtPassword").focus();
        return false;
    }

    return true;

}

limpiar = function () {
    $("#txtNombre").val("");
    $("#txtApPaterno").val("");
    $("#txtApMaterno").val("");
    $("#txtIngreso").val("");
    $("#txtPrestamos").val("");
    $("#txtDeduccionDesayuno").val("");
    $("#txtDeduccionAhorro").val("");
    $("#txtGasolina").val("");
    $("#txtEmail").val("");;
    $("#txtPassword").val("");
}

function obtenerValorParametro(sParametroNombre) {
    var sPaginaURL = window.location.search.substring(1);
    var sURLVariables = sPaginaURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParametro = sURLVariables[i].split('=');
        if (sParametro[0] == sParametroNombre) {
            return sParametro[1];
        }
    }
    return null;
}