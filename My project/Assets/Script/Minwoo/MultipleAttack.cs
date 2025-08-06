using System.Collections;
using UnityEngine;

public class MultipleAttack : MonoBehaviour
{
    IBattleManager bm;
    
    public void initialize(IBattleManager bm)
    {
        this.bm = bm;
    }
    
    public IEnumerator MultiAllySkill(int skillnum)
    {
        yield return null; 
    }
    public IEnumerator MultiEnemySkill(EnemyActions enemy, int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiAllyHeal(int skillnum)
    {
        yield return null;
    }
    public IEnumerator MultiEnemyHeal(EnemyActions enemy, int skillnum)
    {
        yield return null;
    }
    public IEnumerator MultiAllyBuff(int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiEnemyBuff(EnemyActions enemy, int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiAllyDebuff(int skillnum) 
    {
        yield return null;
    }
    public IEnumerator MultiEnemyDebuff(EnemyActions enemy, int skillnum)
    {
        yield return null;
    }
}
