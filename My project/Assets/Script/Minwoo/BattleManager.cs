using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using DamageNumbersPro;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;
using System.Collections;
using System;
public class BattleManager : MonoBehaviour
{
    CameraManager cameraManager;
    PointerEventData pointerEventData;
    Coroutine runningCoroutine = null;
    State state;

    public PlayerInfo playerInfo;

    List<HealthBar> allyHealthBar;
    List<HealthBar> enemyHealthBar;
    
    protected EnemyBattleSetting enemyBattleSetting;
    protected AllyBattleSetting allyBattleSetting;
    
    protected List<AnimaActions> allyActions;
    protected List<EnemyActions> enemyActions;
    
    protected List<GameObject> ally;
    protected List<GameObject> enemy;
    
    List<int> dieAllyAnima;
    DamageNumber damageNumber;
    EventSystem eventSystem;

    Transform turnUI;
    List<GameObject> turn;
    List<AnimaDataSO> turnList;
    List<AnimaDataSO> tmpturnList;
    List<GameObject> isTurn;
    List<GameObject> allyInfo;
    List<GameObject> enemyInfo;

    GameObject battleManager;
    GameObject canvas;
    GameObject animaActionUI;
    GameObject arrow;

    TurnManager turnManager;

    UnityEngine.UI.Button attackButton;
    UnityEngine.UI.Button skillButton;
    bool isZKeyPressed = false;
    bool isXKeyPressed = false;

    float wheel;
    Vector3 originPoint;
    int turnIndex = 0;
    int enemyAnimaNum = 0;
    int allyAnimaNum = 0;
    int roundNum = 1;
    int selectEnemy = 0;
    void Start()
    {
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
        playerInfo.Initialize();

        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);

        battleManager = GameObject.Find("BattleManager");
        canvas = GameObject.Find("Main Battle UI");
        isTurn = new List<GameObject>();
        turnUI = GameObject.Find("Turn UI").transform;

        turn = new List<GameObject>();

        dieAllyAnima = new List<int>();

        state = State.start;

        AnimaActionUISetting();
        AllyBattlePrepare();
        EnemyBattlePrepare();
        BattleStart();

    }
    enum State
    {
        start, playerTurn, enemyTurn, win, defeat
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && attackButton.interactable && !isZKeyPressed)
        {
            ExecuteEvents.Execute(attackButton.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);

        }
        if (Input.GetKeyDown(KeyCode.X) && skillButton.interactable && !isXKeyPressed)
        {
            ExecuteEvents.Execute(skillButton.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
        }
    }

    void AllyBattlePrepare()
    {
        ally = new List<GameObject>();
        allyActions = new List<AnimaActions>();
        allyBattleSetting = gameObject.AddComponent<AllyBattleSetting>();
        allyHealthBar = new List<HealthBar>();
        allyInfo = new List<GameObject>();
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
            enemyInfo = new List<GameObject>();
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
            var allyStatus = GameObject.Find($"Ally{i}");
            allyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite= Resources.Load<Sprite>("Minwoo/Portrait/" + allyActions[i].animaData.Image);
            allyHealthBar.Add(GameObject.Find($"AllyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
            allyHealthBar[i].Initialize(allyActions[i].animaData.Stamina);
            GameObject.Find($"AllyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
            allyAnimaNum++;

        }
    }
    void initializeEnemyAnima()
    {
        for (int i = 0; i < enemyActions.Count; i++)
        {
            enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
            enemyActions[i].animaData.Initialize(enemyBattleSetting.battleEnemyAnima[i]);
            enemyActions[i].animaData.location = i;
            var allyStatus = GameObject.Find($"Enemy{i}");
            allyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + enemyActions[i].animaData.Image);
            enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Stamina);
            GameObject.Find($"EnemyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.level.ToString();
            enemyAnimaNum++;
        }
    }

     void AnimaActionUISetting()
    {
        Instantiate(Resources.Load<GameObject>("Minwoo/Anima Action UI"), GameObject.Find("Anima Action UI Frame").transform.position, Quaternion.identity, GameObject.Find("Anima Action UI Frame").transform);
        animaActionUI = GameObject.Find("Anima Action UI(Clone)");
        Transform attackAction = animaActionUI.transform.Find("Attack Button Frame").transform.Find("Attack Button");
        attackButton = attackAction.GetComponent<UnityEngine.UI.Button>();
        Transform skillAction = animaActionUI.transform.Find("Skill Button Frame").transform.Find("Skill Button");
        skillButton = skillAction.GetComponent<UnityEngine.UI.Button>();
        
        attackButton.onClick.AddListener(battleManager.GetComponent<BattleManager>().PlayerAttackButton);
        //skillButton.onClick.AddListener(battleManager.GetComponent<BattleManager>().PlayerSkillButton);
        animaActionUI.SetActive(false);

    }
    void RoundSetting()
    {
        GameObject.Find("Current Round").GetComponent<TextMeshProUGUI>().text = $"{roundNum++} Round";
    }
    private void BattleStart()
    {

        RoundSetting();
        turnManager = null;
        turnManager = ScriptableObject.CreateInstance<TurnManager>();
        turnManager.ResetTurnList();
        for (int i = 0; i < enemyActions.Count; i++)
        {
            turnManager.InsertAnimaData(enemyActions[i].animaData);
        }
        for (int i = 0; i < allyActions.Count; i++)
        {
            if (!allyActions[i].animaData.Animadie && allyBattleSetting.allyinstance[allyActions[i].animaData.location].activeSelf)
            {
                turnManager.InsertAnimaData(allyActions[i].animaData);
            }

        }
        turnList = turnManager.UpdateTurnList();
        tmpturnList = new List<AnimaDataSO>(turnList);

        turnIndex = 0;

        TurnUISetting(turnList);
        SetState(turnList);
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
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Player Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                int index = turn[i].name.IndexOf("(Clone)");
                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                turnUI.transform.Find($"Player Turn Slot{i}").transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Image);
                isTurn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Player Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Player Turn Slot{i}")));
                index = isTurn[i].name.IndexOf("(Clone)");
                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                GameObject.Find($"IsTurn{i}").SetActive(false);
            }
            else
            {
                turn.Add(UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Minwoo/Enemy Turn Slot"), turnUI.transform.position, Quaternion.identity, turnUI));
                turn[i].transform.Rotate(0, 180f, 0);
                int index = turn[i].name.IndexOf("(Clone)");
                turn[i].name = turn[i].name.Substring(0, index) + "" + i;
                turnUI.transform.Find($"Enemy Turn Slot{i}").transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Image);
                isTurn.Add(Instantiate(Resources.Load<GameObject>("Minwoo/IsTurn"), turnUI.transform.Find($"Enemy Turn Slot{i}").transform.position, Quaternion.identity, turnUI.transform.Find($"Enemy Turn Slot{i}")));
                index = isTurn[i].name.IndexOf("(Clone)");
                isTurn[i].name = isTurn[i].name.Substring(0, index) + "" + i;
                GameObject.Find($"IsTurn{i}").SetActive(false);
            }
        }
    }
    void SetState(List<AnimaDataSO> turnList)
    {
        if (turnList[0].isAlly)
        {

            animaActionUI.SetActive(true);
            isTurn[turnIndex].SetActive(true);
            int index = -1;
            for (int i = 0; i < allyActions.Count; i++)
            {
                if (ReferenceEquals(turnList[0], allyActions[i].animaData))
                {
                    Debug.Log("이새끼 같음");
                    index = i;
                }
            }
            Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.allyinstance[allyActions[index].animaData.location].transform.position.x, allyBattleSetting.allyinstance[allyActions[index].animaData.location].transform.position.y + 1.65f), Quaternion.identity);
            arrow = GameObject.Find("Arrow_down(Clone)");
            GameObject.Find("Anima Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[0].Image);
            GameObject.Find("Currunt Anima Name").GetComponent<TextMeshProUGUI>().text = turnList[0].Name;
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skill\n{turnList[0].Skill_pp}/{turnList[0].MaxSkill_pp}";
            if (turnList[0].Skill_pp == 0)
            {
                skillButton.interactable = false;
            }
            else
            {
                skillButton.interactable = true;
            }
            state = State.playerTurn;
        }
        
        else
        {
            state = State.enemyTurn;
            isTurn[turnIndex].SetActive(true);
        }
        
    }
    void PlayerAttackButton()
    {
        print("플레이어 턴");
        selectEnemy = 0;

        if (state != State.playerTurn)
        {
            return;
        }
        if (state == State.playerTurn)
        {
            isZKeyPressed = true;
            DestroyImmediate(arrow);
            runningCoroutine = StartCoroutine(PlayerAttack(selectEnemy));
            attackButton.interactable = false;
            skillButton.interactable = false;
        }

    }
    IEnumerator PlayerAttack(int selectEnemy)
    {
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        int index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.65f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (enemyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.65f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.65f);
            }
            else if (Input.GetKeyDown(KeyCode.Z) && !attackButton.interactable)
            {
                selectEnemy = index;
                DestroyImmediate(arrow);
                yield return new WaitForSeconds(Time.deltaTime * 30);
                break;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                DestroyImmediate(arrow);
                yield return new WaitForSeconds(Time.deltaTime * 30);
                isZKeyPressed = false;
                attackButton.interactable = true;
                skillButton.interactable = true;
                SetState(turnList);
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
                yield break;
            }
            yield return null;
        }

        foreach (AnimaActions anima in allyActions)
        {
            if (turnList.Count == 0)
            {
                break;
            }
            if (ReferenceEquals(turnList[0], anima.animaData))
            {
                isZKeyPressed = false;
                attackButton.interactable = true;
                skillButton.interactable = true;
                animaActionUI.SetActive(false);
                isTurn[turnIndex].SetActive(false);
                turnList.RemoveAt(0);
                canvas.SetActive(false);//체력 바 동기화 문제 발생 예상
                /* Attack */

                cameraManager.ZoomIn(allyBattleSetting.allyinstance[allyActions.IndexOf(anima)].transform);

                /* Animation */
                
                yield return new WaitForSeconds(3f);
                cameraManager.CameraMove(allyBattleSetting.allyinstance[allyActions.IndexOf(anima)].transform, enemyBattleSetting.enemyinstance[selectEnemy].transform);
                yield return new WaitForSeconds(3f);
                cameraManager.ZoomOut();
                canvas.SetActive(true);
                anima.Attack(anima, enemyActions[selectEnemy], enemyHealthBar[selectEnemy]);
                if (enemyActions[selectEnemy].animaData.Animadie)
                {
                    if (anima.animaData.Speed <= enemyActions[selectEnemy].animaData.Speed)
                    {
                        turn[turnIndex].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                    }
                    else
                    {
                        turn[turnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                    }
                    for (int i = 0; i < tmpturnList.Count; i++)
                    {
                        if (ReferenceEquals(tmpturnList[i], enemyActions[selectEnemy].animaData))
                        {
                            DestroyImmediate(turn[i]);
                            tmpturnList.RemoveAt(i);
                            turn.RemoveAt(i);
                            isTurn.RemoveAt(i);
                            if (UnityEngine.Random.Range(0, 101) <= enemyActions[selectEnemy].animaData.DropRate)
                            {
                                AnimaDataSO animadata = ScriptableObject.CreateInstance<AnimaDataSO>();
                                animadata.GetAnima(enemyActions[selectEnemy].animaData.Name);
                                allyBattleSetting.playerinfo.GetAnima(animadata);
                            }
                        }
                    }
                    //LoadGold.UpdateGold(enemyActions[selectEnemy].animaData.DropGold);
                    turnList.Remove(enemyActions[selectEnemy].animaData);
                    DestroyImmediate(enemyBattleSetting.enemyhpinstance[selectEnemy]);
                    enemyBattleSetting.enemyhpinstance.RemoveAt(selectEnemy);
                    enemyHealthBar.RemoveAt(selectEnemy);
                    enemyActions.RemoveAt(selectEnemy);
                    enemyBattleSetting.animator.RemoveAt(selectEnemy);
                    DestroyImmediate(enemyBattleSetting.enemyinstance[selectEnemy]);
                    enemyBattleSetting.enemyinstance.RemoveAt(selectEnemy);
                    enemyAnimaNum--;
                    if (enemyActions.Count == 0)
                    {
                        foreach (var ally in allyActions)
                        {
                            ally.animaData.location = -1;
                        }
                        state = State.win;
                        print("승리");
                        turnIndex = 0;
                        //winBattle();
                        StopCoroutine(runningCoroutine);
                    }

                }
                else
                {
                    turn[turnIndex++].transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                }
                break;
            }
        }
        if (enemyActions.Count > 0 && turnList.Count == 0)
        {
            runningCoroutine = null;
            //BattleStart();
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }

    }
    public List<AnimaActions> getAlly()
    {
        return allyActions;
    }
    public List<EnemyActions> getEnemy()
    {
        return enemyActions;
    }
}



