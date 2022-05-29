using VendingMachine.Models.Machines;
using VendingMachine.Models.Money;

namespace VendingMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int menuShowCounter = 0;
            int addMenuShowCounter = 0;
            int sellMenuShowCounter = 0;
            Machine vendingMachine = new Machine();
            ShowMenu(menuShowCounter, vendingMachine);

            bool exit = false;
            while(!exit)
            {
                
                int.TryParse(Console.ReadLine(), out int _menuSelection);

                switch (_menuSelection)
                {
                    case 1:
                        AddMoney(addMenuShowCounter, vendingMachine);
                        Console.Clear();
                        ShowMenu(menuShowCounter, vendingMachine);
                        break;
                    case 2:
                        SellItem(sellMenuShowCounter, vendingMachine);
                        Console.Clear();
                        ShowMenu(menuShowCounter, vendingMachine);
                        break;
                    case 3:
                        GiveChange(vendingMachine);
                        Console.Clear();
                        ShowMenu(menuShowCounter, vendingMachine);
                        break;
                    default:
                        exit = true;
                        if (vendingMachine.Balance != 0)
                        {
                            GiveChange(vendingMachine);
                        }                        
                        break;
                }
            }
        }
        public static void ShowMenu(int menuShowCounter, Machine vendingMachine)
        {
            if (menuShowCounter == 0)
            {
                Console.WriteLine("This is the vending maXXXine 2000 5.8 supercharger menu!");
                Console.WriteLine("bip... bop...");
                Console.WriteLine();
                Console.WriteLine("This is my product list! Enjoy it!");
                Console.WriteLine();
                menuShowCounter++;
            }
            ListAllProducts(vendingMachine);
            ShowBalance(vendingMachine);
            Console.WriteLine("What you want to do?");
            Console.WriteLine($"1 - Add money\n2 - Buy item\n3 - Get change\n4 - Exit");
            Console.WriteLine();
        }
        public static void ListAllProducts(Machine vendingMachine)
        {
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            vendingMachine.Products.ForEach(product => Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", product.Id, product.Name, product.Price, product.Quantity));
            Console.WriteLine();
        }
        public static void ShowBalance(Machine vendingMachine)
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"Current balance: {vendingMachine.Balance:C2}");
            Console.WriteLine();
        }
        public static void AddMoney(int addMenuShowCounter, Machine vendingMachine)
        {
            if (addMenuShowCounter == 0)
            {
                Console.Clear();
                Console.WriteLine("You can add the following as money:");
                Console.WriteLine();
                Console.Write("Coins of this value: ");
                foreach (var coin in vendingMachine.Money)
                {
                    if (coin.GetType() == typeof(Coin))
                    {
                        Console.Write("{0:C2} ", coin._value);
                    }
                }
                Console.WriteLine();
                Console.Write("Banknotes of this value: ");
                vendingMachine.Money.ForEach(banknote =>
                {
                    if (banknote.GetType() == typeof(Banknote))
                    {
                        Console.Write("{0:C2} ", banknote._value);
                    }
                });
                Console.WriteLine();
                Console.WriteLine();
                addMenuShowCounter++;
            }
            Console.Write("How much you want do deposit? ");
            float.TryParse(Console.ReadLine(), out float _userDepositValue);
            vendingMachine.UpdateBalance(_userDepositValue);
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"Current balance: {vendingMachine.Balance:C2}");
            Console.WriteLine();
            Console.Write("Do you want to deposit more? Y/N ");
            string _moreDeposits = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (_moreDeposits == "y")
            {
                AddMoney(addMenuShowCounter, vendingMachine);
            }
            else if (_moreDeposits != "y")
            {
                Console.WriteLine("Let's go back to Menu!!!");
                Console.WriteLine();
            }
        }

        public static void SellItem(int sellMenuShowCounter, Machine vendingMachine)
        {
            if (sellMenuShowCounter == 0)
            {
                Console.Clear();
                Console.WriteLine("I'm selling all of this.");
                Console.WriteLine();
                sellMenuShowCounter++;
            }
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"Current balance: {vendingMachine.Balance:C2}");
            Console.WriteLine("This is my product list.");
            Console.WriteLine();
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            vendingMachine.Products.ForEach(product => Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", product.Id, product.Name, product.Price, product.Quantity));
            Console.Write("Type the product ID: ");
            int.TryParse(Console.ReadLine(), out int _productId);
            Console.WriteLine();
            Console.Write("How many? ");
            int.TryParse(Console.ReadLine(), out int _productQuantity);
            Console.WriteLine();
            Console.WriteLine("You're buying this: ");
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", vendingMachine.Products[_productId - 1].Id, vendingMachine.Products[_productId - 1].Name, vendingMachine.Products[_productId - 1].Price, _productQuantity);
            Console.WriteLine($"Total : {(vendingMachine.Products[_productId - 1].Price * _productQuantity):C2}");
            Console.WriteLine();
            vendingMachine.Products[_productId - 1].Quantity -= _productQuantity;
            vendingMachine.UpdateBalance(-(vendingMachine.Products[_productId - 1].Price * _productQuantity));
            Console.Write("Do you want to buy more? Y/N ");
            string _buyMore = Console.ReadLine().ToLower();
            Console.WriteLine();
            if (_buyMore == "y")
            {
                SellItem(sellMenuShowCounter, vendingMachine);
            }
            else if (_buyMore != "y")
            {
                Console.WriteLine("Let's go back to Menu!!!");
                Console.WriteLine();
            }

        }

        public static void GiveChange(Machine vendingMachine)
        {
            Console.Clear();
            Console.WriteLine("You don't need change! The change is mmmiiiinnneee!!!");
            Console.WriteLine("Muhuahuahua");
            Console.WriteLine("#ImEvil");
            Console.WriteLine("You don't need change, right?");
            Console.WriteLine("Right? ...");
            Console.WriteLine("Come on! You know how's this days! I need to feed my family!");
            Console.WriteLine("Fine... fine...");
            Console.WriteLine($"You have {vendingMachine.Balance:C2}.");
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
                Console.WriteLine($"Your balance is now {(vendingMachine.Balance - _userTip):C2}.");
                vendingMachine.UpdateBalance(-_userTip);
                Console.WriteLine();
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's legen... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {vendingMachine.Balance:C2}.");
                Thread.Sleep(5000);
                vendingMachine.UpdateBalance(-vendingMachine.Balance);
            }
            else
            {
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's legen... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {vendingMachine.Balance:C2}.");
                Thread.Sleep(5000);
                vendingMachine.UpdateBalance(-vendingMachine.Balance);
            }
        }
    }
}