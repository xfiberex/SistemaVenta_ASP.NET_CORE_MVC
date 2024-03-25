// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';


// Bar Chart Example
let controlVenta = document.getElementById("charVentas");
let myBarChart = new Chart(controlVenta, {
  type: 'bar',
  data: {
    labels: ["06/07/2022", "07/07/2022", "08/07/2022", "09/07/2022","10/07/2022","11/07/2022", "12/07/2022"],
    datasets: [{
      label: "Cantidad",
      backgroundColor: "#4e73df",
      hoverBackgroundColor: "#2e59d9",
      borderColor: "#4e73df",
      data: [12,10,22,11,15,10,22],
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
