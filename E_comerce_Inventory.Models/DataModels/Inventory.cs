using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public string UserAplicationId { get; set; }

        [ForeignKey("UserAplicationId")]
        public UserAplication UserAplication { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = false,DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime InitialDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false,DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime FinalDate { get; set; }

        [Required]
        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        public bool State { get; set; }

    }
}
