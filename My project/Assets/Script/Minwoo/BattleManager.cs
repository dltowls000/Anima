using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;
using BansheeGz.BGDatabase;
using UnityEngine.UI;
using Unity.VisualScripting;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.Rendering;
public class BattleManager : MonoBehaviour, IBattleManager
{
    [SerializeField]
    CameraManager cameraManager;
    public CameraManager CameraManager => cameraManager;
    [SerializeField]
    Transform turnUI;
    [SerializeField]
    BattleLogManager battleLogManager;
    public BattleLogManager BattleLogManager => battleLogManager;
    [SerializeField]
    GameObject battleManager;
    [SerializeField]
    GameObject canvas;
    private BuffManager buffManager;
    public BuffManager BuffManager => buffManager;
    public GameObject Canvas => canvas;
    
    PointerEventData pointerEventData;
    Coroutine runningCoroutine = null;
    public Coroutine RunningCoroutine
    {
        get => runningCoroutine; 
        set => runningCoroutine = value;
    }
    BattleState state;
    public BattleState stat
    {
        get => state;
        set => state = value;
    }
    TextAsset skillData;
    List<SkillData> skills;
    List<SkillData> matchedSkill;
    public List<SkillData> MatchedSkill => matchedSkill;
    public PlayerInfo playerInfo;
    public PlayerInfo PlayerInfo => playerInfo;
    List<HealthBar> allyHealthBar;
    public List<HealthBar> AllyHealthBar => allyHealthBar;
    List<HealthBar> enemyHealthBar;
    public List<HealthBar> EnemyHealthBar => enemyHealthBar;
    List<ParserBar> allyDamageBar;
    public List<ParserBar> AllyDamageBar => allyDamageBar;
    List<ParserBar> enemyDamageBar;
    public List<ParserBar> EnemyDamageBar => enemyDamageBar;
    List<ParserBar> allyHealBar;
    public List<ParserBar> AllyHealBar => allyHealBar;
    List<ParserBar> enemyHealBar;
    public List<ParserBar> EnemyHealBar => enemyHealBar;
    List<TextMeshProUGUI> allyDamageText;
    public List<TextMeshProUGUI> AllyDamageText => allyDamageText;
    List<TextMeshProUGUI> enemyDamageText;
    public List<TextMeshProUGUI> EnemyDamageText => enemyDamageText;
    List<TextMeshProUGUI> allyHealText;
    public List<TextMeshProUGUI> AllyHealText => allyHealText;
    List<TextMeshProUGUI> enemyHealText;
    public List<TextMeshProUGUI> EnemyHealText => enemyHealText;

    EnemyBattleSetting enemyBattleSetting;
    public IEnemyBattleSetting EnemyBattleSetting => enemyBattleSetting;
    AllyBattleSetting allyBattleSetting;
    public IAllyBattleSetting AllyBattleSetting => allyBattleSetting;
    AnimaActions tmpAnima;
    List<AnimaActions> allyActions;
    public List<AnimaActions> AllyActions => allyActions;
    List<EnemyActions> enemyActions;
    public List<EnemyActions> EnemyActions => enemyActions;

    List<GameObject> ally;
    List<GameObject> enemy;

    List<int> dieAllyAnima;
    public List<int> DieAllyAnima => dieAllyAnima;
    [SerializeField]
    DamageNumber damageNumber;
    public DamageNumber DamageNumber => damageNumber;

    EventSystem eventSystem;

    
    List<GameObject> turn;
    public List<GameObject> Turn => turn;

    List<AnimaDataSO> turnList;
    public List<AnimaDataSO> TurnList => turnList;

    List<AnimaDataSO> tmpturnList;
    public List<AnimaDataSO> TmpturnList => tmpturnList;

    List<GameObject> isTurn;
    public List<GameObject> IsTurn => isTurn;

    List<GameObject> allyInfo;
    List<GameObject> enemyInfo;
    
    List<AnimaDataSO> dropAnima;
    public List<AnimaDataSO> DropAnima => dropAnima;

    GameObject animaActionUI;
    public GameObject AnimaActionUI => animaActionUI;

    GameObject arrow;
    
    GameObject rebuild;

    TurnManager turnManager;
    Button attackButton;
    public Button AttackButton => attackButton;
    Button skillButton;
    public Button SkillButton => skillButton;
    Button skill1;
    
    Button skill2;
    
    bool isZKeyPressed = false;
    public bool IsZKeyPressed
    {
        get => isZKeyPressed;
        set => isZKeyPressed = value;
    }
    
    bool isXKeyPressed = false;
    
    BGRepo database;
    BGMetaEntity animaTable;
    public BGMetaEntity AnimaTable => animaTable;
    BGMetaEntity skillTable;
    int turnIndex = 0;
    public int TurnIndex
    {
        get => turnIndex;
        set => turnIndex = value;
    }
    int enemyAnimaNum = 0;
    public int EnemyAnimaNum
    {
        get => enemyAnimaNum;
        set => enemyAnimaNum = value;
    }
    
    int allyAnimaNum = 0;
    public int AllyAnimaNum
    {
        get => allyAnimaNum;
        set => allyAnimaNum = value;
    }
    
    int roundNum = 1;
    
    int selectEnemy = 0;
    int index = 0;
    float maxValue = 0;
    public float MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }
    

    SingleAttack singleAttack;
    MultipleAttack multipleAttack;   
    void Start()
    {
        playerInfo = GameObject.Find("Game Manager").GetComponent<AnimaInventoryManager>().playerInfo;
        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);
        singleAttack = this.AddComponent<SingleAttack>();
        singleAttack.initialize(this);
        multipleAttack = this.AddComponent<MultipleAttack>();
        multipleAttack.initialize(this);
        isTurn = new List<GameObject>();
        buffManager = new BuffManager();
        turn = new List<GameObject>();
        skillData = Resources.Load<TextAsset>("Minwoo/SkillList");
        skills = JsonConvert.DeserializeObject<List<SkillData>>(skillData.text);
        dieAllyAnima = new List<int>();
        dropAnima = new List<AnimaDataSO>();
        state = BattleState.start;
        database = BGRepo.I;
        animaTable = database.GetMeta("Anima");
        skillTable = database.GetMeta("Skill");
        AnimaActionUISetting();
        AllyBattlePrepare();
        EnemyBattlePrepare();
        BattleStart();
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
        allyHealBar = new List<ParserBar>();
        allyDamageText = new List<TextMeshProUGUI>();
        allyHealText = new List<TextMeshProUGUI>();
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
            enemyHealBar = new List<ParserBar>();
            enemyDamageText = new List<TextMeshProUGUI>();
            enemyHealText = new List<TextMeshProUGUI>();
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
            allyHealBar.Add(allyParser.transform.Find($"A{i}Heal").transform.Find($"A{i} Heal Bar").GetComponent<ParserBar>());
            allyHealthBar[i].Initialize(allyActions[i].animaData.Maxstamina, allyActions[i].animaData.Stamina);
            allyDamageBar[i].Initialize();
            allyHealBar[i].Initialize();
            allyParser.GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.Name;
            allyDamageText.Add(allyParser.transform.Find($"A{i}Damage").GetComponent<TextMeshProUGUI>());
            allyHealText.Add(allyParser.transform.Find($"A{i}Heal").GetComponent<TextMeshProUGUI>());
            GameObject.Find($"AllyAnimaHP{i}").transform.Find("LV UI").transform.Find("Current LV").GetComponent<TextMeshProUGUI>().text = allyActions[i].animaData.level.ToString();
            allyAnimaNum++;

        }
    }
    void initializeEnemyAnima()
    {
        int level = 0;
        
        if(playerInfo.haveAnima.Count > 0)
        {
            foreach(var anima in playerInfo.haveAnima)
            {
                if(anima.level >= level)
                {
                    level = anima.level;
                }

            }
        }
        if(playerInfo.battleAnima.Count > 0)
        {
            foreach (var anima in playerInfo.battleAnima)
            {
                if (anima.level >= level)
                {
                    level = anima.level;
                }
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
            enemyHealBar.Add(enemyParser.transform.Find($"E{i}Heal").transform.Find($"E{i} Heal Bar").GetComponent<ParserBar>());
            enemyHealthBar[i].Initialize(enemyActions[i].animaData.Maxstamina, enemyActions[i].animaData.Stamina);
            enemyDamageBar[i].Initialize();
            enemyHealBar[i].Initialize();
            enemyParser.GetComponent<TextMeshProUGUI>().text = enemyActions[i].animaData.Name;
            enemyDamageText.Add(enemyParser.transform.Find($"E{i}Damage").GetComponent<TextMeshProUGUI>());
            enemyHealText.Add(enemyParser.transform.Find($"E{i}Heal").GetComponent<TextMeshProUGUI>());
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
        Transform skillAction = animaActionUI.transform.Find("Skill Button Frame2").transform.Find("Skill Button2");
        skillButton = skillAction.GetComponent<UnityEngine.UI.Button>();
        skill1 = animaActionUI.transform.Find("Skill Button Frame0").transform.Find("Skill Button0").GetComponent<Button>();
        skill2 = animaActionUI.transform.Find("Skill Button Frame1").transform.Find("Skill Button1").GetComponent<Button>();
        attackButton.onClick.AddListener(battleManager.GetComponent<BattleManager>().PlayerAttackButton);
        skillButton.onClick.AddListener(battleManager.GetComponent<BattleManager>().PlayerSkillButton);
        skill1.onClick.AddListener(battleManager.GetComponent<BattleManager>().SkillButton1);
        skill2.onClick.AddListener(battleManager.GetComponent<BattleManager>().SkillButton2);
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
            skillButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Skill\n";
            for(int i = 0; i < turnList[0].skillName.Count ; i++)
            {
                animaActionUI.transform.Find($"Skill Button Frame{i}").Find($"Skill Button{i}").Find($"Skill Text{i}").GetComponent<TextMeshProUGUI>().text = turnList[0].skillName[i];
            }
            if (turnList[0].skillName.Count == 1)
            {
                animaActionUI.transform.Find("Skill Button Frame1").Find("Skill Button1").GetComponent<Button>().interactable = false;
            }
            else
            {
                skillButton.interactable = true;
            }
            state = BattleState.playerTurn;
        }
        
        else
        {
            state = BattleState.enemyTurn;
            isTurn[turnIndex].SetActive(true);
            runningCoroutine = StartCoroutine(EnemyTurn());
        }
        
    }
    void PlayerAttackButton()
    {
        
        selectEnemy = 0;

        if (state != BattleState.playerTurn)
        {
            return;
        }
        if (state == BattleState.playerTurn)
        {
            isZKeyPressed = true;
            DestroyImmediate(arrow);
            runningCoroutine = StartCoroutine(PlayerAttack());
            attackButton.interactable = false;
            skillButton.interactable = false;
        }

    }
    void PlayerSkillButton()
    {
       
        selectEnemy = 0;

        if (state != BattleState.playerTurn)
        {
            return;
        }
        if (state == BattleState.playerTurn)
        {
            isZKeyPressed = true;
            DestroyImmediate(arrow);
            for(int i = 0; i < 2; i++)
            {
                animaActionUI.transform.Find($"Skill Button Frame{i}").gameObject.SetActive(true);
            }
            GameObject.Find("Skill Button0").GetComponent<Button>().Select();
            
        }

    }
    void SkillButton1()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        matchedSkill = skills.Where(s => s.name == skill1.transform.Find("Skill Text0").GetComponent<TextMeshProUGUI>().text).ToList();

        switch (matchedSkill[0].Type)
        {
            case "SingleAttack":
                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 0));
                break;
            case "SingleHeal":
                runningCoroutine = StartCoroutine(PlayerSingleHeal( 0));
                break;
            case "SingleBuff":
                runningCoroutine = StartCoroutine(PlayerSingleBuff(0));
                break;
            case "SingleDebuff":
                runningCoroutine = StartCoroutine(PlayerSingleDebuff(0));
                break;
                //case "AreaAttack":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 0));
                //    break;
                //case "AreaHeal":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 0));
                //    break;
                //case "AreaBuff":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 0));
                //    break;
                //case "AreaDebuff":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 0));
                //    break;
        }
        
        
    }
    void SkillButton2()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        matchedSkill = skills.Where(s => s.name == skill1.transform.Find("Skill Text0").GetComponent<TextMeshProUGUI>().text).ToList();
        switch (matchedSkill[0].Type)
        {
            case "SingleAttack":
                runningCoroutine = StartCoroutine(PlayerSingleAttackSkill( 1));
                break;
            case "SingleHeal":
                runningCoroutine = StartCoroutine(PlayerSingleHeal( 1));
                break;
            case "SingleBuff":
                runningCoroutine = StartCoroutine(PlayerSingleBuff(1));
                break;
            case "SingleDebuff":
                runningCoroutine = StartCoroutine(PlayerSingleDebuff(1));
                break;
                //case "AreaAttack":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                //    break;
                //case "AreaHeal":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                //    break;
                //case "AreaBuff":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                //    break;
                //case "AreaDebuff":
                //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                //    break;
        }
    }
    IEnumerator PlayerAttack()
    {
        yield return AttackCursorInit();
        
        tmpAnima = PresentAllyTurn();
        
        yield return StartCoroutine(singleAttack.SingleAllyAttack(tmpAnima, selectEnemy));
        
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
    
    IEnumerator PlayerSingleAttackSkill( int skillNum)
    {

        yield return AttackCursorInit();
        
        tmpAnima = PresentAllyTurn();
        
        yield return StartCoroutine(singleAttack.SingleAllySkill(tmpAnima, selectEnemy, skillNum));
           
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
    IEnumerator PlayerSingleHeal(int skillNum)
    {
        yield return BuffCursorInit();

        tmpAnima = PresentAllyTurn();
        yield return StartCoroutine(singleAttack.SingleAllyHeal(tmpAnima, selectEnemy, skillNum));
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
    IEnumerator PlayerSingleBuff(int skillNum)
    {
        yield return BuffCursorInit();
        tmpAnima = PresentAllyTurn();
        yield return StartCoroutine(singleAttack.SingleAllyBuff(tmpAnima, selectEnemy, skillNum));
        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[selectEnemy].animaData);
        buffManager.AddOrRenuwBuff(buff);
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
    IEnumerator PlayerSingleDebuff(int skillNum)
    {
        yield return AttackCursorInit();
        tmpAnima = PresentAllyTurn();
        yield return StartCoroutine(singleAttack.SingleAllyDebuff(tmpAnima, selectEnemy, skillNum));
        Buff buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData);
        buffManager.AddOrRenuwBuff(buff);
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
        int selectAlly = selectNoDieAnima();
        selectEnemy = Random.Range(0, enemyActions.Count);
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
                    yield return StartCoroutine(singleAttack.SingleEnemyAttack(enemy, selectAlly));

                }
                else if (enemy.performance.Equals("Skill"))
                {
                    matchedSkill = skills.Where(s => s.name == enemy.animaData.skillName[0]).ToList();
                    Buff buff;
                    switch (matchedSkill[0].Type)
                    {
                        case "SingleAttack":
                            yield return StartCoroutine(singleAttack.SingleEnemySkill(enemy, selectAlly));
                            break;
                        case "SingleHeal":
                            yield return StartCoroutine(singleAttack.SingleEnemyHeal(enemy, selectEnemy));
                            break;
                        case "SingleBuff":
                            yield return StartCoroutine(singleAttack.SingleEnemyBuff(enemy, selectEnemy));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, enemyActions[selectEnemy].animaData);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                        case "SingleDebuff":
                            runningCoroutine = StartCoroutine(singleAttack.SingleEnemyDebuff(enemy, selectAlly));
                            buff = new Buff(matchedSkill[0].Affect, matchedSkill[0].Weight, matchedSkill[0].Turn, allyActions[selectAlly].animaData);
                            buffManager.AddOrRenuwBuff(buff);
                            break;
                            //case "AreaAttack":
                            //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                            //    break;
                            //case "AreaHeal":
                            //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                            //    break;
                            //case "AreaBuff":
                            //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                            //    break;
                            //case "AreaDebuff":
                            //    runningCoroutine = StartCoroutine(PlayerSkill(selectEnemy, 1));
                            //    break;
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
    
    public List<AnimaActions> GetAllyActions(){ return allyActions; }
    public List<EnemyActions> GetEnemyActions(){ return enemyActions; }
    
    
    public void WinBattle()
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
    public void LoseBattle()
    {
        Instantiate(Resources.Load<GameObject>("Minwoo/Game Over UI"), canvas.transform);
    }
    AnimaActions PresentAllyTurn()
    {
        foreach (AnimaActions anima in allyActions)
        {
            if (TurnList.Count == 0)
            {
                BattleStart(); //라운드 재정비?
            }
            if (ReferenceEquals(TurnList[0], anima.animaData))
            {
                return anima;
            }
        }
        return null;
    }
    IEnumerator AttackCursorInit()
    {
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        index = 0;
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
    }
    IEnumerator BuffCursorInit()
    {
        arrow = GameObject.Find("Arrow_down(Clone)");
        DestroyImmediate(arrow);
        index = 0;
        Instantiate(Resources.Load<GameObject>("Minwoo/Arrow_down"), new Vector2(allyBattleSetting.allyinstance[index].transform.position.x, allyBattleSetting.allyinstance[index].transform.position.y + 1.2f), Quaternion.identity);
        arrow = GameObject.Find("Arrow_down(Clone)");
        while (true)
        {
            if (index != 2 && index < (allyAnimaNum - 1) && Input.GetKeyUp(KeyCode.RightArrow))
            {
                index++;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(allyBattleSetting.allyinstance[index].transform.position.x, allyBattleSetting.allyinstance[index].transform.position.y + 1.2f);
            }
            if (index != 0 && Input.GetKeyUp(KeyCode.LeftArrow))
            {
                index--;
                GameObject.Find("Arrow_down(Clone)").transform.position = new Vector2(allyBattleSetting.allyinstance[index].transform.position.x, allyBattleSetting.allyinstance[index].transform.position.y + 1.2f);
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
    }
}



