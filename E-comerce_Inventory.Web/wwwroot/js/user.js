let dataTable;

$(document).ready(function () {
      fetch("/Admin/User/GetAll").then((res)=> {
          res.json().then((data) => {
              console.log(data)
          });
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
                url: '/Admin/User/GetAll',

            },
            columns:
                [                    
                    { data: "user.name", width: "20%" },
                    { data: "user.lastName", width: "20%" },
                    { data: "user.email", width: "20%" },
                    { data: "role", width: "20%" },
                                                        
                   
                    {
                        data: {
                            id: "user.id",
                            lockoutEnd: "user.lockoutEnd"
                        },
                        render: function (data) {

                           
                            var dateNow = new Date().getTime();
                            var Block = new Date(data.user.lockoutEnd).getTime();
                            
                            if (Block > dateNow)//user bloqued
                            {
                                return `
                                <div class="text-center">                                  
                                    <a  onclick=BlockUnblock('${data.user.id}')  class="btn btn-danger text-white" style="cursor:pointer">
                                       <i class="bi bi-lock-fill"></i>  Bloqueado
                                    </a >
                                </div>
                               `
                            } else {
                                return `
                                <div class="text-center">                                  
                                    <a  onclick=BlockUnblock('${data.user.id}')  class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="bi bi-unlock-fill"></i>Permitido
                                    </a >
                                </div>
                               `
                            }
                        
                        },
                        width: "20%"
                    }
                ]
                            
        })

}



function BlockUnblock(id)
{
      debugger
            $.ajax({
                type: "POST",
                url: `/Admin/User/BlockUnblok/${id}`,
                data: JSON.stringify(id),
                contentType : "application/json",
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