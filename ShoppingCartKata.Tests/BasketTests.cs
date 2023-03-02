using FluentAssertions;

namespace ShoppingCartKata.Tests
{
    public class BasketTests
    {
        [Fact]
        public void GetItemQuantity_Success()
        {
            Item item = new Item("Test", 10);
            Basket basket = new Basket();
            List<ItemQuantity> initialBasket = new List<ItemQuantity>()
            {
                new ItemQuantity(item, 5)
            };
            basket.UpdateBasket(initialBasket);

            basket.GetItemQuantity(item).Should().Be(5);
        }

        [Fact]
        public void GetItemQuantity_Fail_Params() 
        {
            Item item = new Item("Test1", 10);
            Basket basket = new Basket();
            List<ItemQuantity> initialBasket = new List<ItemQuantity>()
            {
                new ItemQuantity(item, 5)
            };
            basket.UpdateBasket(initialBasket);

            Action noItem = () => basket.GetItemQuantity(null);

            noItem.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateBasket_Success()
        {
            Item item1 = new Item("Item1", 10);
            Item item2 = new Item("Item2", 10);
            Item item3 = new Item("Item3", 10);
            Item item4 = new Item("Item4", 10);
            Item item5 = new Item("Item5", 10);
            Basket basket = new Basket();
            List<ItemQuantity> initialBasket = new List<ItemQuantity>()
            {
                new ItemQuantity(item1, 5),
                new ItemQuantity(item2, 5),
                new ItemQuantity(item3, 5),
                new ItemQuantity(item4, 5),
                new ItemQuantity(item5, 5)
            };
            List<ItemQuantity> updatedBasket = new List<ItemQuantity>()
            {
                new ItemQuantity(item1, 10),
                new ItemQuantity(item3, 1),
                new ItemQuantity(item4, 5),
                new ItemQuantity(item5, 5)
            };

            basket.UpdateBasket(initialBasket);

            basket.BasketContents.Should().HaveCount(5);
            basket.GetNumberOfBasketItems().Should().Be(25);
            basket.GetItemQuantity(item1).Should().Be(5);
            basket.GetItemQuantity(item2).Should().Be(5);
            basket.GetItemQuantity(item3).Should().Be(5);
            basket.GetItemQuantity(item4).Should().Be(5);
            basket.GetItemQuantity(item5).Should().Be(5);

            basket.UpdateBasket(updatedBasket);

            basket.BasketContents.Should().HaveCount(4);
            basket.GetNumberOfBasketItems().Should().Be(21);
            basket.GetItemQuantity(item1).Should().Be(10);
            basket.GetItemQuantity(item2).Should().Be(0);
            basket.GetItemQuantity(item3).Should().Be(1);
            basket.GetItemQuantity(item4).Should().Be(5);
            basket.GetItemQuantity(item5).Should().Be(5);
        }

        [Fact]
        public void EmptyBasket_Success()
        {
            Item item = new Item("Test", 10);
            Basket basket = new Basket();
            List<ItemQuantity> initialBasket = new List<ItemQuantity>()
            { 
                new ItemQuantity(item, 1) 
            };
            basket.UpdateBasket(initialBasket);

            basket.EmptyBasket();

            basket.BasketContents.Should().HaveCount(0);
        }

        public class GetTotal_TheoryData : TheoryData<List<ItemQuantity>, decimal>
        {
            public GetTotal_TheoryData()
            {
                Item itemA = new Item("A", 10);
                Item itemB = new Item("B", 15);
                Item itemC = new Item("C", 40);
                Item itemD = new Item("D", 55);

                List<ItemQuantity> test1 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemA, 1),
                    new ItemQuantity(itemB, 1),
                    new ItemQuantity(itemC, 1),
                    new ItemQuantity(itemD, 1)
                };
                List<ItemQuantity> test2 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemA, 1),
                    new ItemQuantity(itemB, 3),
                    new ItemQuantity(itemC, 1),
                    new ItemQuantity(itemD, 1)
                };
                List<ItemQuantity> test3 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemA, 1),
                    new ItemQuantity(itemB, 1),
                    new ItemQuantity(itemC, 1),
                    new ItemQuantity(itemD, 4)
                };
                List<ItemQuantity> test4 = new List<ItemQuantity>()
                {
                    new ItemQuantity(itemA, 3),
                    new ItemQuantity(itemB, 3),
                    new ItemQuantity(itemC, 2),
                    new ItemQuantity(itemD, 2)
                };

                // No discount: 10 + 15 + 40 + 55
                Add(test1, 120M);
                // 3 For 40 discount: 10 + 40 + 40 + 55
                Add(test2, 145M);
                // 25% of 2 discount: 10 + 15 + 40 + 220 - 27.5 - 27.5
                Add(test3, 230M);
                // 3 For 40 and 25% of 2 discounts: 30 + 40 + 80 + 110 - 27.5
                Add(test4, 232.5M);
            }
        }

        [Theory]
        [ClassData(typeof(GetTotal_TheoryData))]
        public void GetTotal_Success(List<ItemQuantity> itemQuantities, decimal expectedTotal)
        {
            Basket basket = new Basket();
            basket.UpdateBasket(itemQuantities);

            decimal total = basket.GetTotal();

            total.Should().Be(expectedTotal);
        }
    }
}
