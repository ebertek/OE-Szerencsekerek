namespace Szerencsekerek
{
    class Player
    {
        public int Winnings { get; private set; } // number of points the player has
        public System.Collections.Generic.List<string> Prizes { get; } = new System.Collections.Generic.List<string>(); // prizes bought by the player

        public void Add(int amount) // add points
        {
            Winnings += amount;
        }
        public bool Subtract(int amount) // subtract points if possible
        {
            if (Winnings - amount >= 0)
            {
                Winnings -= amount;
                return true;
            }
            return false;
        }
        public bool Buy(Shop shop, int index) // buy an item from the shop
        {
            int price = shop.Price(index);
            if (price > 0 && Subtract(price))
            {
                Prizes.Add(shop.Item(index));
                shop.Buy(index);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset() // lose all points
        {
            Winnings = 0;
        }
    }
}
