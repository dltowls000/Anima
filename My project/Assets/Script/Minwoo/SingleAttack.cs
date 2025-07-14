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
    
    //public IEnumerator SingleEnemySkill(int selectAlly)
    //{

    //        yield return new WaitForSeconds(0.5f);
    //        isTurn[turnIndex].SetActive(false);
    //        turnList.RemoveAt(0);
    //        canvas.SetActive(false);//체력 바 동기화 문제 발생 예상
    //        /* Attack */


    //        /* Animation */

    //        yield return cameraManager.ZoomSingleOpp(enemyBattleSetting.enemyinstance[enemyActions.IndexOf(enemy)].transform, allyBattleSetting.allyinstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
    //        canvas.SetActive(true);
    //        yield return enemy.Skill(enemy, allyActions[selectAlly], allyHealthBar[selectAlly], enemyDamageBar[enemy.animaData.enemyIndex]);
    //        damageNumber.Spawn(new Vector2(allyBattleSetting.allyinstance[selectAlly].transform.position.x - 0.1f, allyBattleSetting.allyinstance[selectAlly].transform.position.y + 0.1f), allyActions[selectAlly].damage);
    //        battleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName}\" on {allyActions[selectAlly].animaData.Name} for {Mathf.Ceil(allyActions[selectAlly].damage)} damage", false);
    //        enemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(enemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
    //        foreach (var max in allyDamageBar)
    //        {
    //            if (maxValue < max.maxPoint)
    //            {
    //                maxValue = max.maxPoint;
    //            }
    //        }
    //        foreach (var max in enemyDamageBar)
    //        {
    //            if (maxValue < max.maxPoint)
    //            {
    //                maxValue = max.maxPoint;
    //            }
    //        }
    //        foreach (var foo in allyDamageBar)
    //        {
    //            foo.maxPoint = maxValue;
    //            foo.Initialize();
    //        }
    //        foreach (var foo in enemyDamageBar)
    //        {
    //            foo.maxPoint = maxValue;
    //            foo.Initialize();
    //        }
    //        if (allyActions[selectAlly].animaData.Animadie)
    //        {
    //            if (enemy.animaData.Speed < allyActions[selectAlly].animaData.Speed)
    //            {
    //                turn[turnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
    //            }
    //            else
    //            {
    //                turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
    //            }
    //            for (int i = 0; i < tmpturnList.Count; i++)
    //            {
    //                if (ReferenceEquals(tmpturnList[i], allyActions[selectAlly].animaData))
    //                {
    //                    DestroyImmediate(turn[i]);
    //                    tmpturnList.RemoveAt(i);
    //                    turn.RemoveAt(i);
    //                    isTurn.RemoveAt(i);
    //                }
    //            }
    //            battleLogManager.AddLog($"{allyActions[selectAlly].animaData.Name}is dead", true);
    //            //dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
    //            //DestroyImmediate(allyBattleSetting.allyhpinstance[selectAlly]);
    //            //allyBattleSetting.allyhpinstance.RemoveAt(selectAlly);
    //            //allyHealthBar.RemoveAt(selectAlly);
    //            //allyActions.RemoveAt(selectAlly);
    //            //allyBattleSetting.animator.RemoveAt(selectAlly);
    //            //DestroyImmediate(allyBattleSetting.allyinstance[selectAlly]);
    //            //DestroyImmediate(allyBattleSetting.allyInfoInstance[selectAlly]);
    //            //allyBattleSetting.allyinstance.RemoveAt(selectAlly);
    //            //allyAnimaNum--;
    //            //turnList.Remove(allyActions[selectAlly].animaData);
    //            playerInfo.DieAnima(allyActions[selectAlly].animaData);
    //            dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
    //            turnList.Remove(allyActions[selectAlly].animaData);
    //            allyBattleSetting.allyhpinstance[allyActions[selectAlly].animaData.location].SetActive(false);
    //            allyBattleSetting.allyinstance[allyActions[selectAlly].animaData.location].SetActive(false);
    //            allyBattleSetting.allyInfoInstance[selectAlly].SetActive(false);
    //            allyAnimaNum--;


    //            if (allyAnimaNum == 0)
    //            {
    //                state = State.defeat;
    //                LoseBattle();
    //                StopCoroutine(runningCoroutine);

    //            }
    //        }
    //        else
    //        {
    //            turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
    //        }


    //}
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
}
