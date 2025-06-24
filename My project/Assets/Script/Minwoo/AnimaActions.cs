using UnityEngine;
using DamageNumbersPro;
using System.Collections;
using UnityEngine.UI;
public class AnimaActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public DamageNumber damageNumber;
    public float damage;
    
    public IEnumerator Attack(AnimaActions ally, EnemyActions enemy, HealthBar enemyHealthBar, ParserBar damageBar)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcAttackDamage(ally.animaData.Damage, enemy);
            yield return enemyHealthBar.TakeDamage(damage);
            enemy.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
    }
    public IEnumerator Skill(AnimaActions ally, EnemyActions enemy, HealthBar enemyHealthBar, ParserBar damageBar)
    {
        if (!ally.animaData.Animadie && !enemy.animaData.Animadie)
        {
            damage = CalcSkillDamage(ally.animaData.Damage, enemy);
            yield return enemyHealthBar.TakeDamage(damage);
            enemy.TakeSkillDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }

    public float TakeSkillDamage(float damage)
    {
        this.damage = damage;
        this.animaData.Stamina -= damage;
        
        if (animaData.Stamina <= 0)
        {
            Die();
        }
        return damage;
    }
    public float TakeDamage(float damage)
    {
        this.damage = damage;
        this.animaData.Stamina -= damage;
        
        if (animaData.Stamina <= 0)
        {
            Die();
        }
        return damage;
    }
    public float CalcAttackDamage(float damage , EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    public float CalcSkillDamage(float damage, EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f) * 1.13f;
    }
    public void Die()
    {
        this.animaData.Animadie = true;
    }
   
}
