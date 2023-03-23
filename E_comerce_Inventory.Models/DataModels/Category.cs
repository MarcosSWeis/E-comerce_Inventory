using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Category
    {
        [Key]
        [Required(ErrorMessage = "El campo Id es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [MaxLength(50,ErrorMessage = "El campo solo puede tener hasta 50 carateres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo estado es requerido")]
        public bool State { get; set; }
    }
}
