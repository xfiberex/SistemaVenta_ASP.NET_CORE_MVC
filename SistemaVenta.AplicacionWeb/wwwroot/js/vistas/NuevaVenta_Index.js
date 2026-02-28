
let ValorImpuesto = 0;

function sanitizeImageUrl(url) {
    if (typeof url !== "string") {
        return "";
    }

    try {
        const parsedUrl = new URL(url, window.location.origin);
        return ["http:", "https:"].includes(parsedUrl.protocol) ? parsedUrl.href : "";
    }
    catch {
        return "";
    }
}

$(document).ready(function () {

    secureFetch("/Venta/ListaTipoDocumentoVenta")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTipoDocumentoVenta").append(
                        $("<option>").val(item.idTipoDocumentoVenta).text(item.descripcion)
                    )
                })
            }
        })

    secureFetch("/Negocio/Obtener")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.estado) {

                const d = responseJson.objeto;

                $("#inputGroupSubTotal").text(`Sub Total - ${d.simboloMoneda}`)
                $("#inputGroupITBIS").text(`ITBIS (${d.porcentajeImpuesto}%) - ${d.simboloMoneda}`)
                $("#inputGroupTotal").text(`Total - ${d.simboloMoneda}`)

                ValorImpuesto = parseFloat(d.porcentajeImpuesto)
            }
        })

    $("#cboBuscarProducto").select2({
        ajax: {
            url: "/Venta/ObtenerProductos",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                return {
                    busqueda: params.term
                };
            },
            processResults: function (data) { 
                return {
                    results: data.map((item) => ({
                        id: item.idProducto,
                        text: item.descripcion,
                        marca: item.marca,
                        categoria: item.nombreCategoria,
                        urlImagen: item.urlImagen,
                        precio: parseFloat(item.precio)
                    }))
                };
            },
        },
        language: "es",
        placeholder: 'Buscar productos...',
        minimumInputLength: 1,
        templateResult: formatoResultados
    });
})

function formatoResultados(data) {

    if (data.loading)
        return data.text;

    const contenedor = $("<table>").attr("width", "100%");
    const fila = $("<tr>");
    const columnaImagen = $("<td>").css("width", "60px");
    const imagen = $("<img>")
        .css({ height: "77px", width: "77px", marginRight: "10px" })
        .attr("src", sanitizeImageUrl(data.urlImagen));

    const columnaTexto = $("<td>");
    const marca = $("<p>").css({ fontWeight: "bolder", margin: "2px" }).text(data.marca || "");
    const descripcion = $("<p>").css("margin", "2px").text(`Descripción: ${data.text || ""}`);
    const categoriaPrecio = $("<p>")
        .css("margin", "2px")
        .text(`Categoría: ${data.categoria || ""} - Precio: ${data.precio || 0} RD$`);

    columnaImagen.append(imagen);
    columnaTexto.append(marca, descripcion, categoriaPrecio);
    fila.append(columnaImagen, columnaTexto);
    contenedor.append(fila);

    return contenedor;
}

$(document).on("select2:open", function () {
    document.querySelector(".select2-search__field").focus();
})

let ProductosParaVenta = [];
$("#cboBuscarProducto").on("select2:select", function (e) {
    const data = e.params.data;

    let producto_encontrado = ProductosParaVenta.filter(p => p.idProducto == data.id)

    if (producto_encontrado.length > 0) {
        $("#cboBuscarProducto").val("").trigger("change")
        toastr.warning("", "El producto ya fue agregado")
        return false
    }

    swal({
        title: data.marca,
        text: data.text,
        imageUrl: sanitizeImageUrl(data.urlImagen),
        type: "input",
        showCancelButton: true,
        closeOnConfirm: false,
        inputPlaceholder: "Ingrese la cantidad"

    },
        function (valor) {

            if (valor === false) return false;

            if (valor === "") {
                toastr.warning("", "Necesita ingresa la cantidad")
                return false;
            }

            if (isNaN(parseInt(valor))) {
                toastr.warning("", "Debe ingresar un valor numerico")
                return false;
            }

            let producto = {
                idProducto: data.id,
                marcaProducto: data.marca,
                descripcionProducto: data.text,
                categoriaProducto: data.categoria,
                cantidad: parseInt(valor),
                precio: data.precio.toString(),
                total: (parseFloat(valor) * data.precio).toString()
            }
            ProductosParaVenta.push(producto)

            mostrarProducto_Precios();
            $("#cboBuscarProducto").val("").trigger("change")
            swal.close()
        }
    )
})

function mostrarProducto_Precios() {

    let total = 0;
    let itbis = 0;
    let subtotal = 0;
    let porcentaje = ValorImpuesto / 100;

    $("#tbProducto tbody").html("")

    ProductosParaVenta.forEach((item) => {

        total = total + parseFloat(item.total)

        $("#tbProducto tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        $("<i>").addClass("fas fa-trash-alt")
                    ).data("idProducto", item.idProducto)
                ),
                $("<td>").text(item.descripcionProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.total)
            )
        )
    })

    subtotal = total;
    itbis = total * porcentaje;
    total = itbis + subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2))
    $("#txtITBIS").val(itbis.toFixed(2))
    $("#txtTotal").val(total.toFixed(2))

}

$(document).on("click", "button.btn-eliminar", function () {

    const _idproducto = $(this).data("idProducto")

    ProductosParaVenta = ProductosParaVenta.filter(p => p.idProducto != _idproducto);

    mostrarProducto_Precios();

})

$("#btnTerminarVenta").click(function () {

    if (ProductosParaVenta.length < 1) {
        toastr.warning("", "Debe ingresar los productos")
        return;
    }

    if ($("#txtDocumentoCliente").val().trim() === "") {
        toastr.warning("", "Debe ingresar el documento del cliente");
        return;
    }

    if ($("#txtNombreCliente").val() === "") {
        toastr.warning("", "Debe ingresar el nombre del cliente")
        return;
    }

    const vmDetalleVenta = ProductosParaVenta;
    const venta = {
        idTipoDocumentoVenta: $("#cboTipoDocumentoVenta").val(),
        documentoCliente: $("#txtDocumentoCliente").val(),
        nombreCliente: $("#txtNombreCliente").val(),
        subTotal: $("#txtSubTotal").val(),
        impuestoTotal: $("#txtITBIS").val(),
        total: $("#txtTotal").val(),
        DetalleVenta: vmDetalleVenta
    }

    $("#btnTerminarVenta").LoadingOverlay("show")

    secureFetch("/Venta/RegistrarVenta", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(venta)
    })
        .then(response => {
            $("#btnTerminarVenta").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.estado) {
                ProductosParaVenta = [];
                mostrarProducto_Precios();

                $("#txtDocumentoCliente").val("")
                $("#txtNombreCliente").val("")
                $("#cboTipoDocumentoVenta").val($("#cboTipoDocumentoVenta option:first").val())

                swal("Registrado!", `Numero de venta: ${responseJson.objeto.numeroVenta}`, "success")
            }
            else {
                swal("Lo sentimos!", "No se pudo registrar la venta", "error")
            }
        })
})
