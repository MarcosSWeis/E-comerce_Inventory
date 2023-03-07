using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_comerce_Inventory.Models.DataModels
{
    public class Product
    {
        [Key, Required(ErrorMessage = "Campo requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido"), MaxLength(50,ErrorMessage = "El maximo de caracteres aceptados son 50")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Campo requerido"), MaxLength(30,ErrorMessage = "El maximo de caracteres aceptados son 30")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Campo requerido"), MaxLength(200,ErrorMessage = "El maximo de caracteres aceptados son 200")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Campo requerido"), Range(1,double.MaxValue), Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Column(TypeName = "money")]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }

        #region Navigation Properties
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        #endregion

        #region Recursion
        public int? ParentId { get; set; }
        public virtual Product Parent { get; set; }
        #endregion
    }
}
