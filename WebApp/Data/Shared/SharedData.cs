using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Data.Shared
{
    public class SharedData
    {
        public static List<Machine> MachineList { get; set; } = default!;
        public static List<Product> ProductList { get; set; } = default!;
    }
}
