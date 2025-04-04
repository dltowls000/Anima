using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using DamageNumbersPro;
using TMPro;
public class BattleManager : MonoBehaviour
{
    List<HealthBar> allyHealthBar;
    List<HealthBar> enemyHealthBar;
    EnemyBattleSetting enemyBattleSetting;
    AllyBattleSetting allyBattleSetting;
    List<AnimaActions> allyActions;
    List<EnemyActions> enemyActions;
    List<GameObject> ally;
    List<GameObject> enemy;
    List<GameObject> turn;
    List<AnimaDataSO> turnList;
    List<AnimaDataSO> tmpturnList;
    List<int> dieAllyAnima;
    DamageNumber damageNumber;
    PlayerInfo playerInfo;

    int enemyAnimaNum = 0;
    int allyAnimaNum = 0;

    void Start()
    {
        //Instantiate<PlayerInfo>()
        playerInfo.Initialize();
        dieAllyAnima = new List<int>();
        turn = new List<GameObject>();
        AllyBattlePrepare();
        EnemyBattlePrepare();
    }

    void AllyBattlePrepare()
    {
        ally = new List<GameObject>();
        allyActions = new List<AnimaActions>();
        allyBattleSetting = gameObject.AddComponent<AllyBattleSetting>();
        allyHealthBar = new List<HealthBar>();
        allyBattleSetting.initialize();
        allyBattleSetting.SpawnAlly();
        setAllyanima();
        setAllyActions();
        initializeAllyAnima();
    }
    void EnemyBattlePrepare()
    {
        enemyAnimaNum = 0;
        if (enemy.Count > 0)
        {
            enemy.Clear();
            enemyActions.Clear();
            enemyHealthBar.Clear();

        }
        else
        {
            enemy = new List<GameObject>();
            enemyActions = new List<EnemyActions>();
            enemyHealthBar = new List<HealthBar>();
        }
        enemyBattleSetting = gameObject.AddComponent<EnemyBattleSetting>();
        enemyBattleSetting.SpawnEnemy();
        setEnemyanima();
        setEnemyActions();
        initializeEnemyAnima();
    }
    void setAllyanima()
    {
        for (int i = 0; i < allyBattleSetting.allyobjPrefab.Count; i++)
        {
            ally.Add(allyBattleSetting.allyobjPrefab[i]);
        }
    }
    void setEnemyanima()
    {
        for (int i = 0; i < enemyBattleSetting.enemyobjPrefab.Count; i++)
        {
            enemy.Add(enemyBattleSetting.enemyobjPrefab[i]);
        }
    }
    void setAllyActions()
    {
        for (int i = 0; i < ally.Count; i++)
        {
            allyActions.Add(ally[i].AddComponent<AnimaActions>());
        }
    }
    void setEnemyActions()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            enemyActions.Add(enemy[i].AddComponent<EnemyActions>());
            enemyActions[i].InitializeWeights();
        }
    }
    void initializeAllyAnima()
    {
        dieAllyAnima.Clear();
        for (int i = 0; i < allyActions.Count; i++)
        {

            allyActions[i].animaData = allyBattleSetting.playerinfo.battleAnima[i];
            allyActions[i].animaData.isAlly = true;

            allyHealthBar.Add(GameObject.Find($"AllyAnimaInfo{i}").GetComponent<HealthBar>());
            allyHealthBar[i].Initialize(allyActions[i].animaData.Stamina);
            //GameObject.Find($"AllyAnimaInfo{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
            allyAnimaNum++;

        }
    }
    void initializeEnemyAnima()
    {
        for (int i = 0; i < enemyActions.Count; i++)
        {
            enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
            enemyActions[i].animaData.Initialize(enemyBattleSetting.battleEnemyAnima[i]);
            enemyHealthBar.Add(GameObject.Find($"EnemyAnimaInfo{i}").transform.Find("HP").GetComponent<HealthBar>());
            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Stamina);
            GameObject.Find($"EnemyAnimaInfo{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
            enemyAnimaNum++;
        }
    }
}
