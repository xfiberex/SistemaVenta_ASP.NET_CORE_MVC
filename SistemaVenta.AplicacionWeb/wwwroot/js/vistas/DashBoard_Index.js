
$(document).ready(function () {

    $("div.container-fluid").LoadingOverlay("show")

    fetch("/DashBoard/ObtenerResumen")
        .then(response => {
            $("div.container-fluid").LoadingOverlay("hide")
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {

            if (responseJson.estado) {

                // Mostrar datos para la tarjetas
                let d = responseJson.objeto

                $("#totalVenta").text(d.totalVentas)
                $("#totalIngresos").text(d.totalIngresos)
                $("#totalProductos").text(d.totalProductos)
                $("#totalCategorias").text(d.totalCategorias)

                // Obtener los textos y valores para nuestros graficos
                let barchart_labels;
                let barchart_data;

                if (d.ventasUltimaSemana.length > 0) {
                    barchart_labels = d.ventasUltimaSemana.map((item) => { return item.fecha })
                    barchart_data = d.ventasUltimaSemana.map((item) => { return item.total })
                }
                else {
                    barchart_labels = ["Sin resultados"]
                    barchart_data = [0]
                }

                // Obtener los textos y valores para nuestros graficos de pie
                let piechart_labels;
                let piechart_data;

                if (d.productosTopUltimaSemana.length > 0) {
                    piechart_labels = d.productosTopUltimaSemana.map((item) => { return item.producto })
                    piechart_data = d.productosTopUltimaSemana.map((item) => { return item.cantidad })
                }
                else {
                    piechart_labels = ["Sin resultados"]
                    piechart_data = [0]
                }

                // Bar Chart Example
                let controlVenta = document.getElementById("chartVentas");
                let myBarChart = new Chart(controlVenta, {
                    type: 'bar',
                    data: {
                        labels: barchart_labels,
                        datasets: [{
                            label: "Cantidad",
                            backgroundColor: "#4e73df",
                            hoverBackgroundColor: "#2e59d9",
                            borderColor: "#4e73df",
                            data: barchart_data,
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        legend: {
                            display: false
                        },
                        scales: {
                            xAxes: [{
                                gridLines: {
                                    display: false,
                                    drawBorder: false
                                },
                                maxBarThickness: 50,
                            }],
                            yAxes: [{
                                ticks: {
                                    min: 0,
                                    maxTicksLimit: 5
                                }
                            }],
                        },
                    }
                });

                // Pie Chart Example
                let controlProducto = document.getElementById("chartProductos");
                let myPieChart = new Chart(controlProducto, {
                    type: 'doughnut',
                    data: {
                        labels: piechart_labels,
                        datasets: [{
                            data: piechart_data,
                            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                            hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: true
                        },
                        cutoutPercentage: 80,
                    },
                });
            }

        })
})

function formatearNumero(numero) {
    // Convertir el número a cadena
    let numeroStr = numero.toString();

    // Separar la parte entera de la parte decimal
    let [parteEntera, parteDecimal] = numeroStr.split(".");

    // Agregar comas a la parte entera
    parteEntera = parteEntera.replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    // Si hay una parte decimal, agregar un punto y la parte decimal
    if (parteDecimal) {
        return `${parteEntera}.${parteDecimal}`;
    } else {
        return parteEntera;
    }
}