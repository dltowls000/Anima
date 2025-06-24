
using System.Collections.Generic;
using UnityEngine;

public class VillageDataManager : MonoBehaviour
{
    public static VillageDataManager Instance { get; private set; }
    
    private Dictionary<string, VillageShopState> villageShops = new Dictionary<string, VillageShopState>(10);
    private int innCurrentPrice = 300;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public int GetInnPrice()
    {
        return innCurrentPrice;
    }
    
    public void UpdateInnPrice(int newPrice)
    {
        innCurrentPrice = newPrice;
    }
    
    public void ResetInnPrice()
    {
        innCurrentPrice = 300;
    }
    
    public VillageShopState GetVillageShopState(string villageID, List<ShopItemData> shopItems)
    {
        if (villageShops.ContainsKey(villageID))
        {
            return villageShops[villageID];
        }
        
        VillageShopState newState = new VillageShopState(villageID, shopItems);
        villageShops[villageID] = newState;
        
        Debug.Log($"새 마을 상점 상태 생성: {villageID}");
        
        return newState;
    }
    
    public bool PurchaseItem(string villageID, string itemID)
    {
        if (!villageShops.ContainsKey(villageID))
        {
            Debug.LogWarning($"존재하지 않는 마을: {villageID}");
            return false;
        }
        
        return villageShops[villageID].PurchaseItem(itemID);
    }
    
    public void ClearAllVillages()
    {
        villageShops.Clear();
        Debug.Log("모든 마을 상점 상태 초기화됨");
    }
    
    public void ClearVillage(string villageID)
    {
        if (villageShops.ContainsKey(villageID))
        {
            villageShops.Remove(villageID);
            Debug.Log($"마을 상점 상태 초기화됨: {villageID}");
        }
    }
    
    public void DebugPrintAllVillages()
    {
        Debug.Log($"현재 저장된 마을 수: {villageShops.Count}");
        
        foreach (var entry in villageShops)
        {
            Debug.Log($"마을 ID: {entry.Key}");
            foreach (var item in entry.Value.remainingCounts)
            {
                Debug.Log($"  아이템: {item.Key}, 남은 횟수: {item.Value}");
            }
        }
    }
}