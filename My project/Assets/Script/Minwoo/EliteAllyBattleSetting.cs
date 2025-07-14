using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEditor.Animations;
using UnityEngine;

public class EliteAllyBattleSetting : MonoBehaviour,IAllyBattleSetting
{
    public GameObject canvas;
    public List<GameObject> allyobjPrefab;
    public List<GameObject> allyinstance;
    public List<GameObject> allyInfoPrefab;
    public List<GameObject> allyInfoInstance;
    public string objname;
    public PlayerInfo playerinfo;
    public GameObject prefab;
    public List<GameObject> allyhpPrefab;
    public List<GameObject> allyhpinstance;
    public List<float> damagex;
    public List<float> damagey;
    EliteBattleManager eliteBattleManager;
    List<GameObject> allyParserPrefab;
    List<GameObject> allyParserInstance;
    GameObject battleParser;
    public GameObject Canvas => canvas;
    public List<GameObject> AllyObjPrefab => allyobjPrefab;
    public List<GameObject> AllyInstance => allyinstance;
    public List<GameObject> AllyInfoPrefab => allyInfoPrefab;
    public List<GameObject> AllyInfoInstance => allyInfoInstance;
    public string ObjName => objname;
    public PlayerInfo PlayerInfo => playerinfo;
    public GameObject Prefab => prefab;
    public List<GameObject> AllyHpPrefab => allyhpPrefab;
    public List<GameObject> AllyHpInstance => allyhpinstance;
    public List<float> DamageX => damagex;
    public List<float> DamageY => damagey;

    public List<GameObject> AllyParserPrefab => allyParserPrefab;
    public List<GameObject> AllyParserInstance => allyParserInstance;
    public GameObject BattleParser => battleParser;
    
    public void initialize()
    {
        allyinstance = new List<GameObject>();
        allyobjPrefab = new List<GameObject>();
        allyhpPrefab = new List<GameObject>();
        allyhpinstance = new List<GameObject>();
        allyInfoInstance = new List<GameObject>();
        allyInfoPrefab = new List<GameObject>();
        allyParserPrefab = new List<GameObject>();
        allyParserInstance = new List<GameObject>();
        damagex = new List<float>();
        damagey = new List<float>();
        eliteBattleManager = GameObject.Find("BattleManager").GetComponent<EliteBattleManager>();
        battleParser = GameObject.Find("Battle Parser");
        playerinfo = eliteBattleManager.PlayerInfo;
        canvas = GameObject.Find("Main Battle UI");
    }
    public void SpawnAlly()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        for (int i = 0; i < playerinfo.battleAnima.Count; i++)
        {
            allyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/Portrait/" + playerinfo.battleAnima[i].Objectfile));
            allyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/AllyAnimaHP"));
            allyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/AllyElite{i}"));
            allyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Ally{i}Name"));

        }
        if (allyobjPrefab != null)
        {
            if (allyobjPrefab.Count == 3)
            {
                for (int i = 0; i < allyobjPrefab.Count; i++)
                {
                    allyinstance.Add(Instantiate(allyobjPrefab[i], new Vector3((i * 3.5f) - 3.5f, -2.2f, 0f), Quaternion.identity));
                    allyinstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = allyinstance[i].name.IndexOf("(Clone)");
                    allyinstance[i].name = allyinstance[i].name.Substring(0, index) + i;
                    allyinstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    allyhpinstance.Add(Instantiate(allyhpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    allyhpinstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 380f, -390f, 0f);
                    index = allyhpinstance[i].name.IndexOf("(Clone)");
                    allyhpinstance[i].name = allyhpinstance[i].name.Substring(0, index) + i;
                    allyInfoInstance.Add(Instantiate(allyInfoPrefab[i], canvas.transform));
                    index = allyInfoInstance[i].name.IndexOf("(Clone)");
                    allyInfoInstance[i].name = allyInfoInstance[i].name.Substring(0, index);
                    allyParserInstance.Add(Instantiate(allyParserPrefab[i], battleParser.transform));
                    index = allyParserInstance[i].name.IndexOf("(Clone)");
                    allyParserInstance[i].name = allyParserInstance[i].name.Substring(0, index);
                }
            }
            else if (allyobjPrefab.Count == 2)
            {
                for (int i = 0; i < allyobjPrefab.Count; i++)
                {
                    allyinstance.Add(Instantiate(allyobjPrefab[i], new Vector3((i * 3.5f) - 1.75f, -2.2f, 0f), Quaternion.identity));
                    allyinstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                    int index = allyinstance[i].name.IndexOf("(Clone)");
                    allyinstance[i].name = allyinstance[i].name.Substring(0, index) + i;
                    allyinstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    allyhpinstance.Add(Instantiate(allyhpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                    allyhpinstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 200f, -390f, 0f);
                    index = allyhpinstance[i].name.IndexOf("(Clone)");
                    allyhpinstance[i].name = allyhpinstance[i].name.Substring(0, index) + i;
                    allyInfoInstance.Add(Instantiate(allyInfoPrefab[i], canvas.transform));
                    index = allyInfoInstance[i].name.IndexOf("(Clone)");
                    allyInfoInstance[i].name = allyInfoInstance[i].name.Substring(0, index);
                    allyParserInstance.Add(Instantiate(allyParserPrefab[i], battleParser.transform));
                    index = allyParserInstance[i].name.IndexOf("(Clone)");
                    allyParserInstance[i].name = allyParserInstance[i].name.Substring(0, index);
                }
            }
            else
            {
                allyinstance.Add(Instantiate(allyobjPrefab[0], new Vector3(0f, -2.2f, 0), Quaternion.identity));
                allyinstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = allyinstance[0].name.IndexOf("(Clone)");
                allyinstance[0].name = allyinstance[0].name.Substring(0, index) + 0;
                allyinstance[0].transform.Rotate(0, 180f, 0);
                allyinstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                allyhpinstance.Add(Instantiate(allyhpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
                allyhpinstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -390f, 0f);
                index = allyhpinstance[0].name.IndexOf("(Clone)");
                allyhpinstance[0].name = allyhpinstance[0].name.Substring(0, index) + 0;
                allyInfoInstance.Add(Instantiate(allyInfoPrefab[0], canvas.transform));
                index = allyInfoInstance[0].name.IndexOf("(Clone)");
                allyInfoInstance[0].name = allyInfoInstance[0].name.Substring(0, index);
                allyParserInstance.Add(Instantiate(allyParserPrefab[0], battleParser.transform));
                index = allyParserInstance[0].name.IndexOf("(Clone)");
                allyParserInstance[0].name = allyParserInstance[0].name.Substring(0, index);
            }
        }

    }

}
