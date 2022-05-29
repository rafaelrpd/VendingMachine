namespace VendingMachine.Models.Products
{
    public class Product
    {
        public int _id { get; set; }
        public string _name { get; set; }
        public float _price { get; set; }
        public int _quantity { get; set; }

        public Product(int id, string name, float price, int quantity)
        {
            _id = id;
            _name = name;
            _price = price;
            _quantity = quantity;
        }
    }
}