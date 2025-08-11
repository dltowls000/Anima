using DamageNumbersPro;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SingleAttack:MonoBehaviour
{
    IBattleManager bm;
    List<string> expiredBuffList;
    public void initialize(IBattleManager bm)
    {
        this.bm = bm;
    }

    public IEnumerator SingleAllyAttack(AnimaActions anima , int selectEnemy)
    {
        PrepareAttack();

        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.attackName);
        bm.Canvas.SetActive(true);
        yield return anima.Attack(anima, bm.EnemyActions[selectEnemy], bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)]);
        bm.DamageNumber.Spawn(new Vector2(bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.x - 0.1f, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.y + 0.1f), anima.damage);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} hit {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(anima.damage)}damage", true);
        bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        DamageParserUpdate();

        if (bm.EnemyActions[selectEnemy].animaData.Animadie)
        {
            if (anima.animaData.Speed <= bm.EnemyActions[selectEnemy].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatEnemy(bm.EnemyActions[selectEnemy], selectEnemy);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(anima.animaData);

    }
    public IEnumerator SingleAllySkill(AnimaActions anima, int selectEnemy, int skillNum)
    {
        PrepareAttack();

        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.skillName[skillNum]);

        bm.Canvas.SetActive(true);
        yield return anima.Skill(anima, bm.EnemyActions[selectEnemy], bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)]);
        bm.DamageNumber.Spawn(new Vector2(bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.x - 0.1f, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.y + 0.1f), anima.damage);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(anima.damage)}damage", true);
        bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.EnemyActions[selectEnemy].animaData.Animadie)
        {
            if (anima.animaData.Speed <= bm.EnemyActions[selectEnemy].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatEnemy(bm.EnemyActions[selectEnemy], selectEnemy);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(anima.animaData);
    }

    public IEnumerator SingleEnemyAttack(EnemyActions enemy, int selectAlly)
    {
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.attackName);
        bm.Canvas.SetActive(true);
        yield return enemy.Attack(enemy, bm.AllyActions[selectAlly], bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.DamageNumber.Spawn(new Vector2(bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.x - 0.1f, bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.y + 0.1f), enemy.damage);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} hit {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatAlly(bm.AllyActions[selectAlly], selectAlly);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator SingleEnemySkill(EnemyActions enemy, int selectAlly)
    {
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.Skill(enemy, bm.AllyActions[selectAlly], bm.AllyHealthBar[selectAlly], bm.EnemyDamageBar[enemy.animaData.enemyIndex]);
        bm.DamageNumber.Spawn(new Vector2(bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.x - 0.1f, bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.y + 0.1f), enemy.damage);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(enemy.damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        DamageParserUpdate();
        if (bm.AllyActions[selectAlly].animaData.Animadie)
        {
            if (enemy.animaData.Speed < bm.AllyActions[selectAlly].animaData.Speed)
            {
                bm.Turn[bm.TurnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            else
            {
                bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
            }
            DefeatAlly(bm.AllyActions[selectAlly], selectAlly);
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
        BuffUpdate(enemy.animaData);
    }
    
    public IEnumerator SingleAllyHeal(AnimaActions anima, int selectAlly, int skillNum)
    {
        PrepareAttack();
        yield return bm.CameraManager.ZoomSingleIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.Heal(anima, bm.AllyActions[selectAlly], bm.AllyHealthBar[selectAlly], bm.AllyHealBar[bm.AllyActions.IndexOf(anima)]);
        bm.DamageNumber.Spawn(new Vector2(bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.x - 0.1f, bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.y + 0.1f), anima.heal);
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(anima.heal)}heal", true);
        bm.AllyHealText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyHealBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyHeal(EnemyActions enemy, int selectEnemy)
    {
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);

        yield return bm.CameraManager.ZoomSingleIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.Heal(enemy, bm.EnemyActions[selectEnemy], bm.EnemyHealthBar[selectEnemy], bm.EnemyHealBar[enemy.animaData.enemyIndex]);
        bm.DamageNumber.Spawn(new Vector2(bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.x - 0.1f, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.y + 0.1f), enemy.heal);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(enemy.heal)} heal", false);
        bm.EnemyHealText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyHealBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        HealParserUpdate();
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator SingleAllyBuff(AnimaActions anima, int selectAlly, int skillNum)
    {
        PrepareAttack();
        yield return bm.CameraManager.ZoomSingleIde(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.IncreaseAbility(anima, bm.AllyActions[selectAlly], bm.MatchedSkill[0].Affect.ToArray());
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {string.Join(", ",bm.MatchedSkill[0].Affect)} up", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyBuff(EnemyActions enemy, int selectEnemy)
    {
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomSingleIde(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.IncreaseAbility(enemy, bm.EnemyActions[selectEnemy], bm.MatchedSkill[0].Affect.ToArray());
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} up", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    public IEnumerator SingleAllyDebuff(AnimaActions anima, int selectEnemy, int skillNum)
    {
        PrepareAttack();
        yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.skillName[skillNum]);
        bm.Canvas.SetActive(true);
        yield return anima.DecreaseAbility(anima, bm.EnemyActions[selectEnemy], bm.MatchedSkill[0].Affect.ToArray());
        bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", true);
        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(anima.animaData);
    }
    public IEnumerator SingleEnemyDebuff(EnemyActions enemy, int selectAlly)
    {
        yield return new WaitForSeconds(0.5f);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
        yield return bm.CameraManager.ZoomSingleOpp(bm.EnemyBattleSetting.EnemyInstance[bm.EnemyActions.IndexOf(enemy)].transform, bm.AllyBattleSetting.AllyInstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
        bm.Canvas.SetActive(true);
        yield return enemy.DecreaseAbility(enemy, bm.AllyActions[selectAlly], bm.MatchedSkill[0].Affect.ToArray());
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {string.Join(", ", bm.MatchedSkill[0].Affect)} down", false);
        bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        BuffUpdate(enemy.animaData);
    }
    private void PrepareAttack()
    {
        bm.IsZKeyPressed = false;
        bm.AttackButton.interactable = true;
        bm.SkillButton.interactable = true;
        bm.AnimaActionUI.SetActive(false);
        bm.IsTurn[bm.TurnIndex].SetActive(false);
        bm.TurnList.RemoveAt(0);
        bm.Canvas.SetActive(false);
    }
    private void DamageParserUpdate()
    {
        foreach (var max in bm.AllyDamageBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var max in bm.EnemyDamageBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var foo in bm.AllyDamageBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
        foreach (var foo in bm.EnemyDamageBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
    }
    private void HealParserUpdate()
    {
        foreach(var max in bm.AllyHealBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var max in bm.EnemyHealBar)
        {
            if (bm.MaxValue < max.maxPoint)
            {
                bm.MaxValue = max.maxPoint;
            }
        }
        foreach (var foo in bm.AllyHealBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
        foreach (var foo in bm.EnemyHealBar)
        {
            foo.maxPoint = bm.MaxValue;
            foo.Initialize();
        }
    }
    private void DefeatEnemy(EnemyActions enemy, int selectEnemy)
    {
        for (int i = 0; i < bm.TmpturnList.Count; i++)
        {
            if (ReferenceEquals(bm.TmpturnList[i], enemy.animaData))
            {
                DestroyImmediate(bm.Turn[i]);
                bm.TmpturnList.RemoveAt(i);
                bm.Turn.RemoveAt(i);
                bm.IsTurn.RemoveAt(i);
                if (UnityEngine.Random.Range(0, 101) <= enemy.animaData.DropRate)
                {
                    AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                    animadata.GetAnima(enemy.animaData.Name, enemy.animaData.level);
                    bm.AllyBattleSetting.PlayerInfo.GetAnima(animadata);
                    bm.DropAnima.Add(animadata);
                    bm.AnimaTable.ForEachEntity(entity =>
                    {
                        if (entity.Get<string>("name") == enemy.animaData.Name)
                        {
                            entity.Set<int>("Meeted", 2);
                            DBUpdater.Save();

                        }
                    });
                }
                foreach (var tmp in bm.AllyActions)
                {
                    if (!tmp.animaData.Animadie)
                    {
                        tmp.animaData.LevelUp();
                        bm.AllyHealthBar[bm.AllyActions.IndexOf(tmp)].UpdateHealthBar();
                        GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                    }
                }
            }
        }
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name}is dead", false);
        GoldManager.Instance.AddGold(enemy.animaData.DropGold);
        bm.TurnList.Remove(enemy.animaData);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyHpInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyHpInstance.RemoveAt(selectEnemy);
        bm.EnemyHealthBar.RemoveAt(selectEnemy);
        bm.EnemyActions.RemoveAt(selectEnemy);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInstance[selectEnemy]);
        DestroyImmediate(bm.EnemyBattleSetting.EnemyInfoInstance[selectEnemy]);
        bm.EnemyBattleSetting.EnemyInstance.RemoveAt(selectEnemy);
        bm.EnemyAnimaNum--;
        for (int i = 0; i < 3; i++)
        {
            var rebuild = GameObject.Find($"Enemy{i}");
            if (rebuild != null)
            {
                rebuild.transform.Find("Status").GetComponent<StatusSync>().dieanima++;
            }
        }

        if (bm.EnemyActions.Count == 0)
        {
            foreach (var ally in bm.AllyActions)
            {
                ally.animaData.location = -1;
            }
            bm.stat = BattleState.win;
            bm.TurnIndex = 0;
            if (bm.RunningCoroutine != null)
            {
                StopCoroutine(bm.RunningCoroutine);
            }
            bm.WinBattle();
        }
    }
    private void DefeatAlly(AnimaActions ally, int selectAlly)
    {
        for (int i = 0; i < bm.TmpturnList.Count; i++)
        {
            if (ReferenceEquals(bm.TmpturnList[i], ally.animaData))
            {
                DestroyImmediate(bm.Turn[i]);
                bm.TmpturnList.RemoveAt(i);
                bm.Turn.RemoveAt(i);
                bm.IsTurn.RemoveAt(i);
            }
        }
        bm.BattleLogManager.AddLog($"{ally.animaData.Name}is dead", true);
        bm.PlayerInfo.DieAnima(ally.animaData);
        bm.DieAllyAnima.Add(bm.AllyActions.IndexOf(ally));
        bm.TurnList.Remove(ally.animaData);
        bm.AllyBattleSetting.AllyHpInstance[ally.animaData.location].SetActive(false);
        bm.AllyBattleSetting.AllyInstance[ally.animaData.location].SetActive(false);
        bm.AllyBattleSetting.AllyInfoInstance[selectAlly].SetActive(false);
        bm.AllyAnimaNum--;


        if (bm.AllyAnimaNum == 0)
        {
            bm.stat = BattleState.defeat;
            if(bm.RunningCoroutine != null)
            {
                StopCoroutine(bm.RunningCoroutine);
            }
            bm.LoseBattle();

        }
    }
    private void BuffUpdate(AnimaDataSO anima)
    {
        expiredBuffList = bm.BuffManager.TickOne(anima);
        while (expiredBuffList.Count < 0)
        {
            switch (expiredBuffList[0])
            {
                case "strength":
                    anima.Damage = anima.tmpAbility["strength"];
                    break;
                case "speed":
                    anima.Speed = anima.tmpAbility["speed"];
                    break;
                case "defense":
                    anima.Defense = anima.tmpAbility["defense"];
                    break;
            }
            expiredBuffList.RemoveAt(0);
        }
    }

}
