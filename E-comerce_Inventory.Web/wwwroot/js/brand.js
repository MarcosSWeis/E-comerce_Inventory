let dataTable;

$(document).ready(function () {
    loadDataTable();
    $("#tbl-data").children("tbody").css("color", "black")
    let tblData = document.getElementById("tbl-data_wrapper");
    let divs = tblData.getElementsByTagName("div");
    for (let i = 0; i < divs.length; i++) {
        divs[i].style.color = "white"
    }
});

function loadDataTable() {

    dataTable = $("#tbl-data").DataTable(
        {
            language: {
                lengthMenu: "Mostrar _MENU_ Registros Por Pagina",
                zeroRecords: "Ningun Registro",
                info: "Mostrar page _PAGE_ de _PAGES_",
                infoEmpty: "no hay registros",
                infoFiltered: "(filtered from _MAX_ total registros)",
                search: "Buscar",
                paginate: {
                    first: "Primero",
                    last: "Último",
                    next: "Siguiente",
                    previous: "Anterior"
                }

            },
            ajax: {
                url: '/Admin/Brand/GetAll',

            },
            columns:
                [
                    { data: "name", width: "40%" },
                    {
                        data: "state",
                        render: data => data ? "Activo" : "Inactivo",
                        width: "40%"
                    },
                    //esta es la parte en la que se muetran los botne de editar y eliminar 
                    //con render le indicamos que quremos crear codigo html, le paso a data, ya que data es la que contiene el ID
                    {
                        data: "id",
                        render: function (data) {
                            return `
                                <div class="text-center">
                                    <a href="/Admin/Brand/Upsert/${data}" class="btn btn-info text-white" style="cursor:pointer">
                                        <i class="bi bi-pencil-square"></i>
                                    </a>
                                    <a  onclick=Delete("/Admin/Brand/Delete/${data}")  class="btn btn-danger text-white" style="cursor:pointer">
                                       <i class="bi bi-trash3"></i>
                                    </a >
                                </div>
                               `
                        },
                        width: "20%"
                    }
                ]
        })

}

function Delete(url) {
    debugger
    Swal.fire({
        title: "Esta seguro de que desea eliminar esta marca",
        text: "Este registro no se podra recuperar",
        icon: "warning",
        confirmButtonText: 'Si, eliminar!',
        showCancelButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                //data es la respuesta que me da el mentodo al que llame
                success: function (data) {
                    if (data.success) {
                        //envio la notidicacion con el mensaje
                        toastr.success(data.message);
                        //recargo la datatable
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}