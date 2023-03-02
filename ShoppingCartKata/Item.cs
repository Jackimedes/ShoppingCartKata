using ShoppingCartKata.Common;

namespace ShoppingCartKata
{
    public class Item
    {
        public Item(string itemSku, decimal unitPrice)
        {
            if (string.IsNullOrEmpty(itemSku))
                throw new ArgumentNullException(nameof(itemSku));
            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice));

            ItemSku = itemSku;
            UnitPrice = unitPrice;
        }

        public string ItemSku { get; private set; }
        public decimal UnitPrice { get; private set; }

        public decimal GetItemPrice(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            return UnitPrice * quantity;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Item item)) 
                return false;

            Item itemObj = (Item)obj!;

            return itemObj.ItemSku == ItemSku;
        }
    }

    public class ItemQuantity
    {
        public ItemQuantity(Item item, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            Item = item ?? throw new ArgumentNullException(nameof(item));
            Quantity = quantity;
        }

        public Item Item { get; private set; }
        public int Quantity { get; private set; }

        internal void Add(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            Quantity += amount;
        }
        internal void Remove(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            int newQty = Quantity - amount;
            if (newQty <= 0)
                throw new ArgumentOutOfRangeException("Cannot have fewer than 0 of an item");
            Quantity = newQty;
        }

        public decimal GetItemValue()
        {
            return Item.GetItemPrice(Quantity);
        }
    }

    public static class ItemQuantitiesExtensions
    {
        public static List<ItemQuantity> GetFlatItemQuantities(this IEnumerable<ItemQuantity> itemQuantities)
        {
            if (itemQuantities.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(itemQuantities));

            List<ItemQuantity> flatItemQuantities = new();
            foreach (var itemGroup in itemQuantities.GroupBy(gb => gb.Item))
            {
                int totalQty = itemGroup.Select(s => s.Quantity).Sum();
                flatItemQuantities.Add(new ItemQuantity(itemGroup.Key, totalQty));
            }

            return flatItemQuantities;
        }
    }
}