using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUIManager : MonoBehaviour
{
    public static ShopUIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform slotParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private TextMeshProUGUI playerGoldText;

    private Action<ShopItemData> onPurchaseCallback;
    private Dictionary<string, ShopItemSlot> activeSlots = new();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged += UpdateGoldUI;
            
            UpdateGoldUI(GoldManager.Instance.GetCurrentGold());
        }
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateGoldUI;
        }
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
        playerGoldText.text = $"{gold:N0}";
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

    public void OpenShopPanel()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
    }
    
    public void CloseShopPanel()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
    }
}