using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using BansheeGz.BGDatabase;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private CameraManager cameraManager;
    [SerializeField]
    private Transform turnUI;
    [SerializeField]
    private BattleLogManager battleLogManager;
    [SerializeField]
    private GameObject battleManager;
    [SerializeField]
    private GameObject canvas;
    
    
    PointerEventData pointerEventData;
    Coroutine runningCoroutine = null;
    State state;
    
    public PlayerInfo playerInfo;

    List<HealthBar> allyHealthBar;
    List<HealthBar> enemyHealthBar;
    List<ParserBar> allyDamageBar;
    List<ParserBar> enemyDamageBar;
    List<ParserBar> allyHealBar;
    List<ParserBar> enemyHealBar;
    List<TextMeshProUGUI> allyDamageText;
    List<TextMeshProUGUI> enemyDamageText;
    List<TextMeshProUGUI> allyHealText;
    List<TextMeshProUGUI> enemyHealText;

    EnemyBattleSetting enemyBattleSetting;
    AllyBattleSetting allyBattleSetting;
    
    List<AnimaActions> allyActions;
    List<EnemyActions> enemyActions;
    
    List<GameObject> ally;
    List<GameObject> enemy;
    
    List<int> dieAllyAnima;
    [SerializeField]
    DamageNumber damageNumber;
    EventSystem eventSystem;

    
    List<GameObject> turn;
    List<AnimaDataSO> turnList;
    List<AnimaDataSO> tmpturnList;
    List<GameObject> isTurn;
    List<GameObject> allyInfo;
    List<GameObject> enemyInfo;
    List<AnimaDataSO> dropAnima;
    
    GameObject animaActionUI;
    GameObject arrow;
    GameObject rebuild;

    TurnManager turnManager;

    UnityEngine.UI.Button attackButton;
    UnityEngine.UI.Button skillButton;
    bool isZKeyPressed = false;
    bool isXKeyPressed = false;
    BGRepo database;
    BGMetaEntity animaTable;
    int turnIndex = 0;
    int enemyAnimaNum = 0;
    int allyAnimaNum = 0;
    int roundNum = 1;
    int selectEnemy = 0;
    float maxValue = 0;
    void Start()
    {
        playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
        playerInfo.Initialize();
        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);
        
        isTurn = new List<GameObject>();

        turn = new List<GameObject>();

        dieAllyAnima = new List<int>();
        dropAnima = new List<AnimaDataSO>();
        state = State.start;
        database = BGRepo.I;
        animaTable = database.GetMeta("Anima");
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
        allyDamageBar = new List<ParserBar>();
        //allyHealBar = new List<ParserBar>();
        allyDamageText = new List<TextMeshProUGUI>();
        //allyHealText = new List<TextMeshProUGUI>();
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
            enemyDamageBar = new List<ParserBar>();
            //enemyHealBar = new List<ParserBar>();
            enemyDamageText = new List<TextMeshProUGUI>();
            //enemyHealText = new List<TextMeshProUGUI>();
            enemyInfo = new List<GameObject>();
        }
        enemyBattleSetting = gameObject.AddComponent<EnemyBattleSetting>();
        enemyBattleSetting.stage = SceneManager.GetActiveScene().name.Substring(0, SceneManager.GetActiveScene().name.IndexOf("Battle"));
        int n = 0;
        foreach(var tmp in playerInfo.haveAnima)
        {
            if(tmp.level > n) n = tmp.level;
        }
        enemyBattleSetting.SpawnEnemy(n);
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
            allyActions[i].animaData.location = i;
            var allyStatus = GameObject.Find($"Ally{i}");
            var allyParser = GameObject.Find($"Ally{i}Name");
            allyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite= Resources.Load<Sprite>("Minwoo/Portrait/" + allyActions[i].animaData.Objectfile);
            allyHealthBar.Add(GameObject.Find($"AllyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
            
            allyDamageBar.Add(allyParser.transform.Find($"A{i}Damage").transform.Find($"A{i} Damage Bar").GetComponent<ParserBar>());
            //allyHealBar.Add(allyParser.transform.Find($"A{i}Heal").transform.Find($"A{i} Heal Bar").GetComponent<ParserBar>());
            allyHealthBar[i].Initialize(allyActions[i].animaData.Stamina);
            allyDamageBar[i].Initialize();
            //allyHealBar[i].Initialize();
            allyParser.GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.Name;
            allyDamageText.Add(allyParser.transform.Find($"A{i}Damage").GetComponent<TextMeshProUGUI>());
            //allyHealText.Add(allyParser.transform.Find($"A{i}Heal").GetComponent<TextMeshProUGUI>());
            GameObject.Find($"AllyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
            allyAnimaNum++;

        }
    }
    void initializeEnemyAnima()
    {
        int level = 0;
        
        
        foreach (var anima in playerInfo.haveAnima)
        {
            if(anima.level >= level)
            {
                level = anima.level;
            }
        }
        switch (enemyActions.Count)
        {
            case 1:
                level -= 1;
                break;
            case 2:
                level -= 2;
                break;
            case 3:
                level -= 3;
                break;
        }
        for (int i = 0; i < enemyActions.Count; i++)
        {
            enemyActions[i].animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
            enemyActions[i].animaData.Initialize(enemyBattleSetting.battleEnemyAnima[i],level);
            animaTable.ForEachEntity(entity =>
            {
                if (entity.Get<string>("name") == enemyActions[i].animaData.Name && entity.Get<int>("Meeted") == 0 )
                {
                    entity.Set<int>("Meeted", 1);
                    DBUpdater.Save();
                }

            });
            enemyActions[i].animaData.location = i;
            enemyActions[i].animaData.enemyIndex = i;
            var enemyStatus = GameObject.Find($"Enemy{i}");
            var enemyParser = GameObject.Find($"Enemy{i}Name");
            enemyStatus.transform.Find("Image").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + enemyActions[i].animaData.Objectfile);
            enemyHealthBar.Add(GameObject.Find($"EnemyAnimaHP{i}").transform.Find("HP").GetComponent<HealthBar>());
            enemyDamageBar.Add(enemyParser.transform.Find($"E{i}Damage").transform.Find($"E{i} Damage Bar").GetComponent<ParserBar>());
            //enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Stamina);
            enemyDamageBar[i].Initialize();
            //enemyHealBar[i].Initialize();
            enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
            enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
            //enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
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
        skillButton.onClick.AddListener(battleManager.GetComponent<BattleManager>().PlayerSkillButton);
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
                turnUI.transform.Find($"Player Turn Slot{i}").transform.Find("Player Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Objectfile);
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
                turnUI.transform.Find($"Enemy Turn Slot{i}").transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[i].Objectfile);
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
                    index = i;
                }
            }
            Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.allyinstance[allyActions[index].animaData.location].transform.position.x, allyBattleSetting.allyinstance[allyActions[index].animaData.location].transform.position.y + 1.2f), Quaternion.identity);
            arrow = GameObject.Find("Arrow_down(Clone)");
            GameObject.Find("Anima Portrait").GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + turnList[0].Objectfile);
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
            runningCoroutine = StartCoroutine(EnemyTurn());
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
    void PlayerSkillButton()
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
            runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy));
            attackButton.interactable = false;
            skillButton.interactable = false;
        }

    }
    IEnumerator PlayerAttack(int selectEnemy)
    {
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        int index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.2f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (enemyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.2f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.2f);
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


                /* Animation */
                yield return cameraManager.ZoomIn(allyBattleSetting.allyinstance[allyActions.IndexOf(anima)].transform, enemyBattleSetting.enemyinstance[selectEnemy].transform, true, anima.animaData.attackName);
                canvas.SetActive(true);
                yield return anima.Attack(anima, enemyActions[selectEnemy], enemyHealthBar[selectEnemy], allyDamageBar[allyActions.IndexOf(anima)]);
                damageNumber.Spawn(new Vector2(enemyBattleSetting.enemyinstance[selectEnemy].transform.position.x -0.1f, enemyBattleSetting.enemyinstance[selectEnemy].transform.position.y + 0.1f), enemyActions[selectEnemy].damage);
                battleLogManager.AddLog($"{anima.animaData.Name} hit {enemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(enemyActions[selectEnemy].damage)}damage", true);
                allyDamageText[allyActions.IndexOf(anima)].text = Mathf.Ceil(allyDamageBar[allyActions.IndexOf(anima)].thisPoint).ToString();
                foreach (var max in allyDamageBar)
                {
                    if (maxValue < max.maxPoint)
                    {
                        maxValue = max.maxPoint;
                    }
                }
                foreach (var max in enemyDamageBar)
                {
                    if (maxValue < max.maxPoint)
                    {
                        maxValue = max.maxPoint;
                    }
                }
                foreach (var foo in allyDamageBar)
                {
                    foo.maxPoint = maxValue;
                    foo.Initialize();
                }
                foreach (var foo in enemyDamageBar)
                {
                    foo.maxPoint = maxValue;
                    foo.Initialize();
                }

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
                                animadata.GetAnima(enemyActions[selectEnemy].animaData.Name, enemyActions[selectEnemy].animaData.level);
                                allyBattleSetting.playerinfo.GetAnima(animadata);
                                dropAnima.Add(animadata);
                                animaTable.ForEachEntity(entity =>
                                {
                                    if (entity.Get<string>("name") == enemyActions[selectEnemy].animaData.Name)
                                    {
                                        entity.Set<int>("Meeted", 2);
                                        database.Addons.Get<BGAddonSaveLoad>().Save();
                                    }
                                });
                            }
                            foreach (var tmp in allyActions)
                            {
                                if (!tmp.animaData.Animadie)
                                {
                                    tmp.animaData.LevelUp();
                                    GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                                }
                            }
                        }
                    }
                    battleLogManager.AddLog($"{enemyActions[selectEnemy].animaData.Name}is dead", false);
                    //GoldManager.Instance.AddGold(enemyActions[selectEnemy].animaData.DropGold);
                    turnList.Remove(enemyActions[selectEnemy].animaData);
                    DestroyImmediate(enemyBattleSetting.enemyhpinstance[selectEnemy]);
                    enemyBattleSetting.enemyhpinstance.RemoveAt(selectEnemy);
                    enemyHealthBar.RemoveAt(selectEnemy);
                    enemyActions.RemoveAt(selectEnemy);
                    enemyBattleSetting.animator.RemoveAt(selectEnemy);
                    DestroyImmediate(enemyBattleSetting.enemyinstance[selectEnemy]);
                    DestroyImmediate(enemyBattleSetting.enemyInfoInstance[selectEnemy]);
                    enemyBattleSetting.enemyinstance.RemoveAt(selectEnemy);
                    enemyAnimaNum--;
                    for (int i = 0; i < 3; i++)
                    {
                        rebuild = GameObject.Find($"Enemy{i}");
                        if (rebuild != null)
                        {
                            rebuild.transform.Find("Status").GetComponent<StatusSync>().dieanima++;
                        }
                    }

                    if (enemyActions.Count == 0)
                    {
                        foreach (var ally in allyActions)
                        {
                            ally.animaData.location = -1;
                        }
                        state = State.win;
                        turnIndex = 0;
                        WinBattle();
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
            BattleStart();
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }

    }

    IEnumerator PlayerSkill(int selectEnemy)
    {
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        int index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.2f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (enemyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y +1.2f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(enemyBattleSetting.enemyinstance[index].transform.position.x, enemyBattleSetting.enemyinstance[index].transform.position.y + 1.2f);
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

                yield return cameraManager.ZoomIn(allyBattleSetting.allyinstance[allyActions.IndexOf(anima)].transform, enemyBattleSetting.enemyinstance[selectEnemy].transform, true, anima.animaData.skillName[0]);

                /* Animation */
                canvas.SetActive(true);
                yield return anima.Skill(anima, enemyActions[selectEnemy], enemyHealthBar[selectEnemy], allyDamageBar[allyActions.IndexOf(anima)]);
                damageNumber.Spawn(new Vector2(enemyBattleSetting.enemyinstance[selectEnemy].transform.position.x - 0.1f, enemyBattleSetting.enemyinstance[selectEnemy].transform.position.y + 0.1f), enemyActions[selectEnemy].damage);
                battleLogManager.AddLog($"{anima.animaData.Name} used \"{ anima.animaData.skillName}\" on {enemyActions[selectEnemy].animaData.Name} for {Mathf.Ceil(enemyActions[selectEnemy].damage)}damage", true);
                allyDamageText[allyActions.IndexOf(anima)].text = Mathf.Ceil(allyDamageBar[allyActions.IndexOf(anima)].thisPoint).ToString();
                foreach (var max in allyDamageBar)
                {
                    if (maxValue < max.maxPoint)
                    {
                        maxValue = max.maxPoint;
                    }
                }
                foreach (var max in enemyDamageBar)
                {
                    if (maxValue < max.maxPoint)
                    {
                        maxValue = max.maxPoint;
                    }
                }
                foreach (var foo in allyDamageBar)
                {
                    foo.maxPoint = maxValue;
                    foo.Initialize();
                }
                foreach (var foo in enemyDamageBar)
                {
                    foo.maxPoint = maxValue;
                    foo.Initialize();
                }
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
                                animadata.GetAnima(enemyActions[selectEnemy].animaData.Name, enemyActions[selectEnemy].animaData.level);
                                allyBattleSetting.playerinfo.GetAnima(animadata);
                                animaTable.ForEachEntity(entity =>
                                {
                                    if (entity.Get<string>("name") == enemyActions[selectEnemy].animaData.Name)
                                    {
                                        entity.Set<int>("Meeted", 2);
                                        DBUpdater.Save();
                                    }
                                });
                            }
                            foreach (var tmp in allyActions)
                            {
                                if (!tmp.animaData.Animadie)
                                {
                                    tmp.animaData.LevelUp();
                                    GameObject.Find($"AllyAnimaHP{tmp.animaData.location}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = tmp.animaData.level.ToString();
                                }
                            }
                        }
                    }
                    battleLogManager.AddLog($"{enemyActions[selectEnemy].animaData.Name}is dead", false);
                    //GoldManager.Instance.AddGold(enemyActions[selectEnemy].animaData.DropGold);
                    turnList.Remove(enemyActions[selectEnemy].animaData);
                    DestroyImmediate(enemyBattleSetting.enemyhpinstance[selectEnemy]);
                    enemyBattleSetting.enemyhpinstance.RemoveAt(selectEnemy);
                    enemyHealthBar.RemoveAt(selectEnemy);
                    enemyActions.RemoveAt(selectEnemy);
                    enemyBattleSetting.animator.RemoveAt(selectEnemy);
                    DestroyImmediate(enemyBattleSetting.enemyinstance[selectEnemy]);
                    DestroyImmediate(enemyBattleSetting.enemyInfoInstance[selectEnemy]);
                    enemyBattleSetting.enemyInfoInstance.RemoveAt(selectEnemy);
                    enemyBattleSetting.enemyinstance.RemoveAt(selectEnemy);
                    enemyAnimaNum--;
                    
                    for (int i = 0; i < 3; i++)
                    {
                        rebuild = GameObject.Find($"Enemy{i}");
                        if (rebuild != null)
                        {
                            rebuild.transform.Find("Status").GetComponent<StatusSync>().dieanima++;
                        }
                    }
                    
                    if (enemyActions.Count == 0)
                    {
                        foreach (var ally in allyActions)
                        {
                            ally.animaData.location = -1;
                        }
                        state = State.win;
                        print("승리");
                        turnIndex = 0;
                        WinBattle();
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
            BattleStart();
        }
        else if (enemyActions.Count > 0 && turnList.Count != 0)
        {
            runningCoroutine = null;
            SetState(turnList);
        }

    }
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        int selectAlly = selectNoDieAnima();

        foreach (EnemyActions enemy in enemyActions)
        {
            if (turnList.Count == 0)
            {
                break;
            }

            if (ReferenceEquals(turnList[0], enemy.animaData))
            {
                enemy.DecideAction();
                if (enemy.performance.Equals("Attack"))
                {
                    isTurn[turnIndex].SetActive(false);
                    turnList.RemoveAt(0);
                    canvas.SetActive(false);//체력 바 동기화 문제 발생 예상
                    /* Attack */


                    /* Animation */
                    yield return cameraManager.ZoomIn(enemyBattleSetting.enemyinstance[enemyActions.IndexOf(enemy)].transform, allyBattleSetting.allyinstance[selectAlly].transform, false, enemy.animaData.attackName);
                    canvas.SetActive(true);
                    
                    yield return enemy.Attack(enemy, allyActions[selectAlly], allyHealthBar[selectAlly], enemyDamageBar[enemy.animaData.enemyIndex]);
                    damageNumber.Spawn(new Vector2(allyBattleSetting.allyinstance[selectAlly].transform.position.x - 0.1f, allyBattleSetting.allyinstance[selectAlly].transform.position.y + 0.1f), allyActions[selectAlly].damage);
                    battleLogManager.AddLog($"{enemy.animaData.Name} hit {allyActions[selectAlly].animaData.Name} for {Mathf.Ceil(allyActions[selectAlly].damage)} damage", false);
                    enemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(enemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
                    
                    foreach( var max in allyDamageBar)
                    {
                        if(maxValue < max.maxPoint)
                        {
                            maxValue = max.maxPoint;
                        }
                    }
                    foreach( var max in enemyDamageBar)
                    {
                        if (maxValue < max.maxPoint)
                        {
                            maxValue = max.maxPoint;
                        }
                    }
                    foreach (var foo in allyDamageBar)
                    {
                        foo.maxPoint = maxValue;
                        foo.Initialize();
                    }
                    foreach (var foo in enemyDamageBar)
                    {
                        foo.maxPoint = maxValue;
                        foo.Initialize();
                    }
                    if (allyActions[selectAlly].animaData.Animadie)
                    {
                        if (enemy.animaData.Speed < allyActions[selectAlly].animaData.Speed)
                        {
                            turn[turnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                        }
                        else
                        {
                            turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                        }
                        for (int i = 0; i < tmpturnList.Count; i++)
                        {
                            if (ReferenceEquals(tmpturnList[i], allyActions[selectAlly].animaData))
                            {
                                DestroyImmediate(turn[i]);
                                tmpturnList.RemoveAt(i);
                                turn.RemoveAt(i);
                                isTurn.RemoveAt(i);
                            }
                        }
                        //dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
                        //DestroyImmediate(allyBattleSetting.allyhpinstance[selectAlly]);
                        //allyBattleSetting.allyhpinstance.RemoveAt(selectAlly);
                        //allyHealthBar.RemoveAt(selectAlly);
                        //allyBattleSetting.animator.RemoveAt(selectAlly);
                        //DestroyImmediate(allyBattleSetting.allyinstance[selectAlly]);
                        //DestroyImmediate(allyBattleSetting.allyInfoInstance[selectAlly]);
                        //allyBattleSetting.allyinstance.RemoveAt(selectAlly);
                        //allyAnimaNum--;
                        //turnList.Remove(allyActions[selectAlly].animaData);
                        battleLogManager.AddLog($"{allyActions[selectAlly].animaData.Name}is dead", true);
                        dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
                        turnList.Remove(allyActions[selectAlly].animaData);
                        allyBattleSetting.allyhpinstance[allyActions[selectAlly].animaData.location].SetActive(false);
                        allyBattleSetting.allyinstance[allyActions[selectAlly].animaData.location].SetActive(false);
                        allyBattleSetting.allyInfoInstance[selectAlly].SetActive(false);
                        allyAnimaNum--;
                        
                        if (allyAnimaNum == 0)
                        {
                            state = State.defeat;
                            print("패배");
                            LoseBattle();
                            StopCoroutine(runningCoroutine);


                        }
                    }
                    else
                    {
                        turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);

                    }

                }
                else if (enemy.performance.Equals("Skill"))
                {
                    yield return new WaitForSeconds(0.5f);
                    isTurn[turnIndex].SetActive(false);
                    turnList.RemoveAt(0);
                    canvas.SetActive(false);//체력 바 동기화 문제 발생 예상
                    /* Attack */


                    /* Animation */

                    yield return cameraManager.ZoomIn(enemyBattleSetting.enemyinstance[enemyActions.IndexOf(enemy)].transform, allyBattleSetting.allyinstance[selectAlly].transform, false, enemy.animaData.skillName[0]);
                    canvas.SetActive(true);
                    yield return enemy.Skill(enemy, allyActions[selectAlly], allyHealthBar[selectAlly], enemyDamageBar[enemy.animaData.enemyIndex]);
                    damageNumber.Spawn(new Vector2(allyBattleSetting.allyinstance[selectAlly].transform.position.x - 0.1f, allyBattleSetting.allyinstance[selectAlly].transform.position.y + 0.1f), allyActions[selectAlly].damage);
                    battleLogManager.AddLog($"{enemy.animaData.Name} used \"{enemy.animaData.skillName}\" on {allyActions[selectAlly].animaData.Name} for {Mathf.Ceil(allyActions[selectAlly].damage)} damage", false);
                    enemyDamageText[enemy.animaData.enemyIndex].text = Mathf.Ceil(enemyDamageBar[enemy.animaData.enemyIndex].thisPoint).ToString();
                    foreach (var max in allyDamageBar)
                    {
                        if (maxValue < max.maxPoint)
                        {
                            maxValue = max.maxPoint;
                        }
                    }
                    foreach (var max in enemyDamageBar)
                    {
                        if (maxValue < max.maxPoint)
                        {
                            maxValue = max.maxPoint;
                        }
                    }
                    foreach (var foo in allyDamageBar)
                    {
                        foo.maxPoint = maxValue;
                        foo.Initialize();
                    }
                    foreach (var foo in enemyDamageBar)
                    {
                        foo.maxPoint = maxValue;
                        foo.Initialize();
                    }
                    if (allyActions[selectAlly].animaData.Animadie)
                    {
                        if (enemy.animaData.Speed < allyActions[selectAlly].animaData.Speed)
                        {
                            turn[turnIndex].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                        }
                        else
                        {
                            turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                        }
                        for (int i = 0; i < tmpturnList.Count; i++)
                        {
                            if (ReferenceEquals(tmpturnList[i], allyActions[selectAlly].animaData))
                            {
                                DestroyImmediate(turn[i]);
                                tmpturnList.RemoveAt(i);
                                turn.RemoveAt(i);
                                isTurn.RemoveAt(i);
                            }
                        }
                        battleLogManager.AddLog($"{allyActions[selectAlly].animaData.Name}is dead", true);
                        //dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
                        //DestroyImmediate(allyBattleSetting.allyhpinstance[selectAlly]);
                        //allyBattleSetting.allyhpinstance.RemoveAt(selectAlly);
                        //allyHealthBar.RemoveAt(selectAlly);
                        //allyActions.RemoveAt(selectAlly);
                        //allyBattleSetting.animator.RemoveAt(selectAlly);
                        //DestroyImmediate(allyBattleSetting.allyinstance[selectAlly]);
                        //DestroyImmediate(allyBattleSetting.allyInfoInstance[selectAlly]);
                        //allyBattleSetting.allyinstance.RemoveAt(selectAlly);
                        //allyAnimaNum--;
                        //turnList.Remove(allyActions[selectAlly].animaData);
                        dieAllyAnima.Add(allyActions.IndexOf(allyActions[selectAlly]));
                        turnList.Remove(allyActions[selectAlly].animaData);
                        allyBattleSetting.allyhpinstance[allyActions[selectAlly].animaData.location].SetActive(false);
                        allyBattleSetting.allyinstance[allyActions[selectAlly].animaData.location].SetActive(false);
                        allyBattleSetting.allyInfoInstance[selectAlly].SetActive(false);
                        allyAnimaNum--;


                        if (allyAnimaNum == 0)
                        {
                            state = State.defeat;
                            print("패배");
                            LoseBattle();
                            StopCoroutine(runningCoroutine);

                        }
                    }
                    else
                    {
                        turn[turnIndex++].transform.Find("Enemy Turn Portrait").GetComponent<UnityEngine.UI.Image>().color = new Color(77f / 255f, 77f / 255f, 77f / 255f);
                    }

                }

                break;

            }

        }
        runningCoroutine = null;
        if (turnList.Count == 0)
        {
            BattleStart();
        }
        else
        {
            SetState(turnList);
        }
    }
    public int selectNoDieAnima()
    {
        int randomNumber;
        do
        {
            randomNumber = UnityEngine.Random.Range(0, allyActions.Count);
        } while (dieAllyAnima.Contains(randomNumber));
        return randomNumber;
    }
    public List<AnimaActions> getAlly()
    {
        return allyActions;
    }
    public List<EnemyActions> getEnemy()
    {
        return enemyActions;
    }
    void WinBattle()
    {
        
        Instantiate(Resources.Load<GameObject>("Minwoo/Battle Win UI"), canvas.transform);
        for(int i = 0; i< allyActions.Count; i++)
        {
            GameObject animaImage = GameObject.Find("Entry Anima List").transform.Find($"Anima {i}").gameObject;
            animaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + allyActions[i].animaData.Objectfile);
            animaImage.SetActive(true);
        }
        for (int i = 0; i < dropAnima.Count; i++) 
        {
            GameObject dropAnimaImage = GameObject.Find("Drop Anima List").transform.Find($"Anima {i}").gameObject;
            dropAnimaImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Minwoo/Portrait/" + dropAnima[i].Objectfile);
            dropAnimaImage.SetActive(true);
        }
        
    }
    void LoseBattle()
    {
        Instantiate(Resources.Load<GameObject>("Minwoo/Game Over UI"), canvas.transform);
    }
}



