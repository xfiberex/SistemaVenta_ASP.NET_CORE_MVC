
$(document).ready(function () {

    $(".container-fluid").LoadingOverlay("show")

    fetch("/Home/ObtenerUsuario")
        .then(response => {
            $(".container-fluid").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.estado) {

                const d = responseJson.objeto

                $("#imgFoto").attr("src", d.urlFoto)
                $("#txtNombre").val(d.nombre)
                $("#txtCorreo").val(d.correo)
                $("#txTelefono").val(d.telefono)
                $("#txtRol").val(d.nombreRol)
                
            }
            else {
                swal("Lo sentimos!", responseJson.mensaje, "error")
            }
        })
})

$("#btnGuardarCambios").click(function () {

    if ($("#txtCorreo").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Correo")
        $("#txtCorreo").focus()
        return;
    }

    if ($("#txTelefono").val().trim() == "") {
        toastr.warning("", "Debe completar el campo: Telefono")
        $("#txTelefono").focus()
        return;
    }

    swal({
        title: "¿Desea guardar los cambios?",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-primary",
        confirmButtonText: "Si, guardar",
        cancelButtonClass: "btn-secondary",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (respuesta) {
            if (respuesta) {
                $(".showSweetAlert").LoadingOverlay("show")

                let modelo = {
                    correo: $("#txtCorreo").val().trim(),
                    telefono: $("#txTelefono").val().trim()
                }

                fetch("/Home/GuardarPerfil", {
                    method: "POST",
                    headers: { "Content-Type": "application/json; charset=utf-8" },
                    body: JSON.stringify(modelo)
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide")
                        return response.ok ? response.json() : Promise.reject(response)
                    })
                    .then(responseJson => {
                        if (responseJson.estado) {

                            swal("Listo!", "Los cambios fueron guardados", "success")
                        }
                        else {
                            swal("Lo sentimos!", responseJson.mensaje, "error")
                        }
                    })
            }
        }
    )
})

$("#btnCambiarClave").click(function () {

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe completar el campo: "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }

    if ($("#txtClaveNueva").val().trim() != $("#txtConfirmarClave").val().trim()) {
        toastr.warning("", "Las contraseñas no coinciden")
        $("#txtClaveNueva").focus()
        return;
    }

    let modelo = {
        claveActual: $("#txtClaveActual").val().trim(),
        claveNueva: $("#txtClaveNueva").val().trim()
    }

    fetch("/Home/CambiarClave", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(modelo)
    })
        .then(response => {
            $(".showSweetAlert").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.estado) {

                swal("Listo!", "Su contraseña fue actualizada", "success")
                $("input.input-validar").val("");
            }
            else {
                swal("Lo sentimos!", responseJson.mensaje, "error")
            }
        })
})