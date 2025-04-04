using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public enum ActionType { Attack, UseSkill }
    public HealthBar healthBar;
    public List<ActionWeight> actionWeights;
    public float currentHealth;
    public float maxHealth;
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
    public float Attack(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcAttackDamage(enemy.animaData.Damage, ally);
            ally.TakeDamage(damage);
            allyHealthBar.TakeDamage(damage);
        }
        return damage;
    }
    public float Skill(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcSkillDamage(enemy.animaData.Damage, ally);
            ally.TakeSkillDamage(damage);
            allyHealthBar.TakeDamage(damage);
        }
        return damage;
    }
    public float CalcAttackDamage(float damage, AnimaActions ally)
    {
        return damage * animaData.attackweight * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    public float CalcSkillDamage(float damage, AnimaActions ally)
    {
        return damage * animaData.skillweight * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }
    public float BossAttack(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcBossAttackDamage(enemy.animaData.Damage, ally);
            ally.TakeSkillDamage(damage);
            allyHealthBar.TakeDamage(damage);
        }
        return damage;
    }

    public float BossSkill(EnemyActions enemy, AnimaActions ally, HealthBar allyHealthBar, int skillnum)
    {
        if (!enemy.animaData.Animadie && !ally.animaData.Animadie)
        {
            damage = CalcBossSkillDamage(enemy.animaData.Damage, ally , skillnum);
            ally.TakeSkillDamage(damage);
            allyHealthBar.TakeDamage(damage);
        }
        return damage;
    }
    public float CalcBossAttackDamage(float damage, AnimaActions ally)
    {
        return damage * animaData.attackweight * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.98f, 1.11f);
    }
    public float CalcBossSkillDamage(float damage, AnimaActions ally , int skillnum)
    {
        return damage * animaData.skillweight * (1 - ally.animaData.defense * 0.002f) * Random.Range(0.98f, 1.11f);
    }
    public float TakeSkillDamage(float damage)
    {
        float resdamage = damage;
        this.animaData.Stamina -= damage;

        if (animaData.Stamina <= 0)
        {
            Die();
        }
        return resdamage;
    }
    public void TakeDamage(float damage)
    {
        this.animaData.Stamina -= damage;
        
        if (animaData.Stamina <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        this.animaData.Animadie = true;
    }
}
