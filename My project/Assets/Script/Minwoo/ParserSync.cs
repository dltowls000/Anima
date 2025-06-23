//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;

//public class ParserSync : MonoBehaviour
//{
//    BattleManager battleManager;
//    List<AnimaActions> battleAlly;
//    List<EnemyActions> battleEnemy;
//    public GameObject damageParser;
//    public GameObject healParser;
//    List<Transform> damageText;
//    List<Transform> healText;
//    List<ParserBar> damageBar;
//    List<ParserBar> healBar;
//    string objname;
//    public int idx = -1;
//    public int dieanima = 0;
//    void Awake()
//    {
//        objname = this.name;
//        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
//        idx = int.Parse(objname.Substring(objname.IndexOf("y") + 1, 1) + "");
//        var status = GameObject.Find(objname);
//        damageText = new List<Transform>();
//        healText = new List<Transform>();
//        healBar = new List<ParserBar>();
//        damageBar = new List<ParserBar>();

        
//    }
//    private void Start()
//    {
//        foreach( TextMeshProUGUI t in this.GetComponentsInChildren<TextMeshProUGUI>())
//    }

//    public void SetMaxPoint(float maxPoint)
//    {
//        damageBar.maxPoint = maxPoint;
//        damageBar.Initialize();
//    }
//    // Update is called once per frame
//    void OnEnable()
//    {
//        if (objname.StartsWith("A"))
//        {
//            battleAlly = battleManager.getAlly();
//            this.transform.parent.GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.Name;
//            damageText = this.transform.Find($"A{idx}Damage");
//            damageBar = damageText.Find($"A{idx} Damage Bar").GetComponent<ParserBar>();
//            healText = GameObject.Find($"A{idx}Heal").transform;
//            healBar = healText.Find($"A{idx} Heal Bar").GetComponent<ParserBar>();
//        }
//        else
//        {
//            battleEnemy = battleManager.getEnemy();
//            this.transform.parent.GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.Name;
//            damageText = this.transform.Find($"E{idx}Damage");
//            damageBar = damageText.Find($"E{idx} Damage Bar").GetComponent<ParserBar>();
//            healText = GameObject.Find($"E{idx}Heal").transform;
//            healBar = healText.Find($"E{idx} Heal Bar").GetComponent<ParserBar>();
//        }
//        if (damageParser.activeSelf)
//        {
//            damageBar.SetMaxPoint()
//            damageBar.Initialize();
//        }
//        else if (healParser.activeSelf)
//        {
//            healBar.Initialize();
//        }
        
//    }
//}
