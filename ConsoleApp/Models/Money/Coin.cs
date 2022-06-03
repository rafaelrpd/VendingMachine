namespace VendingMachine.Models.Money
{
    public class Coin : IMoney
    {
        public float _value { get; set; }
        public Coin(float value)
        {
            _value = value;
        }
    }
}
