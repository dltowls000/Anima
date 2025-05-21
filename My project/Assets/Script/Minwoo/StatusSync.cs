using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatusSync : MonoBehaviour
{
    BattleManager battleManager;
    List<AnimaActions> battleAlly;
    List<EnemyActions> battleEnemy;
    string objname;
    int idx = -1;

    void Awake()
    {
        objname = this.transform.parent.name;
        idx = int.Parse(objname.Substring(objname.Length - 1, 1) + "");
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        var status = GameObject.Find(objname);
    }

    private void Update()
    {
        this.enabled = false;
        this.enabled = true;
    }
    // Update is called once per frame
    void OnEnable()
    {
        if (objname.StartsWith("A"))
        {
            battleAlly = battleManager.getAlly();
            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.Name;
            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleAlly[idx].animaData.level.ToString();
            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.EXP.ToString();
            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = battleAlly[idx].animaData.Stamina + " / " + battleAlly[idx].animaData.Maxstamina.ToString();
        }
        else
        {
            battleEnemy = battleManager.getEnemy();
            this.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.Name;
            this.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Lv. " + battleEnemy[idx].animaData.level.ToString();
            this.transform.Find("Exp").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.EXP.ToString();
            this.transform.Find("Hp").GetComponent<TextMeshProUGUI>().text = battleEnemy[idx].animaData.Stamina + " / " + battleEnemy[idx].animaData.Maxstamina.ToString();
        }
    }
}
