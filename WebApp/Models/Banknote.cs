namespace VendingMachine.WebApp.Models
{
    public class Banknote : IMoney
    {
        public float _value { get; set; }
        public Banknote(float value)
        {
            _value = value;
        }
    }
}
