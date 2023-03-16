let dataTable;

$(document).ready(function () {
    fetch("/Inventory/Inventory/GetAll").then((res )=> {
        res.json().then((data )=> {
            console.log(data)
        })
    })
    //loadDataTable();
    //$("#tbl-data").children("tbody").css("color", "black")
    //let tblData = document.getElementById("tbl-data_wrapper");
    //let divs = tblData.getElementsByTagName("div");
    //for (let i = 0; i < divs.length; i++) {
    //    divs[i].style.color = "white"
    //}
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
                url: '/Inventory/Inentory/GetAll',
            },
            columns:
                [
                    { data: "name", width: "40%" },   
                    { data: "name", width: "40%" },                   
                    { data: "name", width: "40%" },                   
                    { data: "name", width: "40%" },                   

                 
                   
                   
                ]
        })

}

function Delete(url) {
    debugger
    Swal.fire({
        title: "Esta seguro de que desea eliminar esta categoria",
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