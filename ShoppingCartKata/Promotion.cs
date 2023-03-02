using System.Data;

namespace ShoppingCartKata
{
    internal abstract class Promotion
    {
        internal abstract decimal CalculateDiscount(List<ItemQuantity> items);
        protected abstract List<Item> GetApplicableItems();
        
        /// <summary>
        /// Take a list of Item Quantities and return a list of Items applicable to the promotion
        /// with one entry per Item per quantity where the 
        /// </summary>
        /// <param name="itemQuantities"></param>
        /// <returns></returns>
        protected List<Item> GetFlatApplicableItems(List<ItemQuantity> items)
        {
            // Get applicable items, ordering by unit price to try and keep similar values together
            List<ItemQuantity> applicableItems = items
                .GetFlatItemQuantities()
                .Where(q => GetApplicableItems().Contains(q.Item))
                .OrderBy(ob => ob.Item.UnitPrice)
                .ToList();

            List<Item> flatItems = new();
            foreach (ItemQuantity itemQuantity in applicableItems)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                    flatItems.Add(itemQuantity.Item);
            }

            return flatItems;
        }
    }

    #region Promotion Implementations

    internal class ThreeForFortyPromotion : Promotion
    {
        private readonly IItemRepository _itemRepository;

        public ThreeForFortyPromotion()
        {
            _itemRepository = new  ItemRepository(); // Do this with Dependency Injection in real scenario
        }

        internal override decimal CalculateDiscount(List<ItemQuantity> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            decimal discount = 0M;

            List<Item> flatApplicableItems = GetFlatApplicableItems(items);

            // Chunk the applicable items into appropriate groups
            List<Item[]> chunkedItems = flatApplicableItems.Chunk(3).ToList();
            foreach (Item[] itemChunk in chunkedItems)
            {
                // For every 3 items, the discount is the difference between the sum of the values and 40
                if (itemChunk.Length % 3 == 0)
                    discount += Math.Abs(itemChunk.Sum(s => s.GetItemPrice(1)) - 40);
            }

            return discount;
        }

        protected override List<Item> GetApplicableItems()
        {
            List<Item> items = _itemRepository.GetAllItems();

            return items.Where(q => q.ItemSku == "B").ToList();
        }
    }

    internal class TwentyFiveOffTwoPromotion : Promotion
    {
        private readonly IItemRepository _itemRepository;

        public TwentyFiveOffTwoPromotion()
        {
            _itemRepository = new ItemRepository(); // Do this with Dependency Injection in real scenario
        }

        internal override decimal CalculateDiscount(List<ItemQuantity> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            decimal discount = 0M;

            List<Item> flatApplicableItems = GetFlatApplicableItems(items);

            // Chunk the applicable items into appropriate groups
            List<Item[]> chunkedItems = flatApplicableItems.Chunk(2).ToList();
            foreach (Item[] itemChunk in chunkedItems)
            {
                // For every 2 items, save 25%
                if (itemChunk.Length % 2 == 0)
                    discount += itemChunk.Sum(s => s.GetItemPrice(1)) * 0.25M;
            }

            return discount;
        }

        protected override List<Item> GetApplicableItems()
        {
            List<Item> items = _itemRepository.GetAllItems();

            return items.Where(q => q.ItemSku == "D").ToList();
        }
    }

    #endregion

    public interface IDiscountCalculator
    {
        public decimal CalculateDiscount(List<ItemQuantity> items);
    }

    internal class DiscountCalculator : IDiscountCalculator
    {
        private static Lazy<PromotionCollection> _promotions = new Lazy<PromotionCollection>(() => new PromotionCollection());
        private static PromotionCollection Promotions => _promotions.Value;

        public DiscountCalculator()
        {
        }

        public decimal CalculateDiscount(List<ItemQuantity> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            decimal discount = 0m;

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            IEnumerable<Promotion> promotions = Promotions.ConcretePromotions
                .Select(r => Activator.CreateInstance(r) as Promotion);
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            foreach (Promotion promotion in promotions)
                discount += promotion.CalculateDiscount(items);

            return discount;
        }
    }

    internal class PromotionCollection
    {
        internal Type[] ConcretePromotions { get; }

        public PromotionCollection()
        {
            ConcretePromotions = GetType().Assembly.GetTypes()
                .Where(p => typeof(Promotion).IsAssignableFrom(p) && !p.IsAbstract)
                .ToArray();
        }
    }
}
