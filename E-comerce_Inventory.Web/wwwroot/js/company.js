let dataTable;

$(document).ready(function () {
    fetch("/Admin/Company/GetAll").then((res )=> {
        res.json().then((data )=> {
            console.log(data)
        })
    })
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
                url: '/Admin/Company/GetAll',

            },
            columns:
                [
                    { data: "name", width: "15%" },
                    { data: "description", width: "15%" },            
                    { data: "country", width: "15%" }, 
                    { data: "city", width: "15%" },
                    { data: "phone", width: "15%" },  
                    {
                        data: "id",
                        render: function (data) {
                            return `
                                <div class="text-center">
                                    <a href="/Admin/Company/Upsert/${data}" class="btn btn-info text-white" style="cursor:pointer">
                                        <i class="bi bi-pencil-square"></i>
                                    </a>
                                 </div>
                               `
                        },
                        width: "25%"
                    }
                ]
        })

}
