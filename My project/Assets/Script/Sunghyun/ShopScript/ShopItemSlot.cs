using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopItemSlot : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemRemainingText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Button targetSelectButton; // 단일 대상용

    private ShopItemData itemData;
    private int currentRemaining;
    private Action<ShopItemData> onPurchase;

    public void Setup(ShopItemData data, int remainingCount, Action<ShopItemData> onPurchaseCallback)
    {
        itemData = data;
        currentRemaining = remainingCount;
        onPurchase = onPurchaseCallback;

        itemNameText.text = data.itemName;
        itemPriceText.text = $"{data.price} G";
        itemDescriptionText.text = data.itemDescription;
        itemRemainingText.text = $"{remainingCount} / {data.maxPurchaseCount}";

        purchaseButton.interactable = remainingCount > 0;
        purchaseButton.onClick.RemoveAllListeners();
        purchaseButton.onClick.AddListener(() => onPurchase?.Invoke(data));

        // 대상 선택 버튼 (단일 대상용만 표시)
        targetSelectButton.gameObject.SetActive(data.targetType == TargetType.Single);
        targetSelectButton.onClick.RemoveAllListeners();
        targetSelectButton.onClick.AddListener(() =>
        {
            Debug.Log("대상 선택 UI 호출 - 구현 예정");
        });
    }

    public void UpdateRemainingCount(int newCount)
    {
        currentRemaining = newCount;
        itemRemainingText.text = $"{newCount} / {itemData.maxPurchaseCount}";
        purchaseButton.interactable = newCount > 0;
    }
}