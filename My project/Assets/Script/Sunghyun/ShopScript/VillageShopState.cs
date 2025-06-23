using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 개별 마을의 상점 상태를 저장하는 클래스
/// </summary>
[System.Serializable]
public class VillageShopState
{
    public string villageID;
    
    // 아이템별 남은 구매 횟수 (key: itemID, value: 남은 구매 횟수)
    public Dictionary<string, int> remainingCounts = new Dictionary<string, int>();
    
    /// <summary>
    /// 마을 상점 상태 초기화
    /// </summary>
    /// <param name="villageID">마을 고유 ID</param>
    /// <param name="items">해당 마을에서 판매하는 아이템 목록</param>
    public VillageShopState(string villageID, List<ShopItemData> items)
    {
        this.villageID = villageID;
        
        // 각 아이템의 최대 구매 횟수로 초기화
        foreach (var item in items)
        {
            remainingCounts[item.itemID] = item.maxPurchaseCount;
        }
    }
    
    /// <summary>
    /// 아이템 구매 처리
    /// </summary>
    /// <param name="itemID">구매할 아이템 ID</param>
    /// <returns>구매 성공 여부</returns>
    public bool PurchaseItem(string itemID)
    {
        if (!remainingCounts.ContainsKey(itemID) || remainingCounts[itemID] <= 0)
        {
            return false;
        }
        
        remainingCounts[itemID]--;
        return true;
    }
    
    /// <summary>
    /// 아이템의 남은 구매 횟수 조회
    /// </summary>
    /// <param name="itemID">조회할 아이템 ID</param>
    /// <returns>남은 구매 횟수</returns>
    public int GetRemainingCount(string itemID)
    {
        remainingCounts.TryGetValue(itemID, out int count);
        return count;
    }
    
    /// <summary>
    /// 아이템이 구매 가능한지 여부 조회
    /// </summary>
    /// <param name="itemID">조회할 아이템 ID</param>
    /// <returns>구매 가능 여부</returns>
    public bool IsPurchasable(string itemID)
    {
        return remainingCounts.TryGetValue(itemID, out int count) && count > 0;
    }
}