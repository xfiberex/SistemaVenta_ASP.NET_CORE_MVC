// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example
let controlProducto = document.getElementById("charProductos");
let myPieChart = new Chart(controlProducto, {
  type: 'doughnut',
  data: {
    labels: ["Producto A", "Producto B", "Producto C", "Producto D"],
    datasets: [{
      data: [55, 30, 15, 10],
      backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc',"#FF785B"],
      hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf',"#FF5733"],
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
