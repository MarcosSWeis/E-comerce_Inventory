using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserAplicationId { get; set; }

        [ForeignKey("UserAplicationId")]
        public UserAplication UserAplication { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }
        public string CodeShipping { get; set; }
        public string Carrier { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal OrderTotal { get; set; }

        public string OrderState { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }
        public DateTime PaymentDeadline { get; set; } //fecha del palzo a pagar

        //hay datos que no esytan completos en la tabla user , verificar en el controle que esten completos, de lo contrario redireccionar para compretar ,
        //o darle la opccion para fururas comrpas
        public string TransactionId { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Dni { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }



    }
}
