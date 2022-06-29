using WebApp.Data.Shared;
using WebApp.Models.Machines;
using WebApp.Models.Products;

namespace WebApp.Data
{
    public class SeedData
    {
        public void Initialize()
        {
            List<Machine> list = new List<Machine>()
            {
                new Machine()
                {
                    MachineId = 0,
                    MachineName = "Coke Machine",
                    MachineBalance = 0,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = 0,
                            ProductName = "Coke can",
                            ProductPrice = 2.5m,
                            ProductQuantity = 10
                        },
                        new Product()
                        {
                            ProductId = 1,
                            ProductName = "Water bottle",
                            ProductPrice = 1.0m,
                            ProductQuantity = 10
                        },

                    }
                },
                new Machine()
                {
                    MachineId = 1,
                    MachineName = "Pepsi Machine",
                    MachineBalance = 50m,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = 0,
                            ProductName = "Pepsi can",
                            ProductPrice = 2.55m,
                            ProductQuantity = 10
                        }
                    }
                }
            };
            SharedData.Current = list[0];
        }
        public static List<Machine> ListView()
        {
            return new List<Machine>()
            {
                new Machine()
                {
                    MachineId = 0,
                    MachineName = "Coke Machine",
                    MachineBalance = 0,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = 0,
                            ProductName = "Coke can",
                            ProductPrice = 2.5m,
                            ProductQuantity = 10
                        },
                        new Product()
                        {
                            ProductId = 1,
                            ProductName = "Water bottle",
                            ProductPrice = 1.0m,
                            ProductQuantity = 10
                        },

                    }
                },
                new Machine()
                {
                    MachineId = 1,
                    MachineName = "Pepsi Machine",
                    MachineBalance = 50m,
                    MachineProductList = new List<Product>
                    {
                        new Product()
                        {
                            ProductId = 0,
                            ProductName = "Pepsi can",
                            ProductPrice = 2.55m,
                            ProductQuantity = 10
                        }
                    }
                }
            };
        }
    }
}
