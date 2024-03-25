// Call the dataTables jQuery plugin


$(document).ready(function() {
  $('#tbdata').DataTable({
    responsive: true,
    // "ajax": {
    //     "url": '',
    //     "type": "GET",
    //     "datatype": "json"
    // },
    // "columns": [
    //     { "data": "clave" },
    //     {
    //         "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
    //             '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
    //         "orderable": false,
    //         "searchable": false,
    //         "width": "80px"
    //     }
    // ],
    // order: [[0, "desc"]],
    dom:"Bfrtip",
    buttons:[
        {
            text: 'Exportar Excel',
            extend: 'excelHtml5',
            title: '',
            filename: 'Reporte Usuarios',
            exportOptions: {
                columns: [ 1,2 ]
            }
        },'pageLength'
    ],
    language:{
        url:"https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
    },
  });
});
