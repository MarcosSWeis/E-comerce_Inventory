using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class BaseEntity
    {
        [Key]
        [Required(ErrorMessage = "El campo es requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public bool State { get; set; }
    }
}
