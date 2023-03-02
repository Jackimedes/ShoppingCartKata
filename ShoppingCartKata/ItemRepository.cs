
namespace ShoppingCartKata
{
    class ItemRepository : IItemRepository
    {
        internal ItemRepository() 
        {
        }

        List<Item> IItemRepository.GetAllItems()
        {
            // Faking interaction with Db
            List<Item> items = new List<Item>()
            {
                new Item("A", 10),
                new Item("B", 15),
                new Item("C", 40),
                new Item("D", 55)
            };

            return items;
        }
    }

    public interface IItemRepository
    {
        public List<Item> GetAllItems();
    }
}
