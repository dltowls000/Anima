using UnityEngine;
using UnityEngine.EventSystems;

public class GroupDropHandler : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventorySlotType slotType;

    public void OnDrop(PointerEventData eventData)
    {
        if (DraggedPortraitUI.Instance == null) return;
        
        var draggedSlot = DraggedPortraitUI.Instance.OriginSlot;
        if (draggedSlot == null || draggedSlot.AnimaData == null) return;

        var anima = draggedSlot.AnimaData;

        if (AnimaInventoryManager.Instance != null)
        {
            if (slotType == InventorySlotType.Party)
            {
                AnimaInventoryManager.Instance.MoveToParty(anima);
            }
            else if (slotType == InventorySlotType.Inventory)
            {
                AnimaInventoryManager.Instance.MoveToInventory(anima);
            }
        }

        if (DraggedPortraitUI.Instance != null)
        {
            DraggedPortraitUI.Instance.EndDrag();
        }
    }
}