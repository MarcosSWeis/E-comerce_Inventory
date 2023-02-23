using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Store
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50,ErrorMessage = "El campo nombre solo puede tener como maximo 50 carateres")]
        [Display(Name = "Nombre")]//este es el nombre que se usara en la vista
        public string Name { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage = "El campo descripción solo puede tener como maximo 100 carateres")]
        public string Description { get; set; }
        [Required]
        public bool State { get; set; }
    }
}
