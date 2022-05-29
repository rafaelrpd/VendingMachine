using VendingMachine.Models.Money;
using VendingMachine.Models.Products;

namespace VendingMachine.Models.Machines
{
    internal class Machine
    {
        private List<Product> Products { get; set; }
        private float Balance { get; set; }
        private List<IMoney> Money { get; set; }
        private int MenuShowCounter { get; set; }
        private int AddMenuShowCounter { get; set; }
        private int SellMenuShowCounter { get; set; }

        public Machine()
        {
            Products = new List<Product>();
            Balance = 0;
            Money = new List<IMoney>();
            MenuShowCounter = 0;
            AddMenuShowCounter = 0;
            SellMenuShowCounter = 0;
            SetupMachine();
        }
        public void ShowMenu()
        {
            if (MenuShowCounter == 0)
            {
                Console.WriteLine("This is the vending maXXXine 2000 5.8 supercharger menu!");
                Console.WriteLine("bip... bop...");
                Console.WriteLine();
                Console.WriteLine("This is my product list! Enjoy it!");
                Console.WriteLine();
                MenuShowCounter++;
            }            
            ListAllProducts();
            ShowBalance();
            Console.WriteLine("What you want to do?");
            Console.WriteLine($"1 - Add money\n2 - Buy item\n3 - Get change\n4 - Exit");
            Console.WriteLine();
        }

        public void AddMoney()
        {
            if (AddMenuShowCounter == 0)
            {
                Console.Clear();
                Console.WriteLine("You can add the following as money:");
                Console.WriteLine();
                Console.Write("Coins of this value: ");
                foreach (var coin in Money)
                {
                    if (coin.GetType() == typeof(Coin))
                    {
                        Console.Write("{0:C2} ", coin._value);
                    }
                }
                Console.WriteLine();
                Console.Write("Banknotes of this value: ");
                Money.ForEach(banknote =>
                {
                    if (banknote.GetType() == typeof(Banknote))
                    {
                        Console.Write("{0:C2} ", banknote._value);
                    }
                });
                Console.WriteLine();
                Console.WriteLine();
                AddMenuShowCounter++;
            }
            Console.Write("How much you want do deposit? ");
            float.TryParse(Console.ReadLine(), out float _userDepositValue);
            Balance += _userDepositValue;
            Console.WriteLine();
            ShowBalance();
            Console.WriteLine();
            Console.Write("Do you want to deposit more? Y/N ");
            string _moreDeposits = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (_moreDeposits == "y")
            {
                AddMoney();
            } 
            else if (_moreDeposits != "y")
            {
                Console.WriteLine("Let's go back to Menu!!!");
                Console.WriteLine();
            }
        }

        public void SellItem()
        {
            if (SellMenuShowCounter == 0)
            {
                Console.Clear();
                Console.WriteLine("I'm selling all of this.");
                Console.WriteLine();
                SellMenuShowCounter++;
            }
            ShowBalance();
            Console.WriteLine("This is my product list.");
            Console.WriteLine();
            ListAllProducts();
            Console.Write("Type the product ID: ");
            int.TryParse(Console.ReadLine(), out int _productId);
            Console.WriteLine();
            Console.Write("How many? ");
            int.TryParse(Console.ReadLine(), out int _productQuantity);
            Console.WriteLine();
            Console.WriteLine("You're buying this: ");
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", Products[_productId - 1]._id, Products[_productId - 1]._name, Products[_productId - 1]._price, _productQuantity);
            Console.WriteLine($"Total : {(Products[_productId - 1]._price * _productQuantity):C2}");
            Console.WriteLine();
            Products[_productId - 1]._quantity -= _productQuantity;
            Balance -= (Products[_productId - 1]._price * _productQuantity);
            Console.Write("Do you want to buy more? Y/N ");
            string _buyMore = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (_buyMore == "y")
            {
                SellItem();
            }
            else if (_buyMore != "y")
            {
                Console.WriteLine("Let's go back to Menu!!!");
                Console.WriteLine();
            }

        }

        public void GiveChange()
        {
            Console.Clear();
            Console.WriteLine("You don't need change! The change is mmmiiiinnneee!!!");
            Console.WriteLine("Muhuahuahua");
            Console.WriteLine("#ImEvil");
            Console.WriteLine("You don't need change, right?");
            Console.WriteLine("Right? ...");
            Console.WriteLine("Come on! You know how's this days! I need to feed my family!");
            Console.WriteLine("Fine... fine...");
            Console.WriteLine($"You have {Balance:C2}.");
            Console.WriteLine("At least give me a tip!");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Do you want to give a tip? Y/N ");
            string _userSelection = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (_userSelection == "y")
            {
                Console.WriteLine("How much you will tip?");
                float.TryParse(Console.ReadLine(), out float _userTip);
                Console.WriteLine($"You tipped the machine for {_userTip:C2}");
                Console.WriteLine($"Your balance is now {(Balance - _userTip):C2}.");
                Balance -= _userTip;
                Console.WriteLine();
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's lege... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {Balance:C2}.");
                Thread.Sleep(5000);
                Balance = 0;
            }
            else
            {
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's lege... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {Balance:C2}.");
                Thread.Sleep(5000);
                Balance = 0;
            }
        }

        private void ShowBalance()
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"Current balance: {Balance:C2}");
            Console.WriteLine();
        }

        private void ListAllProducts()
        {
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            Products.ForEach(product => Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", product._id, product._name, product._price, product._quantity));
            Console.WriteLine();
        }

        private void SetupMachine()
        {
            // Add first products
            Products.Add(new Product(1, "rice 10kg", 25.50f, 10, "R$"));
            Products.Add(new Product(2, "beans 1kg", 7.00f, 10, "R$"));
            Products.Add(new Product(3, "pasta 1kg", 5.00f, 10, "R$"));
            Products.Add(new Product(4, "soda can 250ml", 4.25f, 10, "R$"));
            Products.Add(new Product(5, "coke 350ml", 5.50f, 10, "R$"));

            // Add all coins and cash notes possible as payment
            IMoney[] coinArray = { new Coin(0.25f), new Coin(0.50f), new Coin(1.00f), new Coin(2.00f) };
            Money.AddRange(coinArray);

            IMoney[] banknoteArray = { new Banknote(2.00f), new Banknote(5.00f), new Banknote(10.00f), new Banknote(25.00f), new Banknote(50.00f), new Banknote(100.00f), new Banknote(200.00f) };
            Money.AddRange(banknoteArray);
        }
    }
}