using System.Collections.Generic;

public class ShopList
{
    public List<ShopItemData> defaultItems;
    public List<ShopItemData> randomItems;

    public List<ShopItemData> GetAllItems()
    {
        List<ShopItemData> all = new();
        all.AddRange(defaultItems);
        all.AddRange(randomItems);
        return all;
    }

    public static ShopList CreateForVillage(string villageID)
    {
        return new ShopList
        {
            defaultItems = DefaultShopItems.GetDefaultItems(),
            randomItems = VillageShopGenerator.GenerateRandomItemsForVillage(villageID, 2)
        };
    }
}