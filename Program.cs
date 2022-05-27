using VendingMachine.Models.Machines;
using VendingMachine.Models.Money;
using VendingMachine.Models.Products;

namespace VendingMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Machine vendingMachine = new Machine();
            setupMachine(vendingMachine);

            bool exit = false;
            while(!exit)
            {
                // Todo: remove gibberish

                //Console.WriteLine();
                //Console.WriteLine("This is all coins that this Vending Machine accept as payment.");
                //vendingMachine.ListAllCoins().ForEach(coin => { Console.Write($"{coin._value} "); });
                //Console.WriteLine();
                //Console.WriteLine("This is all cash bills that this Vending Machine accept as payment.");
                //vendingMachine.ListAllCoins().ForEach(cashBills => { Console.Write($"{cashBills._value} "); });

                printMenu(vendingMachine.ListAllProducts());
                sellProduct(vendingMachine);
                exit = true; // Todo: Need to put this "exit" thing like a switch case.
            }
        }

        private static void sellProduct(Machine vendingMachine) // Todo: Maybe recursion on this thing?
        {
            int userProductIdSelection = 0;
            int userQuantitySelection = 0;
            float totalCost = 0;
            string currency = "";

            Console.WriteLine("You can buy an item by typing the id.");
            int.TryParse(Console.ReadLine(), out userProductIdSelection);
            Console.WriteLine("Nice choise! How many do you want?");
            int.TryParse(Console.ReadLine(), out userQuantitySelection);
            (currency, totalCost) = vendingMachine.ProductCost(vendingMachine.ListAllProducts(), userProductIdSelection, userQuantitySelection);
            Console.WriteLine($"Will cost {currency}{totalCost * userQuantitySelection:0.00}.");
            Console.WriteLine($"You balance is: {vendingMachine.ShowBalance():0.00}");

            if (vendingMachine.ShowBalance() < totalCost)
            {
                var userDebit = totalCost - vendingMachine.ShowBalance();
                Console.WriteLine("Damn! I need some money!");
                Console.WriteLine($"You need to deposit at least: {userDebit}");
                Console.WriteLine("How much do you want to deposit?");
                float.TryParse(Console.ReadLine(), out float ammoutDeposited);
                vendingMachine.DepositMoney(ammoutDeposited);
                if(vendingMachine.ShowBalance() == totalCost)
                {
                    Console.WriteLine("Right ammout!");
                    Console.WriteLine("Thank you! I hope to see you again!");
                    vendingMachine.SellProduct(userProductIdSelection, userQuantitySelection);
                }
                else
                {
                    Console.WriteLine("You're rich! You gave me too much!");
                    Console.WriteLine($"There's your change: {vendingMachine.GiveChange(totalCost)}");
                    Console.WriteLine("Thank you! I hope to see you again!");
                }

                
            }
            else if (vendingMachine.ShowBalance() == totalCost)
            {
                Console.WriteLine("Right ammout!");
                Console.WriteLine("Thank you! I hope to see you again!");
                vendingMachine.SellProduct(userProductIdSelection, userQuantitySelection);
            }
            else
            {
                Console.WriteLine("You're rich! You gave me too much!");
                Console.WriteLine($"There's your change: {vendingMachine.GiveChange(totalCost)}");
                Console.WriteLine("Thank you! I hope to see you again!");
            }
        }

        private static void printMenu(List<Product> products)
        {
            Console.WriteLine("This is the vending maXXXine 2000 5.8 supercharger");
            Console.WriteLine("bip... bop...");
            Console.WriteLine("What you want to buy?");
            Console.WriteLine();
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}","ID", "Name", "Price", "Quantity");
            products.ForEach(product => Console.WriteLine($"{product._id,-3} {product._name,-20} {product._currency}{product._price,-10:0.00} {product._quantity,-10}"));
            Console.WriteLine();
        }

        private static void setupMachine(Machine machine)
        {
            // Add first products
            machine.AddProduct(new Product(1, "rice 10kg", 25.50f, 10, "R$"));
            machine.AddProduct(new Product(2, "beans 1kg", 7.00f, 10, "R$"));
            machine.AddProduct(new Product(3, "pasta 1kg", 5.00f, 10, "R$"));
            machine.AddProduct(new Product(4, "soda can 250ml", 4.25f, 10, "R$"));
            machine.AddProduct(new Product(5, "coke 350ml", 5.50f, 10, "R$"));

            // Add all coins and cash notes possible as payment
            machine.AddCoin(new Coin(0.25f));
            machine.AddCoin(new Coin(0.50f));
            machine.AddCoin(new Coin(1.00f));
            machine.AddCoin(new Coin(2.00f));
            machine.AddCashBill(new Cash(2.00f));
            machine.AddCashBill(new Cash(5.00f));
            machine.AddCashBill(new Cash(10.00f));
        }
    }
}