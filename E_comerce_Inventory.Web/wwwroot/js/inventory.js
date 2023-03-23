let dataTable;

$(document).ready(function () {
    fetch("/Inventory/Inventory/GetAll").then((res )=> {
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
                url: '/Inventory/Inventory/GetAll',
            },
            columns:
                [
                    { data: "store.name", width: "40%" },                   
                    { data: "product.title", width: "40%" },   
                    { data: "product.cost", width: "40%" },                   
                    { data: "quantity", width: "40%" }, 
                ]
        })

}

