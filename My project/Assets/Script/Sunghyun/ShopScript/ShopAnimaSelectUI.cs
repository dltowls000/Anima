using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 상점에서 아이템 적용할 아니마를 선택하는 UI
/// </summary>
public class ShopAnimaSelectUI : MonoBehaviour
{
    public static ShopAnimaSelectUI Instance { get; private set; }
    
    [Header("UI 요소")]
    [SerializeField] private GameObject selectPanel; // 선택 패널 전체
    [SerializeField] private Transform animaListContent; // 아니마 슬롯들이 배치될 부모 Transform
    [SerializeField] private GameObject animaSlotPrefab; // AnimaSlotUI 프리팹
    [SerializeField] private TextMeshProUGUI emptyListText; // 선택할 아니마가 없을 때 표시할 텍스트
    [SerializeField] private Button closeButton; // 닫기 버튼
    
    private ShopItemData currentItem; // 현재 선택 중인 아이템
    private Action<ShopItemData, AnimaDataSO> onSelectCallback; // 아니마 선택 완료 시 콜백
    
    private void Awake()
    {
        Instance = this;
        
        // 닫기 버튼 이벤트 연결
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseUI);
        }
        
        // 초기에는 UI 숨기기
        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 아이템에 따른 아니마 선택 UI 표시
    /// </summary>
    /// <param name="item">적용할 상점 아이템</param>
    /// <param name="callback">선택 완료 콜백 (item, selectedAnima)</param>
    public void ShowUI(ShopItemData item, Action<ShopItemData, AnimaDataSO> callback)
    {
        // 현재 아이템과 콜백 저장
        currentItem = item;
        onSelectCallback = callback;
        
        // 기존 슬롯 제거
        ClearAnimaSlots();
        
        // AnimaInventoryManager에서 모든 아니마 가져오기 (인벤토리 + 활성 아니마)
        List<AnimaDataSO> allAnimas = new List<AnimaDataSO>();
        
        if (AnimaInventoryManager.Instance != null)
        {
            allAnimas.AddRange(AnimaInventoryManager.Instance.GetAllAnima());
            allAnimas.AddRange(AnimaInventoryManager.Instance.GetActiveAnima());
            
            // 중복 제거
            allAnimas = new List<AnimaDataSO>(new HashSet<AnimaDataSO>(allAnimas));
        }
        
        // 아이템 타입에 따라 필터링된 아니마 목록 생성
        List<AnimaDataSO> filteredAnimas = FilterAnimasByItem(allAnimas, item);
        
        // 필터링된 아니마가 없는 경우
        if (filteredAnimas.Count == 0)
        {
            ShowEmptyMessage("대상 아니마 없음");
            return;
        }
        
        // 아니마 슬롯 생성
        CreateAnimaSlots(filteredAnimas);
        
        // UI 표시
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
        
        // 빈 메시지 숨기기
        if (emptyListText != null)
        {
            emptyListText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// UI 닫기
    /// </summary>
    public void CloseUI()
    {
        if (selectPanel != null)
        {
            selectPanel.SetActive(false);
        }
    }
    
    /// <summary>
    /// 아이템에 따라 적용 가능한 아니마만 필터링
    /// </summary>
    private List<AnimaDataSO> FilterAnimasByItem(List<AnimaDataSO> animas, ShopItemData item)
    {
        List<AnimaDataSO> filtered = new List<AnimaDataSO>();
        
        foreach (AnimaDataSO anima in animas)
        {
            // 아이템 타입에 따른 필터링 조건
            bool isEligible = false;
            
            switch (item.itemType)
            {
                case ItemType.Heal:
                    // 체력이 최대가 아니고 기절하지 않은 아니마
                    isEligible = !anima.Animadie && anima.Stamina < anima.Maxstamina;
                    break;
                    
                case ItemType.Revive:
                    // 기절한 아니마만
                    isEligible = anima.Animadie;
                    break;
                    
                case ItemType.Growth:
                case ItemType.Enhance:
                    // 기절하지 않은 아니마만
                    isEligible = !anima.Animadie;
                    break;
                    
                default:
                    // 기본적으로 모든 아니마 허용
                    isEligible = true;
                    break;
            }
            
            if (isEligible)
            {
                filtered.Add(anima);
            }
        }
        
        return filtered;
    }
    
    /// <summary>
    /// 기존 아니마 슬롯 정리
    /// </summary>
    private void ClearAnimaSlots()
    {
        if (animaListContent != null)
        {
            foreach (Transform child in animaListContent)
            {
                Destroy(child.gameObject);
            }
        }
    }
    
    /// <summary>
    /// 아니마 슬롯 생성
    /// </summary>
    private void CreateAnimaSlots(List<AnimaDataSO> animas)
    {
        foreach (AnimaDataSO anima in animas)
        {
            GameObject slotObject = Instantiate(animaSlotPrefab, animaListContent);
            AnimaSlotUI slot = slotObject.GetComponent<AnimaSlotUI>();
            
            // AnimaSlotUI 설정 (기존 AnimaSlotUI 활용)
            // 기존의 SetData 메서드에 맞춰서 파라미터 전달
            // 단, 상점에서는 드래그앤드롭 기능은 필요 없으므로 클릭 콜백만 설정
            slot.SetData(anima, InventorySlotType.Shop, OnAnimaSelected);
        }
    }
    
    /// <summary>
    /// 선택할 아니마가 없을 때 메시지 표시
    /// </summary>
    private void ShowEmptyMessage(string message)
    {
        if (emptyListText != null)
        {
            emptyListText.text = message;
            emptyListText.gameObject.SetActive(true);
        }
        
        if (selectPanel != null)
        {
            selectPanel.SetActive(true);
        }
    }
    
    /// <summary>
    /// 아니마 선택 시 호출되는 콜백
    /// </summary>
    private void OnAnimaSelected(AnimaDataSO selectedAnima)
    {
        // UI 닫기
        CloseUI();
        
        // 상점 매니저에 선택 결과 전달
        onSelectCallback?.Invoke(currentItem, selectedAnima);
    }
}