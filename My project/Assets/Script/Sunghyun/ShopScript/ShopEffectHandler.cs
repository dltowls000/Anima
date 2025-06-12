using UnityEngine;

public static class ShopEffectHandler
{
    public static void ApplyEffect(ShopItemData itemData, object target = null)
    {
        if (itemData == null)
        {
            Debug.LogWarning("아이템 데이터가 null입니다.");
            return;
        }

        switch (itemData.itemType)
        {
            case ItemType.Heal:
                ApplyHeal(itemData, target);
                break;

            case ItemType.Revive:
                ApplyRevive(itemData, target);
                break;

            case ItemType.Growth:
                Debug.Log($"[성장 아이템 사용됨] {itemData.itemName}");
                break;

            case ItemType.Recipe:
                Debug.Log($"[레시피 아이템 획득] {itemData.itemName}");
                break;

            case ItemType.Enhance:
                Debug.Log($"[강화 아이템 사용됨] {itemData.itemName}");
                break;

            default:
                Debug.LogWarning($"알 수 없는 아이템 타입: {itemData.itemType}");
                break;
        }
    }

    private static void ApplyHeal(ShopItemData itemData, object target)
    {
        if (itemData.targetType == TargetType.Single)
        {
            Debug.Log($"[단일 회복] {itemData.itemName} → 대상: {target}");
        }
        else if (itemData.targetType == TargetType.All)
        {
            Debug.Log($"[전체 회복] {itemData.itemName} → 모든 아니마에게 회복 적용");
        }
    }

    private static void ApplyRevive(ShopItemData itemData, object target)
    {
        if (itemData.targetType == TargetType.Single)
        {
            Debug.Log($"[단일 부활] {itemData.itemName} → 대상: {target}");
        }
    }
}