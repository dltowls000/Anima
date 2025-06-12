/*using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private string villageID = "";
    
    private ShopList shopList;
    private Dictionary<string, int> remainingCounts = new();
    
    private void Start()
    {
        if (GoldManager.GetCurrentGold() == 0)
        {
            GoldManager.Initialize(300);
        }
        else
        {
            GoldManager.LoadGold();
        }
        
        shopList = ShopList.CreateForVillage(villageID);
        
        foreach (var item in shopList.GetAllItems())
        {
            remainingCounts[item.itemID] = item.maxPurchaseCount;
        }
        
        ShopUIManager.Instance.ShowShopItems(shopList.GetAllItems(), remainingCounts, OnItemPurchase);
        ShopUIManager.Instance.UpdateGoldUI(GoldManager.GetCurrentGold());
    }
    
    private void OnItemPurchase(ShopItemData item)
    {
        if (!GoldManager.SpendGold(item.price))
        {
            Debug.Log("골드가 부족합니다!");
            return;
        }
        
        remainingCounts[item.itemID]--;
        ShopUIManager.Instance.UpdateItemSlot(item.itemID, remainingCounts[item.itemID]);
        
        if (item.targetType == TargetType.Single)
        {
            AnimaSelectUI.Instance.ShowSelectUI(item, OnAnimaSelected);
        }
        else
        {
            ShopEffectHandler.ApplyEffect(item);
        }
    }
    
    private void OnAnimaSelected(ShopItemData item, AnimaDataSO selectedAnima)
    {
        ShopEffectHandler.ApplyEffect(item, selectedAnima);
    }
}*/