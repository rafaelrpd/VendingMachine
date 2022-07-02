using WebApp.Data.Shared;
using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Data
{
    public class SeedData
    {
        public static void Initialize()
        {
            List<Machine> machineList = new List<Machine>()
            {
                new Machine()
                {
                    MachineId = Guid.NewGuid(),
                    MachineName = "Coke Machine",
                    MachineBalance = 0,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = Guid.NewGuid(),
                            ProductName = "Coke can",
                            ProductPrice = 2.5m,
                            ProductQuantity = 10
                        },
                        new Product()
                        {
                            ProductId = Guid.NewGuid(),
                            ProductName = "Water bottle",
                            ProductPrice = 1.0m,
                            ProductQuantity = 10
                        },
                    }
                },
                new Machine()
                {
                    MachineId = Guid.NewGuid(),
                    MachineName = "Pepsi Machine",
                    MachineBalance = 50m,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = Guid.NewGuid(),
                            ProductName = "Pepsi can",
                            ProductPrice = 2.55m,
                            ProductQuantity = 10
                        }
                    }
                }
            };
            //List<Machine> machineList = new List<Machine> { };
            List<Product> productList = new List<Product> { };
            SharedData.MachineList = machineList;
            SharedData.ProductList = productList;
        }
    }
}
