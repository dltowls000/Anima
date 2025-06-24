using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimaInventoryManager : MonoBehaviour
{
    public static AnimaInventoryManager Instance { get; private set; }

    [SerializeField] private List<AnimaDataSO> playerAnima = new List<AnimaDataSO>();
    [SerializeField] private List<AnimaDataSO> activeAnima = new List<AnimaDataSO>();

    public event Action OnAnimaInventoryChanged;
    public event Action<AnimaDataSO> OnPartyAddFailed;

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

    public List<AnimaDataSO> GetAllAnima() => new List<AnimaDataSO>(playerAnima);
    public List<AnimaDataSO> GetActiveAnima() => new List<AnimaDataSO>(activeAnima);

    public bool IsAnimaDefeated(AnimaDataSO anima)
    {
        if (anima == null) return false;
        return anima.Animadie || anima.Stamina <= 0;
    }

    public void AddAnima(AnimaDataSO anima)
    {
        if (anima != null && !playerAnima.Contains(anima))
        {
            playerAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }

    public void SetActiveAnima(List<AnimaDataSO> selected)
    {
        if (selected == null) return;
        
        activeAnima.Clear();
        foreach (var anima in selected)
        {
            if (anima != null)
            {
                activeAnima.Add(anima);
            }
        }
        
        OnAnimaInventoryChanged?.Invoke();
    }

    public void SwapSlots(AnimaSlotUI fromSlot, AnimaSlotUI toSlot)
    {
        if (fromSlot == null || toSlot == null) return;

        var fromAnima = fromSlot.AnimaData;
        var toAnima = toSlot.AnimaData;
        
        if (fromAnima == null && toAnima == null) return;
        
        var fromType = fromSlot.SlotType;
        var toType = toSlot.SlotType;
        
        var fromList = fromType == InventorySlotType.Inventory ? playerAnima : activeAnima;
        var toList = toType == InventorySlotType.Inventory ? playerAnima : activeAnima;
        
        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Party && 
            IsAnimaDefeated(fromAnima))
        {
            OnPartyAddFailed?.Invoke(fromAnima);
            return;
        }
        
        int fromIndex = fromAnima != null ? fromList.IndexOf(fromAnima) : -1;
        int toIndex = toAnima != null ? toList.IndexOf(toAnima) : -1;
        
        if (fromList != toList)
        {
            if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Party)
            {
                if (fromAnima != null)
                {
                    if (IsAnimaDefeated(fromAnima))
                    {
                        OnPartyAddFailed?.Invoke(fromAnima);
                        return;
                    }

                    if (toAnima == null)
                    {
                        if (activeAnima.Count < 3)
                        {
                            playerAnima.Remove(fromAnima);
                            activeAnima.Add(fromAnima);
                        }
                    }
                    else if (toIndex >= 0)
                    {
                        playerAnima.Remove(fromAnima);
                        activeAnima[toIndex] = fromAnima;
                        playerAnima.Add(toAnima);
                    }
                }
            }
            else if (fromType == InventorySlotType.Party && toType == InventorySlotType.Inventory)
            {
                if (fromAnima != null && fromIndex >= 0)
                {
                    if (toAnima == null)
                    {
                        activeAnima.Remove(fromAnima);
                        playerAnima.Add(fromAnima);
                    }
                    else if (toIndex >= 0)
                    {
                        if (IsAnimaDefeated(toAnima))
                        {
                            OnPartyAddFailed?.Invoke(toAnima);
                            return;
                        }

                        activeAnima[fromIndex] = toAnima;
                        playerAnima[toIndex] = fromAnima;
                    }
                }
            }
        }
        else if (fromAnima != null && toAnima != null && fromIndex >= 0 && toIndex >= 0)
        {
            var temp = fromList[fromIndex];
            fromList[fromIndex] = toList[toIndex];
            toList[toIndex] = temp;
        }
        
        OnAnimaInventoryChanged?.Invoke();
    }
    
    public void MoveToParty(AnimaDataSO anima)
    {
        if (anima == null || activeAnima.Contains(anima)) return;
        if (activeAnima.Count >= 3) return;
        
        if (IsAnimaDefeated(anima))
        {
            OnPartyAddFailed?.Invoke(anima);
            return;
        }

        if (playerAnima.Remove(anima))
        {
            activeAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }

    public void MoveToInventory(AnimaDataSO anima)
    {
        if (anima == null || playerAnima.Contains(anima)) return;
        
        if (activeAnima.Remove(anima))
        {
            playerAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }
}