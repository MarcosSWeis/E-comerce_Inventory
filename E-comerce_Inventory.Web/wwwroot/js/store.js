let dataTable;

$(document).ready(function () {
    loadDataTable();     
    $("#tbl-data").children("tbody").css("color", "black")

 });

function loadDataTable() {
    //ese DataTable esta relacionado a el script de data table en _Layout
    //hacemos una solicitud por medio de ajax para obtener los datos de las tiendas, el pedido se hace a "{area}/{controller}/{method}" => "/Admin/Store/GetAll"
    //fetch("/Admin/Store/GetAll").then((reponse) => {
    //    reponse.json().then((data)=>console.log(data))
    //})
    
    dataTable = $("#tbl-data").DataTable(
        {
            ajax: {
                url: '/Admin/Store/GetAll',

            },
            columns:
                [
                    { data: "name", width: "20%" },
                    { data: "description", width: "40%" },
                    {
                        data: "state",
                        render:  data => data ? "Activo" : "Inactivo",
                        width: "20%"
                    },
                //esta es la parte en la que se muetran los botne de editar y eliminar 
                //con render le indicamos que quremos crear codigo html, le paso a data, ya que data es la que contiene el ID
                    {
                    data: "id",
                    render: function (data) {
                        return `
                                <div class="text-center">
                                    <a href="/Admin/Store/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                        <i class="bi bi-pencil-square"></i>
                                    </a>
                                    <a href="" class="btn btn-danger text-white" style="cursor:pointer">
                                       <i class="bi bi-trash3"></i>
                                    </a>
                                </div>
                               `
                    },
                    width:"20%"
                }
            ]
        })
   
}

