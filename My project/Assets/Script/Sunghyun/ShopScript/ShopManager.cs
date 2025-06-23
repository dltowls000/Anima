using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private string villageID = "Village_01"; // 기본값 설정
    
    private ShopList shopList;
    private VillageShopState shopState;
    private Dictionary<string, int> remainingCounts = new Dictionary<string, int>();
    
    private void Start()
    {
        // 상점 아이템 목록 생성
        shopList = ShopList.CreateForVillage(villageID);
        
        // 마을 상점 상태 가져오기 (첫 방문이면 새로 생성)
        shopState = VillageShopManager.Instance.GetVillageShopState(villageID, shopList.GetAllItems());
        
        // UI에 표시할 남은 횟수 캐싱
        foreach (var item in shopList.GetAllItems())
        {
            remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);
        }
        
        // UI에 상점 아이템 표시
        ShopUIManager.Instance.ShowShopItems(shopList.GetAllItems(), remainingCounts, OnItemPurchase);
        
        // 이제 필요 없음 (ShopUIManager의 OnEnable에서 자동으로 처리)
        // ShopUIManager.Instance.UpdateGoldUI(GoldManager.Instance.GetCurrentGold());
    }
    
    private void OnItemPurchase(ShopItemData item)
    {
        // 단일 대상 아이템인 경우 아니마 선택 UI 표시
        if (item.targetType == TargetType.Single)
        {
            // 아니마 선택 UI 호출
            // AnimaSelectUI 내부에서 필요한 아니마 목록 가져오기, 필터링 등 처리
            ShopAnimaSelectUI.Instance.ShowUI(item, OnAnimaSelected);
        }
        else
        {
            // 전체 대상 아이템은 선택 없이 바로 구매 및 효과 적용
            if (ProcessItemPurchase(item))
            {
                // 효과 적용은 ShopEffectHandler에서 담당
                ShopEffectHandler.ApplyEffect(item);
            }
        }
    }
    
    // 아니마 선택 후 호출될 콜백
    private void OnAnimaSelected(ShopItemData item, AnimaDataSO selectedAnima)
    {
        // 아이템 구매 처리 (골드 차감 등)
        if (ProcessItemPurchase(item))
        {
            // 효과 적용
            ShopEffectHandler.ApplyEffect(item, selectedAnima);
        }
    }

    // 구매 처리 로직 (중복 코드 제거)
    private bool ProcessItemPurchase(ShopItemData item)
    {
        // 골드가 충분한지 체크
        if (!GoldManager.Instance.SpendGold(item.price))
        {
            Debug.Log("골드가 부족합니다!");
            return false;
        }

        // 마을 상점 상태 업데이트
        if (!VillageShopManager.Instance.PurchaseItem(villageID, item.itemID))
        {
            Debug.LogWarning("이미 모두 구매한 아이템입니다.");
            // 골드 환불
            GoldManager.Instance.AddGold(item.price);
            return false;
        }

        // 남은 횟수 캐싱 업데이트
        remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);

        // UI 업데이트 (아이템 슬롯만 업데이트)
        ShopUIManager.Instance.UpdateItemSlot(item.itemID, remainingCounts[item.itemID]);
    
        return true;
    }
    
    // Inspector에서 마을 ID 변경 시 호출되는 메서드
    public void SetVillageID(string newVillageID)
    {
        villageID = newVillageID;
        
        if (shopList != null)
        {
            // 상점 목록 및 상태 갱신
            shopList = ShopList.CreateForVillage(villageID);
            shopState = VillageShopManager.Instance.GetVillageShopState(villageID, shopList.GetAllItems());
            
            // UI 갱신
            foreach (var item in shopList.GetAllItems())
            {
                remainingCounts[item.itemID] = shopState.GetRemainingCount(item.itemID);
            }
            
            ShopUIManager.Instance.ShowShopItems(shopList.GetAllItems(), remainingCounts, OnItemPurchase);
        }
    }
}