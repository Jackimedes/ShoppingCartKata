using System.Data;

namespace ShoppingCartKata.Promotions
{
    internal interface IPromotion
    {
        decimal CalculateDiscount(Dictionary<int, Item> itemQuantities);
    }

    #region Promotion Implementations

    internal class ThreeForFortyPromotion : IPromotion
    {
        public decimal CalculateDiscount(Dictionary<int, Item> itemQuantities)
        {
            decimal discount = 0M;

            IEnumerable<KeyValuePair<int, Item>> bItems = itemQuantities.Where(q => q.Value.ItemSku == "B");

            return discount;
        }
    }

    internal class TwentyFiveOffTwoPromotion : IPromotion
    {
        public decimal CalculateDiscount(Dictionary<int, Item> itemQuantities)
        {
            decimal discount = 0M;
            
            IEnumerable<KeyValuePair<int, Item>> dItems = itemQuantities.Where(q => q.Value.ItemSku == "D");

            return discount;
        }
    }

    #endregion

    public interface IDiscountCalculator
    {
        public decimal CalculateDiscount(Dictionary<int, Item> itemQuantities);
    }

    internal class DiscountCalculator : IDiscountCalculator
    {
        private readonly List<IPromotion> _promotions;

        public DiscountCalculator()
        {
            // Get all available promotions from assembly
            #pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            _promotions = GetType().Assembly.GetTypes()
                .Where(p => typeof(IPromotion).IsAssignableFrom(p) && !p.IsInterface)
                .Select(r => Activator.CreateInstance(r) as IPromotion)
                .ToList();
            #pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        public decimal CalculateDiscount(Dictionary<int, Item> itemQuantities)
        {
            decimal discount = 0m;
            foreach (IPromotion promotion in _promotions)
            {
                discount = Math.Max(discount, promotion.CalculateDiscount(itemQuantities));
            }
            return discount;
        }
    }
}
