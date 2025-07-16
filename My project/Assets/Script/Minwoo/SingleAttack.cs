using DamageNumbersPro;
using System.Collections;
using TMPro;
using UnityEngine;
public class SingleAttack:MonoBehaviour
{
    IBattleManager bm;
    public SingleAttack(IBattleManager bm)
    {
        this.bm = bm;
    }

    public IEnumerator SingleAllyAttack(int selectEnemy)
    {
        foreach (AnimaActions anima in bm.AllyActions)
        {
            if (bm.TurnList.Count == 0)
            {
                break;
            }
            if (ReferenceEquals(bm.TurnList[0], anima.animaData))
            {
                PrepareAttack();

                yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.attackName);
                bm.Canvas.SetActive(true);
                yield return anima.Attack(anima, bm.EnemyActions[selectEnemy], bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)]);
                bm.DamageNumber.Spawn(new Vector2(bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.x - 0.1f, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.y + 0.1f), bm.EnemyActions[selectEnemy].damage);
                bm.BattleLogManager.AddLog($"{anima.animaData.Name} hit {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(bm.EnemyActions[selectEnemy].damage)}damage", true);
                bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
                ParserUpdate();

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
                    for (int i = 0; i < bm.TmpturnList.Count; i++)
                    {
                        if (ReferenceEquals(bm.TmpturnList[i], bm.EnemyActions[selectEnemy].animaData))
                        {
                            DestroyImmediate(bm.Turn[i]);
                            bm.TmpturnList.RemoveAt(i);
                            bm.Turn.RemoveAt(i);
                            bm.IsTurn.RemoveAt(i);
                            if (UnityEngine.Random.Range(0, 101) <= bm.EnemyActions[selectEnemy].animaData.DropRate)
                            {
                                AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                                animadata.GetAnima(bm.EnemyActions[selectEnemy].animaData.Name, bm.EnemyActions[selectEnemy].animaData.level);
                                bm.AllyBattleSetting.PlayerInfo.GetAnima(animadata);
                                bm.DropAnima.Add(animadata);
                                bm.AnimaTable.ForEachEntity(entity =>
                                {
                                    if (entity.Get<string>("name") == bm.EnemyActions[selectEnemy].animaData.Name)
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
                                    GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                                }
                            }
                        }
                    }
                    bm.BattleLogManager.AddLog($"{bm.EnemyActions[selectEnemy].animaData.Name}is dead", false);
                    GoldManager.Instance.AddGold(bm.EnemyActions[selectEnemy].animaData.DropGold);
                    bm.TurnList.Remove(bm.EnemyActions[selectEnemy].animaData);
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
                        bm.WinBattle();
                        StopCoroutine(bm.RunningCoroutine);
                    }

                }
                else
                {
                    bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                }
                break;
            }
        }
    }
    public IEnumerator SingleAllySkill(int selectEnemy, int skillNum)
    {
        
            foreach (AnimaActions anima in bm.AllyActions)
            {
                if (bm.TurnList.Count == 0)
                {
                    break;
                }
                if (ReferenceEquals(bm.TurnList[0], anima.animaData))
                {
                    PrepareAttack();

                    yield return bm.CameraManager.ZoomSingleOpp(bm.AllyBattleSetting.AllyInstance[bm.AllyActions.IndexOf(anima)].transform, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform, true, anima.animaData.skillName[skillNum]);

                    bm.Canvas.SetActive(true);
                    yield return anima.Skill(anima, bm.EnemyActions[selectEnemy], bm.EnemyHealthBar[selectEnemy], bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)]);
                    bm.DamageNumber.Spawn(new Vector2(bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.x - 0.1f, bm.EnemyBattleSetting.EnemyInstance[selectEnemy].transform.position.y + 0.1f), bm.EnemyActions[selectEnemy].damage);
                    bm.BattleLogManager.AddLog($"{anima.animaData.Name} used \"{anima.animaData.skillName[skillNum]}\" on {bm.EnemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(bm.EnemyActions[selectEnemy].damage)}damage", true);
                    bm.AllyDamageText[bm.AllyActions.IndexOf(anima)].text = Mathf.Ceil(bm.AllyDamageBar[bm.AllyActions.IndexOf(anima)].thisPoint).ToString();
                    ParserUpdate();
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
                        for (int i = 0; i < bm.TmpturnList.Count; i++)
                        {
                            if (ReferenceEquals(bm.TmpturnList[i], bm.EnemyActions[selectEnemy].animaData))
                            {
                                DestroyImmediate(bm.Turn[i]);
                                bm.TmpturnList.RemoveAt(i);
                                bm.Turn.RemoveAt(i);
                                bm.IsTurn.RemoveAt(i);
                                if (UnityEngine.Random.Range(0, 101) <= bm.EnemyActions[selectEnemy].animaData.DropRate)
                                {
                                    AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                                    animadata.GetAnima(bm.EnemyActions[selectEnemy].animaData.Name, bm.EnemyActions[selectEnemy].animaData.level);
                                    bm.PlayerInfo.GetAnima(animadata);
                                    bm.DropAnima.Add(animadata);
                                    bm.AnimaTable.ForEachEntity(entity =>
                                    {
                                        if (entity.Get<string>("name") == bm.EnemyActions[selectEnemy].animaData.Name)
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
                                        GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                                    }
                                }
                            }
                        }
                        bm.BattleLogManager.AddLog($"{bm.EnemyActions[selectEnemy].animaData.Name}is dead", false);
                        GoldManager.Instance.AddGold(bm.EnemyActions[selectEnemy].animaData.DropGold);
                        bm.TurnList.Remove(bm.EnemyActions[selectEnemy].animaData);
                        DestroyImmediate(bm.EnemyBattleSetting.EnemyHpInstance[selectEnemy]);
                        bm.EnemyBattleSetting.EnemyHpInstance.RemoveAt(selectEnemy);
                        bm.EnemyHealthBar.RemoveAt(selectEnemy);
                        bm.EnemyActions.RemoveAt(selectEnemy);
                        DestroyImmediate(bm.EnemyBattleSetting.EnemyInstance[selectEnemy]);
                        DestroyImmediate(bm.EnemyBattleSetting.EnemyInfoInstance[selectEnemy]);
                        bm.EnemyBattleSetting.EnemyInfoInstance.RemoveAt(selectEnemy);
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
                            bm.WinBattle();
                            StopCoroutine(bm.RunningCoroutine);
                        }

                    }
                    else
                    {
                        bm.Turn[bm.TurnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                    }
                    break;
                }
            }
        
        
        
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
        bm.DamageNumber.Spawn(new Vector2(bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.x - 0.1f, bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.y + 0.1f), bm.AllyActions[selectAlly].damage);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(bm.AllyActions[selectAlly].damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        ParserUpdate();
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
            for (int i = 0; i < bm.TmpturnList.Count; i++)
            {
                if (ReferenceEquals(bm.TmpturnList[i], bm.AllyActions[selectAlly].animaData))
                {
                    DestroyImmediate(bm.Turn[i]);
                    bm.TmpturnList.RemoveAt(i);
                    bm.Turn.RemoveAt(i);
                    bm.IsTurn.RemoveAt(i);
                }
            }
            bm.BattleLogManager.AddLog($"{bm.AllyActions[selectAlly].animaData.Name}is dead", true);
            bm.PlayerInfo.DieAnima(bm.AllyActions[selectAlly].animaData);
            bm.DieAllyAnima.Add(bm.AllyActions.IndexOf(bm.AllyActions[selectAlly]));
            bm.TurnList.Remove(bm.AllyActions[selectAlly].animaData);
            bm.AllyBattleSetting.AllyHpInstance[bm.AllyActions[selectAlly].animaData.location].SetActive(false);
            bm.AllyBattleSetting.AllyInstance[bm.AllyActions[selectAlly].animaData.location].SetActive(false);
            bm.AllyBattleSetting.AllyInfoInstance[selectAlly].SetActive(false);
            bm.AllyAnimaNum--;


            if (bm.AllyAnimaNum == 0)
            {
                bm.stat = BattleState.defeat;
                bm.LoseBattle();
                StopCoroutine(bm.RunningCoroutine);

            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }
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
        bm.DamageNumber.Spawn(new Vector2(bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.x - 0.1f, bm.AllyBattleSetting.AllyInstance[selectAlly].transform.position.y + 0.1f), bm.AllyActions[selectAlly].damage);
        bm.BattleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName[0]}\" on {bm.AllyActions[selectAlly].animaData.Name} for {Mathf.Ceil(bm.AllyActions[selectAlly].damage)} damage", false);
        bm.EnemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(bm.EnemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
        ParserUpdate();
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
            for (int i = 0; i < bm.TmpturnList.Count; i++)
            {
                if (ReferenceEquals(bm.TmpturnList[i], bm.AllyActions[selectAlly].animaData))
                {
                    DestroyImmediate(bm.Turn[i]);
                    bm.TmpturnList.RemoveAt(i);
                    bm.Turn.RemoveAt(i);
                    bm.IsTurn.RemoveAt(i);
                }
            }
            bm.BattleLogManager.AddLog($"{bm.AllyActions[selectAlly].animaData.Name}is dead", true);
            bm.PlayerInfo.DieAnima(bm.AllyActions[selectAlly].animaData);
            bm.DieAllyAnima.Add(bm.AllyActions.IndexOf(bm.AllyActions[selectAlly]));
            bm.TurnList.Remove(bm.AllyActions[selectAlly].animaData);
            bm.AllyBattleSetting.AllyHpInstance[bm.AllyActions[selectAlly].animaData.location].SetActive(false);
            bm.AllyBattleSetting.AllyInstance[bm.AllyActions[selectAlly].animaData.location].SetActive(false);
            bm.AllyBattleSetting.AllyInfoInstance[selectAlly].SetActive(false);
            bm.AllyAnimaNum--;


            if (bm.AllyAnimaNum == 0)
            {
                bm.stat = BattleState.defeat;
                bm.LoseBattle();
                StopCoroutine(bm.RunningCoroutine);

            }
        }
        else
        {
            bm.Turn[bm.TurnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
        }


    }
    
    public IEnumerator SingleAllyHeal()
    {
        yield return null;
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
    private void ParserUpdate()
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
}
