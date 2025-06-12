using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimaSelectUI : MonoBehaviour
{
    public static AnimaSelectUI Instance { get; private set; }
    
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private Transform animaSlotParent;
    [SerializeField] private GameObject animaSlotPrefab;
    
    private ShopItemData currentItem;
    private Action<ShopItemData, AnimaDataSO> onSelectionComplete;
    
    private void Awake()
    {
        Instance = this;
        selectPanel.SetActive(false);
    }
    
    public void ShowSelectUI(ShopItemData item, Action<ShopItemData, AnimaDataSO> callback)
    {
        currentItem = item;
        onSelectionComplete = callback;
        
        foreach (Transform child in animaSlotParent)
        {
            Destroy(child.gameObject);
        }
        
        PlayerInfo playerInfo = FindObjectOfType<BattleManager>()?.playerInfo;
        if (playerInfo == null || playerInfo.battleAnima.Count == 0)
        {
            Debug.LogWarning("아니마 정보를 찾을 수 없습니다.");
            return;
        }
        
        List<AnimaDataSO> eligibleAnima = new List<AnimaDataSO>();
        
        if (item.itemType == ItemType.Revive)
        {
            foreach (var anima in playerInfo.battleAnima)
            {
                if (anima.Animadie)
                {
                    eligibleAnima.Add(anima);
                }
            }
        }
        else if (item.itemType == ItemType.Heal)
        {
            foreach (var anima in playerInfo.battleAnima)
            {
                if (!anima.Animadie)
                {
                    eligibleAnima.Add(anima);
                }
            }
        }
        else
        {
            eligibleAnima.AddRange(playerInfo.battleAnima);
        }
        
        foreach (var anima in eligibleAnima)
        {
            GameObject slotObj = Instantiate(animaSlotPrefab, animaSlotParent);
            AnimaSelectSlot slot = slotObj.GetComponent<AnimaSelectSlot>();
            slot.Setup(anima, OnAnimaSlotClicked);
        }
        
        selectPanel.SetActive(true);
    }
    
    private void OnAnimaSlotClicked(AnimaDataSO selectedAnima)
    {
        selectPanel.SetActive(false);
        onSelectionComplete?.Invoke(currentItem, selectedAnima);
    }
    
    public void CloseSelectUI()
    {
        selectPanel.SetActive(false);
    }
}

public class AnimaSelectSlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image animaImage;
    [SerializeField] private TMPro.TextMeshProUGUI animaNameText;
    [SerializeField] private TMPro.TextMeshProUGUI animaHealthText;
    
    private AnimaDataSO animaData;
    private Action<AnimaDataSO> onClickCallback;
    
    public void Setup(AnimaDataSO anima, Action<AnimaDataSO> callback)
    {
        animaData = anima;
        onClickCallback = callback;
        
        animaNameText.text = anima.Name;
        animaHealthText.text = $"{anima.Stamina}/{anima.Maxstamina}";
        
        if (!string.IsNullOrEmpty(anima.Image))
        {
            Sprite sprite = Resources.Load<Sprite>($"Portraits/{anima.Image}");
            if (sprite != null)
            {
                animaImage.sprite = sprite;
            }
        }
        
        GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => onClickCallback?.Invoke(animaData));
    }
}