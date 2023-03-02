using FluentAssertions;
using Xunit.Abstractions;

namespace ShoppingCartKata.Tests
{
    public class PromotionTests
    {
        public class ThreeForFortyPromotion_CalculateDiscount_TheoryData : TheoryData<List<ItemQuantity>, decimal>
        {
            public ThreeForFortyPromotion_CalculateDiscount_TheoryData()
            {
                Item itemB = new Item("B", 15);
                Item proofItem = new Item("B", 25);

                List<ItemQuantity> test1 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemB, 3)
                };
                List<ItemQuantity> test2 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemB, 5)
                };
                List<ItemQuantity> test3 = new List<ItemQuantity>()
                {
                    new ItemQuantity(proofItem, 3)
                };
                List<ItemQuantity> test4 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemB, 5),
                    new ItemQuantity(proofItem, 5)
                };

                // (15 + 15 + 15) - 40
                Add(test1, 5M);
                // (15 + 15 + 15) - 40, (15 + 15) not applied
                Add(test2, 5M);
                // (25 + 25 + 25) - 40
                Add(test3, 35M);
                // ((15 + 15 + 15) - 40) + ((15 + 15 + 25) - 40) + ((25 + 25 + 25) - 40), (25 + 25) not applied
                Add(test4, 55M); 
            }
        }

        [Theory]
        [ClassData(typeof(ThreeForFortyPromotion_CalculateDiscount_TheoryData))]
        public void ThreeForFortyPromotion_CalculateDiscount_Success(List<ItemQuantity> itemQuantities, decimal expectedDiscount)
        {
            ThreeForFortyPromotion promotion = new ThreeForFortyPromotion();

            decimal discount = promotion.CalculateDiscount(itemQuantities);

            discount.Should().Be(expectedDiscount);
        }

        [Fact]
        public void ThreeForFortyPromotion_CalculateDiscount_Fail_Params()
        {
            ThreeForFortyPromotion promotion = new ThreeForFortyPromotion();

            Action noItems = () => promotion.CalculateDiscount(null);

            noItems.Should().Throw<ArgumentNullException>();
        }

        public class TwentyFiveOffTwoPromotion_CalculateDiscount_TheoryData : TheoryData<List<ItemQuantity>, decimal>
        {
            public TwentyFiveOffTwoPromotion_CalculateDiscount_TheoryData()
            {
                Item itemD = new Item("D", 55);
                Item proofItem = new Item("D", 20);

                List<ItemQuantity> test1 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemD, 2)
                };
                List<ItemQuantity> test2 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemD, 5)
                };
                List<ItemQuantity> test3 = new List<ItemQuantity>()
                {
                    new ItemQuantity(proofItem, 2)
                };
                List<ItemQuantity> test4 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemD, 5),
                    new ItemQuantity(proofItem, 3)
                };

                // (55 + 55) * 0.25
                Add(test1, 27.5M);
                // ((55 + 55) * 0.25) + ((55 + 55) * 0.25), 55 not applied
                Add(test2, 55M);
                // (20 + 20) * 0.25
                Add(test3, 10M);
                // ((20 + 20) * 0.25) + ((20 + 55) * 0.25) + ((55 + 55) * 0.25) + ((55 + 55) * 0.25)
                Add(test4, 83.75M);
            }
        }

        [Theory]
        [ClassData(typeof(TwentyFiveOffTwoPromotion_CalculateDiscount_TheoryData))]
        public void TwentyFiveOffTwoPromotion_CalculateDiscount_Success(List<ItemQuantity> itemQuantities, decimal expectedDiscount)
        {
            TwentyFiveOffTwoPromotion promotion = new TwentyFiveOffTwoPromotion();

            decimal discount = promotion.CalculateDiscount(itemQuantities);

            discount.Should().Be(expectedDiscount);
        }

        [Fact]
        public void TwentyFiveOffTwoPromotion_CalculateDiscount_Fail_Params()
        {
            TwentyFiveOffTwoPromotion promotion = new TwentyFiveOffTwoPromotion();

            Action noItems = () => promotion.CalculateDiscount(null);

            noItems.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void DiscountCalculator_CalculateDiscount_Success()
        {
            DiscountCalculator discountCalculator = new DiscountCalculator();
            Item item1 = new Item("Test1", 10);
            Item item2 = new Item("Test2", 20);
            List<ItemQuantity> itemQuantites = new List<ItemQuantity>() 
            { 
                new ItemQuantity(item1, 1), 
                new ItemQuantity(item2, 1) 
            };

            decimal discount = discountCalculator.CalculateDiscount(itemQuantites);

            discount.Should().Be(0);
        }

        [Fact]
        public void DiscountCalculator_CalculateDiscount_Fail_Params()
        {
            DiscountCalculator discountCalculator = new DiscountCalculator();

            Action noItems = () => discountCalculator.CalculateDiscount(null);

            noItems.Should().Throw<ArgumentNullException>();
        }

        /// <summary>
        /// Force consideration of tests for new implementations of promotion
        /// </summary>
        [Fact]
        public void PromotionCollection_Success_CorrectNumOfPromotions()
        {
            PromotionCollection promotionCollection = new PromotionCollection();

            promotionCollection.ConcretePromotions.Should().HaveCount(2);
        }
    }
}
