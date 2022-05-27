namespace VendingMachine.Models.Money
{
    public class Cash : IMoney
    {
        public float _value { get; set; }
        public Cash(float value)
        {
            _value = value;
        }
    }
}
