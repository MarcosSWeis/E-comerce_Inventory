using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Brand
    {
        [Key]
        [Required(ErrorMessage = "El campo id es requerido requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es nombre es requerido")]
        [MaxLength(50,ErrorMessage = "El campo nombre solo puede contener 50 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es estad requerido")]
        public bool State { get; set; }
    }
}
