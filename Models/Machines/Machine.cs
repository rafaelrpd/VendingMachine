using VendingMachine.Models.Money;
using VendingMachine.Models.Products;

namespace VendingMachine.Models.Machines
{
    internal class Machine
    {
        private List<Product> products { get; set; }
        private float balance { get; set; }
        private List<Coin> coins { get; set; }
        private List<Cash> cashBill { get; set; }

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public void AddCoin(Coin coin)
        {
            coins.Add(coin);
        }
        public void AddCashBill(Cash cash)
        {
            cashBill.Add(cash);
        }

        public (string, float) ProductCost(int userSelectionId, int userQuantitySelection)
        {
            // Refactoring : WTF is this, brooooooooooo
            return (products.Where(product => product._id == userSelectionId).ToList()[0]._currency, products.Where(product => product._id == userSelectionId).ToList()[0]._price * userQuantitySelection);
        }

        public float ShowBalance()
        {
            return balance;
        }

        public void SellProduct(int userSelectionId, int userQuantitySelection)
        {
            // Refactoring : WTF is this, brooooooooooo
            products.Where(product => product._id == userSelectionId).ToList()[0]._quantity -= userQuantitySelection;
        }

        public float GiveChange(float totalCost)
        {
            var change = balance - totalCost;
            balance -= totalCost;
            return change;
        }

        public void DepositMoney(float ammoutDeposited)
        {
            balance += ammoutDeposited;
        }

        public void ChangeProductPrice(int productId, float oldProductPrice, float newProductPrice)
        {
            // Todo: Change prices
        }

        public List<Product> ListAllProducts()
        {
            return products;
        }
        public List<Coin> ListAllCoins()
        {
            return coins;
        }
        public List<Cash> ListAllCashBills()
        {
            return cashBill;
        }

        public Machine()
        {
            products = new List<Product>();
            balance = 0;
            coins = new List<Coin>();
            cashBill = new List<Cash>();
        }
    }
}
