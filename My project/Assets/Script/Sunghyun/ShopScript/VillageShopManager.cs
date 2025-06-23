using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여러 마을의 상점 상태를 관리하는 싱글톤 매니저
/// </summary>
public class VillageShopManager : MonoBehaviour
{
    private static VillageShopManager _instance;
    
    public static VillageShopManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("VillageShopManager");
                _instance = go.AddComponent<VillageShopManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    // 마을별 상점 상태 (key: villageID, value: 마을 상점 상태)
    private Dictionary<string, VillageShopState> villageShops = new Dictionary<string, VillageShopState>(10);
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// 마을 상점 상태 조회 (없으면 새로 생성)
    /// </summary>
    /// <param name="villageID">마을 ID</param>
    /// <param name="shopItems">해당 마을에서 판매하는 아이템 목록</param>
    /// <returns>마을 상점 상태</returns>
    public VillageShopState GetVillageShopState(string villageID, List<ShopItemData> shopItems)
    {
        // 이미 방문한 마을이면 기존 상태 반환
        if (villageShops.ContainsKey(villageID))
        {
            return villageShops[villageID];
        }
        
        // 새 마을이면 상태 생성 후 저장
        VillageShopState newState = new VillageShopState(villageID, shopItems);
        villageShops[villageID] = newState;
        
        Debug.Log($"새 마을 상점 상태 생성: {villageID}");
        
        return newState;
    }
    
    /// <summary>
    /// 아이템 구매 처리
    /// </summary>
    /// <param name="villageID">마을 ID</param>
    /// <param name="itemID">구매할 아이템 ID</param>
    /// <returns>구매 성공 여부</returns>
    public bool PurchaseItem(string villageID, string itemID)
    {
        if (!villageShops.ContainsKey(villageID))
        {
            Debug.LogWarning($"존재하지 않는 마을: {villageID}");
            return false;
        }
        
        return villageShops[villageID].PurchaseItem(itemID);
    }
    
    /// <summary>
    /// 스테이지 전환 시 호출하여 모든 마을 상태 초기화
    /// </summary>
    public void ClearAllVillages()
    {
        villageShops.Clear();
        Debug.Log("모든 마을 상점 상태 초기화됨");
    }
    
    /// <summary>
    /// 특정 마을 상태 초기화
    /// </summary>
    /// <param name="villageID">초기화할 마을 ID</param>
    public void ClearVillage(string villageID)
    {
        if (villageShops.ContainsKey(villageID))
        {
            villageShops.Remove(villageID);
            Debug.Log($"마을 상점 상태 초기화됨: {villageID}");
        }
    }
    
    /// <summary>
    /// 디버깅용: 모든 마을의 상태 출력
    /// </summary>
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