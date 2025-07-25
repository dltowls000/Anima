using UnityEngine;
using DamageNumbersPro;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics.Contracts;
using DG.Tweening.Plugins;
using System;
using UnityEngine.LightTransport;
public class AnimaActions : MonoBehaviour
{
    public AnimaDataSO animaData;
    public DamageNumber damageNumber;
    public float damage;
    public float heal;
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
            heal = CalcHealAmount(healer.animaData.Damage, target);
            yield return allyHealthBar.TakeHeal(heal);
            target.TakeHeal(heal);
            yield return healBar.PutDamage(heal);
        }
    }
    
    public IEnumerator IncreaseAbiltiy(AnimaActions buffer, AnimaActions target, string[] abi)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthUp(buffer, target);
                    break;
                case "speed":
                    yield return SpeedUp(buffer, target);
                    break;
                case "defense":
                    yield return DefenseUp(buffer, target);
                    break;
            }
        }
    }
    private IEnumerator StrengthUp(AnimaActions buffer, AnimaActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Damage"] = target.animaData.Damage;
            target.animaData.Damage *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator StrengthDown(AnimaActions debuffer, EnemyActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Damage"] = target.animaData.Damage;
            target.animaData.Damage *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    private IEnumerator SpeedUp(AnimaActions buffer, AnimaActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator SpeedDown(AnimaActions debuffer, EnemyActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Speed"] = target.animaData.Speed;
            target.animaData.Speed *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    private IEnumerator DefenseUp(AnimaActions buffer, AnimaActions target)
    {
        if (!buffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcBuffRatio(buffer.damage);
        }
        yield return null;
    }
    private IEnumerator DefenseDown(AnimaActions debuffer, EnemyActions target)
    {
        if (!debuffer.animaData.Animadie && !target.animaData.Animadie)
        {
            target.animaData.tmpAbility["Defense"] = target.animaData.Defense;
            target.animaData.Defense *= CalcDebuffRatio(debuffer.damage);
        }
        yield return null;
    }
    public IEnumerator DecreseAbility(AnimaActions debuffer, EnemyActions target, string[] abi)
    {
        foreach (string stat in abi)
        {
            switch (stat)
            {
                case "strength":
                    yield return StrengthDown(debuffer, target);
                    break;
                case "speed":
                    yield return SpeedDown(debuffer, target);
                    break;
                case "defense":
                    yield return DefenseDown(debuffer, target);
                    break;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        this.animaData.Stamina -= damage;
        
        if (this.animaData.Stamina <= 0)
        {
            Die();
        }
        
    }
    public void TakeHeal(float heal)
    {
        this.animaData.Stamina += heal;
        if(this.animaData.Stamina > animaData.Maxstamina)
        {
            animaData.Stamina = animaData.Maxstamina;
        }
    }
    private float CalcAttackDamage(float damage , EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.Defense * 0.002f) * Random.Range(0.95f, 1.11f);
    }

    private float CalcSkillDamage(float damage, EnemyActions enemy)
    {
        return damage * (1 - enemy.animaData.Defense * 0.002f) * Random.Range(0.95f, 1.11f) * 1.13f;
    }
    private float CalcHealAmount(float damage, AnimaActions target)
    {
        float a = damage * Random.Range(0.95f, 1.11f) * 1.13f;
        float b = target.animaData.Maxstamina * 0.4f;
        return a >= b ? b : a;
    }
    private float CalcBuffRatio(float damage)
    {
        return 0.0004f * damage + 1.02f;
    }
    private float CalcDebuffRatio(float damage)
    {
        return -0.0002f * damage + 0.94f;
    }
    public void Die()
    {
        this.animaData.Stamina = 0;
        this.animaData.Animadie = true;
    }
   
}
