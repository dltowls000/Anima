using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using DamageNumbersPro;
using TMPro;
public class BattleManager : MonoBehaviour
{
    public PlayerInfo playerInfo;

    List<HealthBar> allyHealthBar;
    List<HealthBar> enemyHealthBar;
    
    EnemyBattleSetting enemyBattleSetting;
    AllyBattleSetting allyBattleSetting;
    
    List<AnimaActions> allyActions;
    List<EnemyActions> enemyActions;
    
    List<GameObject> ally;
    List<GameObject> enemy;
    
    List<int> dieAllyAnima;
    DamageNumber damageNumber; 
    
    Transform turnUI;
    List<GameObject> turn;
    List<AnimaDataSO> turnList;
    List<AnimaDataSO> tmpturnList;
    List<GameObject> isTurn;

    GameObject canvas;

    int enemyAnimaNum = 0;
    int allyAnimaNum = 0;
    int roundNum = 1;
    void Start()
    {
        playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
        playerInfo.Initialize();
        
        canvas = GameObject.Find("BattleCanvas");
        isTurn = new List<GameObject>();
        turnUI = GameObject.Find("TurnBar").transform;
        
        turn = new List<GameObject>();

        dieAllyAnima = new List<int>();
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
        if (enemy != null &&  enemy.Count > 0)
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

            allyHealthBar.Add(GameObject.Find($"AllyAnimaHP{i}").GetComponent<HealthBar>());
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
            enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").GetComponent<HealthBar>());
            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Stamina);
            //GameObject.Find($"EnemyAnimaInfo{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
            enemyAnimaNum++;
        }
    }

    void RoundSetting()
    {
        GameObject.Find("Current Round").GetComponent<TextMeshProUGUI>().text = $"{roundNum++} Round";
    }

    void TurnUISetting(List<AnimaDataSO> turnList)
    {
        if(turn.Count != 0)
        {
            for(int i = 0; i < turn.Count;)
            {
                DestroyImmediate(turn[i]);
                turn.RemoveAt(i);
                isTurn.RemoveAt(i);
            }
        }
        for (int i = 0; i < turnList.Count; i++)
        {
            if (turnList[i].isAlly)
            {
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/Player Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                int index = turn[i].name.IndexOf("(Clone)");
                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                turnUI.transform.Find($"Player Turn Slot{i}").transform.Find("Player Turn Portrait").GetComponent<Image>().sprite = Resources.Load<Sprite>("Portrait/Small Portrait/" + turnList[i].Image);
                isTurn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/IsTurn"), turnUI.transform.Find($"Player Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Player Turn Slot{i}")));
                index = isTurn[i].name.IndexOf("(Clone)");
                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                GameObject.Find($"IsTurn{i}").SetActive(false);
            }
            else
            {
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefab/Enemy Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                turn[i].transform.Rotate(0, 180f, 0);
                int index = turn[i].name.IndexOf("(Clone)");
                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                turnUI.transform.Find($"Enemy Turn Slot{i}").transform.Find("Enemy Turn Portrait").GetComponent<Image>().sprite = Resources.Load<Sprite>("Portrait/Small Portrait/" + turnList[i].Image);
                isTurn.Add(Instantiate(Resources.Load<GameObject>("Prefab/IsTurn"), turnUI.transform.Find($"Enemy Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Enemy Turn Slot{i}")));
                index = isTurn[i].name.IndexOf("(Clone)");
                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                GameObject.Find($"IsTurn{i}").SetActive(false);
            }
        }
    }

}
