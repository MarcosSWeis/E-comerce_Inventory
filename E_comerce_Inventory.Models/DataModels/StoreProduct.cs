using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class StoreProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

        [Required]
        public int ProdutId { get; set; }

        [ForeignKey("ProdutId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }




    }
}
