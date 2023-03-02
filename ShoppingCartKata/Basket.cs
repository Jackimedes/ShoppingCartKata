using ShoppingCartKata.Common;

namespace ShoppingCartKata
{
    public class Basket
    {
        private readonly List<ItemQuantity> _basketContents = new();
        public IReadOnlyList<ItemQuantity> BasketContents => _basketContents;
        
        private readonly IDiscountCalculator _discountCalculator;

        public Basket()
        {
            _discountCalculator = new DiscountCalculator(); // Do this with Dependency Injection in real scenario
        }

        public void UpdateBasket(IEnumerable<ItemQuantity> items)
        {
            if (items.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(items));

            // Add any new items or update the quantities of any existing items
            foreach (ItemQuantity itemQuantity in items.GetFlatItemQuantities()) 
            {
                // Is the item in the basket already
                if (_basketContents.AnySingleOut(a => a.Item == itemQuantity.Item, out ItemQuantity existing))
                {
                    int qtyDiff = Math.Abs(existing.Quantity - itemQuantity.Quantity);
                    if (qtyDiff > 0)
                    {
                        // Is the quantity increased or decreased
                        if (existing.Quantity > itemQuantity.Quantity)
                        {
                            // The basket has more, so reduce the quantity
                            existing.Remove(qtyDiff);
                        }
                        else
                        {
                            // The basket needs more
                            existing.Add(qtyDiff);
                        }
                    }
                }
                else
                    _basketContents.Add(itemQuantity);
            }

            // Remove any items no longer required
            List<Item> itemsToRemove = _basketContents
                .Select(s => s.Item)
                .Distinct()
                .Except(items.Select(s => s.Item).Distinct())
                .ToList();
            List<ItemQuantity> remove = _basketContents.Where(q => itemsToRemove.Contains(q.Item)).ToList();
            _basketContents.RemoveRange(remove);
        }

        public void EmptyBasket()
            => _basketContents.Clear();

        public decimal GetTotal()
        {
            decimal total = 0M;

            foreach (ItemQuantity basketItem in _basketContents) 
                total += basketItem.GetItemValue();

            // Apply any discounts
            total -= _discountCalculator.CalculateDiscount(_basketContents);

            return total;
        }

        public int GetNumberOfBasketItems() => _basketContents.Sum(s => s.Quantity);

        public int GetItemQuantity(Item item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _basketContents.Where(q => q.Item == item).Sum(s => s.Quantity);
        }
    }
}
