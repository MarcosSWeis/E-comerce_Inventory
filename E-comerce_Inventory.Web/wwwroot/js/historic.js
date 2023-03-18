let dataTable;

$(document).ready(function () {
    fetch("/Inventory/Inventory/GetHistoric").then((res )=> {
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
                url: '/Inventory/Inventory/GetHistoric',
            },
            columns:
                [
                    {
                        data: "initialDate",
                        width: "15%",
                        render: function (data) {
                            let date = new Date(data)
                            return date.toLocaleString()
                        }

                    },                   
                    {
                        data: "finalDate",
                        width: "15%",
                        render: function (data) {
                            let date = new Date(data)
                            return date.toLocaleString()
                        }
                    },   
                    { data: "store.name", width: "40%" },                   
                    {
                        data: "userAplication",
                        width: "10%",
                        render: function (data) {
                            let name = data.name;
                            let lastName = data.lastName;
                            return `${name} ${lastName}`;
                        }

                    },
                    {
                        data: "id",
                        render: function (data) {
                            return `
                                <div class="text-center">
                                    <a href="/Inventory/Inventory/DetailHistoric/${data}" class="btn btn-info text-white" style="cursor:pointer">
                                        Detalle
                                    </a>                                 
                                </div>
                               `
                        },
                        width: "20%"
                    }
                ]
        })

}

