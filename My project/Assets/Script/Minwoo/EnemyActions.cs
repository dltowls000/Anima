using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public enum ActionType { Attack, UseSkill }
    public List<ActionWeight> actionWeights;
    public string performance = "";
    public float damage;
    public class ActionWeight
    {
        public ActionType actionType;
        public float weight;
    }
    public void SetCustomWeights(List<ActionWeight> customWeights)
    {
        actionWeights = customWeights;
    }

    public void InitializeWeights()
    {
        if (actionWeights == null || actionWeights.Count == 0)
        {
            actionWeights = new List<ActionWeight>
            {
                new ActionWeight { actionType = ActionType.Attack, weight = 1.0f },
                new ActionWeight { actionType = ActionType.UseSkill, weight = 1.0f }
            };
        }
    }

    public void DecideAction()
    {
        float totalWeight = 0;
        foreach (ActionWeight actionWeight in actionWeights)
        {
            totalWeight += actionWeight.weight;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (ActionWeight actionWeight in actionWeights)
        {
            cumulativeWeight += actionWeight.weight;
            if (randomValue <= cumulativeWeight)
            {
                SetAction(actionWeight.actionType);
                return;
            }
        }
    }
    public void SetAction(ActionType actionType)
    {
        switch (actionType)
        {
            case ActionType.Attack:
                performance = "Attack";
                break;
            case ActionType.UseSkill:
                performance = "Skill";
                break;
        }
    }
    public IEnumerator Attack(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Skill(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar, ParserBar damageBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcSkillDamage(enemy.animaData.Damage, ally);
            yield return allyHealthBar.TakeDamage(damage);
            ally.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Heal(EnemyActions healer, EnemyActions target, HealthBar enemyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            damage = CalcHealAmount(healer.animaData.Damage, target);
            yield return enemyHealthBar.TakeHeal(damage);
            target.TakeHeal(damage);
            yield return healBar.PutDamage(damage);
        }
    }
    public float CalcAttackDamage(float damage, AnimaActions ally)
    {
        return damage * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    public float CalcSkillDamage(float damage, AnimaActions ally)
    {
        return damage * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f) * 1.13f;
    }
    public float CalcHealAmount(float damage, EnemyActions target)
    {
        float a = damage * Random.Range(0.95f, 1.11f) * 1.13f;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    public void TakeDamage(float damage)
    {
        this.damage = damage;
        this.animaData.Stamina -= damage;
        
        if (this.animaData.Stamina <= 0)
        {
            Die();
        }
        
    }
    public void TakeHeal(float damage)
    {
        this.damage = damage;
        this.animaData.Stamina += damage;
        if (this.animaData.Stamina > animaData.Maxstamina)
        {
            animaData.Stamina = animaData.Maxstamina;
        }
    }
    public void Die()
    {
        this.animaData.Stamina = 0;
        this.animaData.Animadie = true;
    }
}
