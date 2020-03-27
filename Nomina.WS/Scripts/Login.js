var UrlPrefix = '../../api/'

$(document).ready(function () {

    $("#txtEmail").focus();

    var input = document.getElementById("txtPassword");

    // Execute a function when the user releases a key on the keyboard
    input.addEventListener("keyup", function (event) {
        // Number 13 is the "Enter" key on the keyboard
        if (event.keyCode === 13) {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            document.getElementById("btnLogin").click();
        }
    });

    $(document).on("click", '#btnLogin', function () {

        if ($("#txtEmail").val().trim() == "") {
            toastr.warning("Capture un email")
            return
        }
        if ($("#txtPassword").val().trim() == "") {
            toastr.warning("Capture un password")
            return
        }

        var solicitud =
        {
            Email: $("#txtEmail").val().trim(),
            Password: $("#txtPassword").val().trim()
        }

        $.ajax({
            //async: o.async,
            type: 'POST',
            url: UrlPrefix + "Main/Login",
            dataType: 'json',
            data: JSON.stringify(solicitud),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {

                if (data.IsValid) {

                    toastr.success(data.Message)

                    setTimeout(
                        function () {
                            if (data.Data.TipoEmpleadoID == 1) {

                                parent.location = "Administrador.aspx?ID=" + data.Data.EmpleadoID

                            } else {
                                parent.location = "Empleado.aspx?ID=" + data.Data.EmpleadoID
                            }
                        }, 2000);


                } else {
                    toastr.warning(data.Message)
                    setTimeout(
                        function () {

                        }, 2000);
                }

            },
            error: function (data) {

                toastr.error(data.responseText)
            }
        });

    })

})



