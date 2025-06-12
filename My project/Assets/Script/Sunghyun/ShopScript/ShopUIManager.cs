using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TextMeshProUGUI playerGoldText;

    private Action<ShopItemData> onPurchaseCallback;
    private Dictionary<string, ShopItemSlot> activeSlots = new();

    private void Awake()
    {
        Instance = this;
    }

    public void ShowShopItems(List<ShopItemData> items, Dictionary<string, int> remainingCounts, Action<ShopItemData> onPurchase)
    {
        ClearSlots();
        onPurchaseCallback = onPurchase;

        foreach (var item in items)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            ShopItemSlot slot = go.GetComponent<ShopItemSlot>();
            int remaining = remainingCounts.ContainsKey(item.itemID) ? remainingCounts[item.itemID] : item.maxPurchaseCount;

            slot.Setup(item, remaining, onPurchaseCallback);
            activeSlots[item.itemID] = slot;
        }
    }

    public void UpdateGoldUI(int gold)
    {
        playerGoldText.text = $"Gold: {gold}";
    }

    public void UpdateItemSlot(string itemID, int newRemaining)
    {
        if (activeSlots.TryGetValue(itemID, out var slot))
        {
            slot.UpdateRemainingCount(newRemaining);
        }
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
        activeSlots.Clear();
    }
}