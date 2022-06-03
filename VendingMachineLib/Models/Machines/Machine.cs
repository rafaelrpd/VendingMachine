using VendingMachine.Models.Money;
using VendingMachine.Models.Products;

namespace VendingMachine.Models.Machines
{
    internal class Machine
    {
        public List<Product> Products { get; private set; }
        public float Balance { get; private set; }
        public List<IMoney> Money { get; private set; }

        public Machine()
        {
            Products = new List<Product>();
            Balance = 0;
            Money = new List<IMoney>();
            SetupMachine();
        }

        public void AddBalance(float value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Value is invalid", "value");
            }
            Balance += value;
        }
        public void GiveChange()
        {
            Console.WriteLine($"There's your change: {Balance:C2}");
            Console.WriteLine("Have a nice day!");
            Console.WriteLine();
            Balance = 0;
        }
        
        public void SellItem(int index)
        {
            if (Balance < Products[index].Price)
            {
                throw new ArgumentException("Not enough balance to buy item.");
            }
            if (Products.ElementAtOrDefault(index) == null)
            {
                throw new IndexOutOfRangeException("Index choosed doesn't exist.");
            }
            Products[index].Quantity -= 1;
            Balance -= Products[index].Price;
            if (Products[index].Quantity <= 0)
            {
                Products.RemoveAt(index);
            }
        }

        private void SetupMachine()
        {
            // Add first products
            Products.Add(new Product("rice 10kg", 25.50f, 1));
            Products.Add(new Product("beans 1kg", 7.00f, 10));
            Products.Add(new Product("pasta 1kg", 5.00f, 10));
            Products.Add(new Product("soda can 250ml", 4.25f, 10));
            Products.Add(new Product("coke 350ml", 5.50f, 10));

            // Add all coins and cash notes possible as payment
            IMoney[] coinArray = { new Coin(0.25f), new Coin(0.50f), new Coin(1.00f), new Coin(2.00f) };
            Money.AddRange(coinArray);

            IMoney[] banknoteArray = { new Banknote(2.00f), new Banknote(5.00f), new Banknote(10.00f), new Banknote(25.00f), new Banknote(50.00f), new Banknote(100.00f), new Banknote(200.00f) };
            Money.AddRange(banknoteArray);
        }
    }
}