using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.ViewModels
{
    public class DataPaymentUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo es requerido"), StringLength(15,ErrorMessage = "Longitud maxima de 15 caracteres")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "El campo es requerido"), StringLength(50,ErrorMessage = "Longitud maxima de 50 caracteres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo es requerido"), StringLength(100,ErrorMessage = "Longitud maxima de 100 caracteres")]
        public string City { get; set; }

        [Required(ErrorMessage = "El campo es requerido"), StringLength(10,ErrorMessage = "Longitud maxima de 10 caracteres")]
        public string PostalCode { get; set; }

        //[Required(ErrorMessage = "El campo es requerido"), StringLength(10,ErrorMessage = "Longitud maxima de 15 caracteres")]
        //public string PhoneNumber { get; set; }

        //[Required()]
        //public bool Saveindb { get; set; }
    }
}
