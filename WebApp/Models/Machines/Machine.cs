using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Models.Products;


namespace WebApp.Models.Machines
{
    public class Machine
    {
        //[Key]
        [Display(Name = "Machine ID")]
        public Guid MachineId { get; set; } = Guid.NewGuid();

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Display(Name = "Machine Name")]
        public string MachineName { get; set; } = string.Empty;

        //[DataType(DataType.Currency)]
        //[Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Machine Balance")]
        public decimal MachineBalance { get; set; } = decimal.Zero;

        [Display(Name = "Machine Product List")]
        public List<Product> MachineProductList { get; set; } = new List<Product> { };
    }
}