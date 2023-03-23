using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class ShoppingCart
    {

        public ShoppingCart()
        {
            Quantity = 1;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string UserAplicationId { get; set; }

        [ForeignKey("UserAplicationId")]
        public UserAplication UserAplication { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required, Range(1,1000,ErrorMessage = "El valor debe estar entre 1 y 1000")]
        public int Quantity { get; set; }

        [NotMapped]
        public decimal Price { get; set; }

    }
}
