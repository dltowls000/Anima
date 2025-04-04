using UnityEngine;
using DamageNumbersPro;
public class AnimaActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public HealthBar healthBar;
    public DamageNumber damageNumber;
    public float damage;
    public float Attack(AnimaActions ally, EnemyActions enemy, HealthBar enemyHealthBar)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcAttackDamage(ally.animaData.Damage, enemy);
            enemy.TakeDamage(damage);
            enemyHealthBar.TakeDamage(damage);
        }
        return damage;
    }
    public float Skill(AnimaActions ally, EnemyActions enemy, HealthBar enemyHealthBar)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcSkillDamage(ally.animaData.Damage, enemy);
            enemy.TakeSkillDamage(damage);
            enemyHealthBar.TakeDamage(damage);
        }
        return damage;
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
    public float CalcAttackDamage(float damage , EnemyActions enemy)
    {
        return damage * animaData.attackweight * (1 - enemy.animaData.defense * 0.002f);
    }

    public float CalcSkillDamage(float damage, EnemyActions enemy)
    {
        return damage * animaData.skillweight * (1 - enemy.animaData.defense * 0.002f);
    }
    public void Die()
    {
        this.animaData.Animadie = true;
    }
   
}
