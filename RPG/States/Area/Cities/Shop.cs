using RPG.States.Characters.Items;
using System.Collections.Generic;
using System.Linq;

namespace RPG.States.Area.Cities
{
    class Shop
    {
        int Krown = 0;
        ShopStock _stock = new ShopStock();
        int getOffer(Item item)
        {
            ShopItem stockItem = _stock.getItem(item);

            if (stockItem == default(ShopItem))
            {
                stockItem = new ShopItem();
                stockItem.addItem(item, 0);
            }
            return stockItem.getOffer();
        }
        public void buyItems()
        {

        }
    }
    class ShopStock
    {
        List<ShopItem> _shopItemStock = new List<ShopItem>();
        public ShopItem getItem(Item item)
        {
            return _shopItemStock.FirstOrDefault(a => a._item == item);
        }
        public void addItemToStock(Item item, int amount)
        {
            ShopItem addItem = getItem(item);
            if (addItem != default(ShopItem))
            {
                addItem.increaseStock(amount);
            }
            else
            {
                addItem = new ShopItem();
                addItem.addItem(item, amount);
            }
        }
    }
    public class ShopItem
    {
        int _amount = 0;
        double _basePrice = 0;
        public Item _item;

        public void addItem(Item item, int amount)
        {
            _item = item;
            _amount = amount;
        }
        public void increaseStock(int amount)
        {
            _amount = amount;
        }
        public int getOffer()
        {
            //IKKE BRA
            int price = (int)(_basePrice * (1d / (double)(_amount + 1)));
            return price;
        }

    }
}
