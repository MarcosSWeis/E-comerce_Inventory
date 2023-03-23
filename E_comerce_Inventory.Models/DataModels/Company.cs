using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Required, MaxLength(50)]
        public string Country { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(80)]
        public string Address { get; set; }

        [Required, MaxLength(80)]
        public string Phone { get; set; }

        public int StoreSaleId { get; set; }

        [ForeignKey("StoreSaleId")]
        public Store Store { get; set; }

        public string LogoUrl { get; set; }

    }
}
