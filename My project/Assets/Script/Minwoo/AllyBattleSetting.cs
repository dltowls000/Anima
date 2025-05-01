using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEditor.Animations;
using UnityEngine;

public class AllyBattleSetting : MonoBehaviour
{
    public GameObject canvas;
    public List <GameObject> allyobjPrefab;
    public List <GameObject> allyinstance;
    public List<GameObject> allyInfoPrefab;
    public List <GameObject> allyInfoInstance;
    public string objname;
    public PlayerInfo playerinfo;
    public GameObject prefab;
    public List<Animator> animator;
    public List<GameObject> allyhpPrefab;
    public List<GameObject> allyhpinstance;
    public List<float> damagex;
    public List<float> damagey;
    BattleManager battleManager;
    //public SlotManager slotManager;
    public void initialize()
    {
        allyinstance = new List<GameObject>();
        allyobjPrefab = new List<GameObject>();
        allyhpPrefab = new List<GameObject>();
        allyhpinstance = new List<GameObject>();
        allyInfoInstance = new List<GameObject>();
        allyInfoPrefab = new List<GameObject>();
        animator = new List<Animator>();
        damagex = new List<float>();
        damagey = new List<float>();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        playerinfo = battleManager.playerInfo; 
        canvas = GameObject.Find("Main Battle UI");
    }
    public void SpawnAlly()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        for(int i = 0; i < playerinfo.battleAnima.Count ; i++)
        {
            allyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/" + playerinfo.battleAnima[i].Objectfile));
            allyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/AllyAnimaHP"));
            allyInfoPrefab.Add(Instantiate(Resources.Load<GameObject>($"Minwoo/Ally{i}") , canvas.transform));
        }
        if (allyobjPrefab != null)
        {
            for(int i =0; i< allyobjPrefab.Count; i++)
            {
                allyinstance.Add(Instantiate(allyobjPrefab[i], new Vector3(5.25f * Mathf.Pow(i, 2) - (8.75f * i), -2.2f, 0f), Quaternion.identity));
                allyinstance[i].transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                //damagex.Add(Random.Range(-4.5f * Mathf.Pow(i, 2) + 8.5f *i - 10.5f + 0.5f, -4.5f * Mathf.Pow(i, 2) + 8.5f *i - 10.5f + 1.5f));
                //damagey.Add(Random.Range(y - 2.5f *i + 0.25f, y - 2.5f *i + 1.25f));
                allyhpinstance.Add(Instantiate(allyhpPrefab[i], new Vector2(598.5f * Mathf.Pow(i, 2) - (997.5f * i) +951f , 158f), Quaternion.identity, canvas.transform));
                allyInfoInstance.Add(Instantiate(allyInfoPrefab[i]));
                int index = allyhpinstance[i].name.IndexOf("(Clone)");
                allyhpinstance[i].name = allyhpinstance[i].name.Substring(0, index) + "" + i;
                animator.Add(allyinstance[i].GetComponent<Animator>());
            }
           
        }
    }

}
