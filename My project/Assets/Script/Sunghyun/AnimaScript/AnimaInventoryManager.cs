using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AnimaInventoryManager : MonoBehaviour
{
    public static AnimaInventoryManager Instance { get; private set; }

    public PlayerInfo playerInfo;

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
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "save.dat")))
        {
            Debug.Log("¾øÀ½");
            DBUpdater.Save();
        }
        else
        {
            DBUpdater.Load();
        }
        playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
        playerInfo.Initialize();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public List<AnimaDataSO> GetAllAnima() => new List<AnimaDataSO>(playerInfo.haveAnima).Concat(new List<AnimaDataSO>(playerInfo.battleAnima)).ToList();
    public List<AnimaDataSO> GetActiveAnima() => new List<AnimaDataSO>(playerInfo.battleAnima);
    public List<AnimaDataSO> GetHaveAnima() => new List<AnimaDataSO>(playerInfo.haveAnima);
    public bool IsAnimaDefeated(AnimaDataSO anima)
    {
        return anima != null && (anima.Animadie || anima.Stamina <= 0);
    }

    public void AddAnima(AnimaDataSO anima)
    {
        if (anima != null && !playerInfo.haveAnima.Contains(anima))
        {
            playerInfo.haveAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }

    public void SetActiveAnima(List<AnimaDataSO> selected)
    {
        if (selected == null) return;

        playerInfo.battleAnima.Clear();
        foreach (var anima in selected)
        {
            if (anima != null)
            {
                playerInfo.battleAnima.Add(anima);
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

        var fromList = fromType == InventorySlotType.Inventory ? playerInfo.haveAnima : playerInfo.battleAnima;
        var toList = toType == InventorySlotType.Inventory ? playerInfo.haveAnima : playerInfo.battleAnima;

        if (fromType == InventorySlotType.Inventory && toType == InventorySlotType.Party && IsAnimaDefeated(fromAnima))
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

                    if (toAnima == null && playerInfo.battleAnima.Count < playerInfo.maxAnimaNum)
                    {
                        playerInfo.haveAnima.Remove(fromAnima);
                        playerInfo.battleAnima.Add(fromAnima);
                    }
                    else if (toIndex >= 0)
                    {
                        playerInfo.haveAnima.Remove(fromAnima);
                        playerInfo.battleAnima[toIndex] = fromAnima;
                        playerInfo.haveAnima.Add(toAnima);
                    }
                }
            }
            else if (fromType == InventorySlotType.Party && toType == InventorySlotType.Inventory)
            {
                if (fromAnima != null && fromIndex >= 0)
                {
                    if (toAnima == null)
                    {
                        playerInfo.battleAnima.Remove(fromAnima);
                        playerInfo.haveAnima.Add(fromAnima);
                    }
                    else if (toIndex >= 0)
                    {
                        if (IsAnimaDefeated(toAnima))
                        {
                            OnPartyAddFailed?.Invoke(toAnima);
                            return;
                        }

                        playerInfo.battleAnima[fromIndex] = toAnima;
                        playerInfo.haveAnima[toIndex] = fromAnima;
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
        if (anima == null || playerInfo.battleAnima.Contains(anima)) return;
        if (playerInfo.battleAnima.Count >= playerInfo.maxAnimaNum) return;

        if (IsAnimaDefeated(anima))
        {
            OnPartyAddFailed?.Invoke(anima);
            return;
        }

        if (playerInfo.haveAnima.Remove(anima))
        {
            playerInfo.battleAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }

    public void MoveToInventory(AnimaDataSO anima)
    {
        if (anima == null || playerInfo.haveAnima.Contains(anima)) return;

        if (playerInfo.battleAnima.Remove(anima))
        {
            playerInfo.haveAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }
}