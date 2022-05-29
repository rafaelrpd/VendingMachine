using VendingMachine.Models.Machines;
using VendingMachine.Models.Money;

namespace VendingMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Machine vendingMachine = new Machine();

            bool exit = false;
            while(!exit)
            {
                ShowMenu(vendingMachine);
                int.TryParse(Console.ReadLine(), out int _menuSelection);

                switch (_menuSelection)
                {
                    case 1:
                        AddMoney(vendingMachine);
                        break;
                    case 2:
                        SellItem(vendingMachine);
                        break;
                    case 3:
                        GiveChange(vendingMachine);
                        break;
                    default:
                        exit = true;
                        if (vendingMachine.Balance != 0)
                        {
                            // Todo: Doesn't work, because AddBalance only add and so, it just accept positive values.
                            GiveChange(vendingMachine);
                        }                        
                        break;
                }
            }
        }
        public static void ShowMenu(Machine vendingMachine)
        {
            Console.WriteLine("This is the vending maXXXine 2000 5.8 supercharger menu!");
            Console.WriteLine("bip... bop...");
            Console.WriteLine();
            Console.WriteLine("This is my product list! Enjoy it!");
            Console.WriteLine();
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
        public static void AddMoney(Machine vendingMachine)
        {

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

            Console.Write("How much you want do deposit? ");
            float.TryParse(Console.ReadLine(), out float _userDepositValue);
            Console.WriteLine();
            try
            {
                vendingMachine.AddBalance(_userDepositValue);
            }
            catch (Exception)
            {

                Console.WriteLine("You can't deposit no values, zeros, negative numbers or words");
                Console.WriteLine();
            }
        }

        public static void SellItem(Machine vendingMachine)
        {
            Console.WriteLine("I'm selling all of this.");
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"Current balance: {vendingMachine.Balance:C2}");
            Console.WriteLine("This is my product list.");
            Console.WriteLine();
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            vendingMachine.Products.ForEach(product => Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", product.Id, product.Name, product.Price, product.Quantity));


            Console.Write("Type the product ID: ");
            int.TryParse(Console.ReadLine(), out int productId);
            Console.WriteLine();
            Console.Write("How many? ");
            int.TryParse(Console.ReadLine(), out int _productQuantity);
            Console.WriteLine();


            try
            {
                Console.WriteLine("You're buying this: ");
                Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
                Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", vendingMachine.Products[productId - 1].Id, vendingMachine.Products[productId - 1].Name, vendingMachine.Products[productId - 1].Price, _productQuantity);
                Console.WriteLine($"Total : {(vendingMachine.Products[productId - 1].Price * _productQuantity):C2}");
                Console.WriteLine();
                vendingMachine.UpdateProductQuantity(productId, _productQuantity);
                vendingMachine.AddBalance(-(vendingMachine.Products[productId - 1].Price * _productQuantity));
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Item ID doesn't exist.");
                Console.WriteLine();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Item quantity should be greater than 0 and less of equal than total quantity.");
            }
        }
        public static void GiveChange(Machine vendingMachine)
        {
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
                // Todo: Doesn't work, because AddBalance only add and so, it just accept positive values.
                vendingMachine.AddBalance(-_userTip);
                Console.WriteLine();
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's legen... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {vendingMachine.Balance:C2}.");
                Thread.Sleep(5000);
                vendingMachine.AddBalance(-vendingMachine.Balance);
            }
            else
            {
                Console.WriteLine("Processing change...");
                Console.WriteLine("It's legen... wait for it... dary!");
                Console.WriteLine($"You got back all your money! {vendingMachine.Balance:C2}.");
                Thread.Sleep(5000);
                // Todo: Doesn't work, because AddBalance only add and so, it just accept positive values.
                vendingMachine.AddBalance(-vendingMachine.Balance);
            }
        }
    }
}