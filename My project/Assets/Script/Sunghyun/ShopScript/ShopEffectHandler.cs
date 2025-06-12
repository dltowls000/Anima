using UnityEngine;

public static class ShopEffectHandler
{
    public static void ApplyEffect(ShopItemData itemData, AnimaDataSO target = null)
    {
        if (itemData == null)
        {
            Debug.LogWarning("아이템 데이터가 null입니다.");
            return;
        }
        
        PlayerInfo playerInfo = GameObject.FindObjectOfType<BattleManager>()?.playerInfo;
        if (playerInfo == null)
        {
            Debug.LogWarning("PlayerInfo를 찾을 수 없습니다.");
            return;
        }

        switch (itemData.itemType)
        {
            case ItemType.Heal:
                ApplyHeal(itemData, target, playerInfo);
                break;

            case ItemType.Revive:
                ApplyRevive(itemData, target, playerInfo);
                break;

            case ItemType.Growth:
                ApplyGrowth(itemData, target);
                break;

            case ItemType.Recipe:
                ApplyRecipe(itemData);
                break;

            case ItemType.Enhance:
                ApplyEnhance(itemData, target);
                break;

            default:
                Debug.LogWarning($"알 수 없는 아이템 타입: {itemData.itemType}");
                break;
        }
    }

    private static void ApplyHeal(ShopItemData itemData, AnimaDataSO target, PlayerInfo playerInfo)
    {
        if (itemData.targetType == TargetType.Single)
        {
            if (target == null)
            {
                Debug.LogWarning("치유 대상이 선택되지 않았습니다.");
                return;
            }

            float healAmount = 0;
            if (itemData.itemID.Contains("half"))
            {
                healAmount = target.Maxstamina * 0.5f;
            }
            else if (itemData.itemID.Contains("Full"))
            {
                healAmount = target.Maxstamina;
            }

            float prevHealth = target.Stamina;
            target.Stamina = Mathf.Min(target.Stamina + healAmount, target.Maxstamina);
            
            Debug.Log($"[단일 회복] {target.Name}의 체력이 {target.Stamina - prevHealth} 회복되었습니다. ({target.Stamina}/{target.Maxstamina})");
        }
        else if (itemData.targetType == TargetType.All)
        {
            foreach (var anima in playerInfo.battleAnima)
            {
                if (!anima.Animadie)
                {
                    float prevHealth = anima.Stamina;
                    anima.Stamina = anima.Maxstamina;

                    Debug.Log(
                        $"[전체 회복] {anima.Name}의 체력이 {anima.Maxstamina - prevHealth} 회복되었습니다. ({anima.Maxstamina}/{anima.Maxstamina})");
                }
            }
        }
    }

    private static void ApplyRevive(ShopItemData itemData, AnimaDataSO target, PlayerInfo playerInfo)
    {
        if (itemData.targetType == TargetType.Single)
        {
            if (target == null || !target.Animadie)
            {
                Debug.LogWarning("부활 대상이 선택되지 않았거나 기절 상태가 아닙니다.");
                return;
            }

            // 부활 처리
            target.Animadie = false;
            target.Stamina = target.Maxstamina * 0.3f;
            
            Debug.Log($"[부활] {target.Name}이(가) 부활했습니다. ({target.Stamina}/{target.Maxstamina})");
        }
    }

    private static void ApplyGrowth(ShopItemData itemData, AnimaDataSO target)
    {
        if (target == null)
        {
            Debug.LogWarning("성장 아이템 대상이 선택되지 않았습니다.");
            return;
        }

        if (itemData.itemID.Contains("Max_boost"))
        {
            Debug.Log($"[성장] {target.Name}의 레벨 상한이 증가했습니다.");
        }
    }

    private static void ApplyRecipe(ShopItemData itemData)
    {
        Debug.Log($"[레시피] {itemData.itemName}을(를) 획득했습니다.");
    }

    private static void ApplyEnhance(ShopItemData itemData, AnimaDataSO target)
    {
        if (target == null)
        {
            Debug.LogWarning("강화 아이템 대상이 선택되지 않았습니다.");
            return;
        }

        if (itemData.itemID.Contains("AP_buff"))
        {
            target.Damage += 5; 
            Debug.Log($"[강화] {target.Name}의 공격력이 증가했습니다. (현재 공격력: {target.Damage})");
        }
    }
}
