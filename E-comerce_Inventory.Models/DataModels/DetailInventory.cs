using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class DetailInventory
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int InventoryId { get; set; }

        [ForeignKey("InventoryId")]
        public Inventory Inventory { get; set; }

        [Required]
        public int ProducId { get; set; }

        [ForeignKey("ProducId")]
        public Product Product { get; set; }

        [Required]
        public int OldStock { get; set; }

        [Required]
        public int NewStock { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
