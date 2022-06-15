using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models.Currency;
using WebApp.Models.Products;


namespace WebApp.Models.Machines
{
    public class Machine
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string MachineName { get; set; } = string.Empty;

        public List<Product> ProductList { get; set; } = new List<Product>();

        public List<Money> MoneyList { get; set; } = new List<Money>();

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }
    }
}