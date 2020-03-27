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
   
}
BuscarReciboEmpleado = function (periodoID, empleadoID) {

    llamarServicio('Empleado/BuscarReciboEmpleadoPorPeriodo', { EmpleadoID: empleadoID, PeriodoID: periodoID }, function (datos) {

        obtenerEmpleados();

    });
}

CargarEventos = function () {

    $(document).off("click", "#btnBuscarRecibo").on("click", "#btnBuscarRecibo", function (e) {

        var solicitud = {
            EmpleadoID: EmpleadoLogeadoID,
            PeriodoID: parseInt($("#txtPeriodoEmpleado").val())
        }

        llamarServicio('Empleado/BuscarReciboEmpleadoPorPeriodo', solicitud, function (datos) {

            if (datos)
            {
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