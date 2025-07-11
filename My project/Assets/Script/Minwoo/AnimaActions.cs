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
            enemy.TakeDamage(damage);
            yield return damageBar.PutDamage(damage);
        }
        
    }
    public IEnumerator Heal(AnimaActions healer, AnimaActions target, HealthBar allyHealthBar, ParserBar healBar)
    {
        if (!healer.animaData.Animadie && !target.animaData.Animadie)
        {
            damage = CalcHealAmount(healer.animaData.Damage, target);
            yield return allyHealthBar.TakeHeal(damage);
            target.TakeHeal(damage);
            yield return healBar.PutDamage(damage);
        }
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
        if(this.animaData.Stamina > animaData.Maxstamina)
        {
            animaData.Stamina = animaData.Maxstamina;
        }
    }
    public float CalcAttackDamage(float damage , EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    public float CalcSkillDamage(float damage, EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.defense * 0.002f) * Random.Range(0.95f, 1.11f) * 1.13f;
    }
    public float CalcHealAmount(float damage, AnimaActions target)
    {
        float a = damage * Random.Range(0.95f, 1.11f) * 1.13f;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    public void Die()
    {
        this.animaData.Stamina = 0;
        this.animaData.Animadie = true;
    }
   
}
