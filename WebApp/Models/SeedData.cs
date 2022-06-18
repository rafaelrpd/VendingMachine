using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Models
{
    public static class SeedData
    {
        public static Machine Initialize()
        {
            Machine machine = new Machine()
            {
                MachineId = 0,
                MachineName = "Coke Machine",
                MachineBalance = 0,
                MachineProductList = new List<Product>{
                    new Product()
                    {
                        ProductId = 0,
                        ProductName = "Coke can",
                        ProductPrice = 2.5m,
                        ProductQuantity = 10
                    }
                }
            };

            return machine;
        }
    }
}
