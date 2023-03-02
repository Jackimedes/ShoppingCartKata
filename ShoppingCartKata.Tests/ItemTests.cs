using FluentAssertions;
using Xunit;

namespace ShoppingCartKata.Tests
{
    public class ItemTests
    {
        [Fact]
        public void Item_Create_Success()
        {
            Item item = new Item("Test", 10);

            Assert.NotNull(item);
            item.ItemSku.Should().Be("Test");
            item.UnitPrice.Should().Be(10);
        }

        [Fact]
        public void Item_Create_Fail_Params()
        {
            Action skuNull = () => new Item(null, 10);
            Action skuEmpty = () => new Item("", 10);
            Action noPrice = () => new Item("Test", 0);

            skuNull.Should().Throw<ArgumentNullException>();
            skuEmpty.Should().Throw<ArgumentNullException>();
            noPrice.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Item_GetItemPrice_Success()
        {
            Item item = new Item("Test", 10);

            decimal itemPrice1 = item.GetItemPrice(1);
            decimal itemPrice2 = item.GetItemPrice(10);
            decimal itemPrice3 = item.GetItemPrice(55);
            decimal itemPrice4 = item.GetItemPrice(100000000);

            itemPrice1.Should().Be(10);
            itemPrice2.Should().Be(100);
            itemPrice3.Should().Be(550);
            itemPrice4.Should().Be(1000000000);
        }

        [Fact]
        public void Item_GetItemPrice_Fail_Params()
        {
            Item item = new Item("Test", 10);

            Action noQuantity = () => item.GetItemPrice(0);

            noQuantity.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ItemQuantity_Create_Success()
        {
            Item item = new Item("Test", 10);

            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            Assert.NotNull(itemQuantity);
            itemQuantity.Item.Should().Be(item);
            itemQuantity.Quantity.Should().Be(1);
        }

        [Fact]
        public void ItemQuantity_Create_Fail_Params()
        {
            Item item = new Item("Test", 10);

            Action noItem = () => new ItemQuantity(null, 1);
            Action noQty = () => new ItemQuantity(item, 0);

            noItem.Should().Throw<ArgumentNullException>();
            noQty.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ItemQuantity_Add_Success()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            itemQuantity.Add(1);

            itemQuantity.Quantity.Should().Be(2);
        }

        [Fact]
        public void ItemQuantity_Add_Fail_Params()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            Action noQty = () => itemQuantity.Add(0);

            noQty.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ItemQuantity_Remove_Success()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 2);

            itemQuantity.Remove(1);

            itemQuantity.Quantity.Should().Be(1);
        }

        [Fact]
        public void ItemQuantity_Remove_Fail_Params()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            Action noQty = () => itemQuantity.Remove(0);

            noQty.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ItemQuantity_Remove_Fail_NegativeQty()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            Action negative = () => itemQuantity.Remove(1);

            negative.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ItemQuantity_GetItemValue_Success()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity = new ItemQuantity(item, 1);

            decimal itemVal = itemQuantity.GetItemValue();

            itemVal.Should().Be(10);
        }

        [Fact]
        public void ItemQuantityExtension_GetFlatItemQuantities_Success()
        {
            Item item = new Item("Test", 10);
            ItemQuantity itemQuantity1 = new ItemQuantity(item, 1);
            ItemQuantity itemQuantity2 = new ItemQuantity(item, 3);
            List<ItemQuantity> itemQuantities = new List<ItemQuantity>()
            {
                itemQuantity1, 
                itemQuantity2 
            };

            List<ItemQuantity> flatItemQuantities = itemQuantities.GetFlatItemQuantities();

            flatItemQuantities.Should().HaveCount(1);
            flatItemQuantities[0].Quantity.Should().Be(4);
        }

        [Fact]
        public void ItemQuantityExtension_GetFlatItemQuantities_Fail_Params()
        {
            List<ItemQuantity> items = new List<ItemQuantity>();

            Action noItems = () => { items.GetFlatItemQuantities(); };

            noItems.Should().Throw<ArgumentNullException>();
        }
    }
}