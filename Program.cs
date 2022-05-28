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
            vendingMachine.ShowMenu();

            bool exit = false;
            while(!exit)
            {
                
                int.TryParse(Console.ReadLine(), out int _menuSelection);

                switch (_menuSelection)
                {
                    case 1:
                        vendingMachine.AddMoney();
                        Console.Clear();
                        vendingMachine.ShowMenu();
                        break;
                    case 2:
                        vendingMachine.SellItem();
                        Console.Clear();
                        vendingMachine.ShowMenu();
                        break;
                    case 3:
                        exit = true;
                        // Todo: close program and give change
                        break;
                    default:
                        exit = false;
                        break;
                }
            }
        }
    }
}