using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models.Products;


namespace WebApp.Models.Machines
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string MachineName { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MachineBalance { get; set; } = decimal.Zero;

        public List<Product> MachineProductList { get; set; } = default!;
    }
}