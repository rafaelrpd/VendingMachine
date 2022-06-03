namespace VendingMachine._FutureUse
{
    internal class Utils
    {
        // Validate money / change @Not Completed
        bool _userDepositValueValid = false;
            // While user deposit not valid
            while (!_userDepositValueValid)
            {
                // Get user input
                float.TryParse(Console.ReadLine(), out float _userDepositValue);

        // Get decimal of user input
        int _userDecimalValue = (int)(_userDepositValue * 100) % 100;
                
                // Try to validade if each decimal could be used as change for the user
                foreach(Coin coin in Coins)
                {
                    if ((_userDecimalValue % (int) (coin._value* 100)) == 0)
                    {
                        _userDepositValueValid = true;
                    }
}
            }
    }
}
