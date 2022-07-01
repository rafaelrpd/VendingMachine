using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.Products
{
    public class Product
    {
        [Key]
        [Display(Name = "Product ID")]
        public Guid ProductId { get; set; } = Guid.NewGuid();

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Product Price")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Product Quantity")]
        public int ProductQuantity { get; set; }
    }
}