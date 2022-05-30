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
                        vendingMachine.GiveChange();
                        break;
                    default:
                        exit = true;                     
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
            Console.WriteLine($"1 - Add money\n2 - Buy iteme\n3 - Exit");
            Console.WriteLine();
        }
        public static void ListAllProducts(Machine vendingMachine)
        {
            Console.WriteLine("{0,-3} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Price", "Quantity");
            int productIndex = 0;
            foreach (var product in vendingMachine.Products)
            {
                Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", (productIndex + 1), product.Name, product.Price, product.Quantity);
                productIndex++;
            }
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
            int productIndex = 0;
            foreach (var product in vendingMachine.Products)
            {
                Console.WriteLine("{0,-3} {1,-20} {2,-10:C2} {3,-10}", (productIndex + 1), product.Name, product.Price, product.Quantity);
                productIndex++;
            }

            Console.Write("Type the product ID: ");
            int.TryParse(Console.ReadLine(), out int userProductIndexInput);
            Console.WriteLine();

            try
            {
                vendingMachine.SellItem(userProductIndexInput - 1);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Item ID doesn't exist.");
                Console.WriteLine();
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Not enough money to buy. Please add money first.");
                Console.WriteLine();
            }
        }
    }
}