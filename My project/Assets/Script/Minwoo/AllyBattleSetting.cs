using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEditor.Animations;
using UnityEngine;

public class AllyBattleSetting : MonoBehaviour
{
    private float y = 0.75f;
    private float hpy = -99f;
    public GameObject canvas;
    public List <GameObject> allyobjPrefab;
    public List <GameObject> allyinstance;
    public string objname;
    public PlayerInfo playerinfo;
    public GameObject prefab;
    public List<Animator> animator;
    public List<GameObject> allyhpPrefab;
    public List<GameObject> allyhpinstance;
    public List<float> damagex;
    public List<float> damagey;
    public GameObject trash;
    //public SlotManager slotManager;
    public void initialize()
    {
        allyinstance = new List<GameObject>();
        allyobjPrefab = new List<GameObject>();
        allyhpPrefab = new List<GameObject>();
        allyhpinstance = new List<GameObject>();
        animator = new List<Animator>();
        damagex = new List<float>();
        damagey = new List<float>();
        //canvas = GameObject.Find("Main Battle UI");
        //trash = GameObject.Find("Main Stage UI");
        //slotManager = trash.GetComponent<SlotManager>();
        //playerinfo = slotManager.player;
        //trash.SetActive(false);
    }
    public void SpawnAlly()
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        for(int i = 0; i < playerinfo.battleAnima.Count ; i++)
        {
            allyobjPrefab.Add(Resources.Load<GameObject>(playerinfo.battleAnima[i].Objectfile));
            allyhpPrefab.Add(Resources.Load<GameObject>("HP_Bar"));
        }
        
        if (allyobjPrefab != null)
        {
            for(int i =0; i< allyobjPrefab.Count; i++)
            {
                allyinstance.Add(Instantiate(allyobjPrefab[i], new Vector3(-4.5f * Mathf.Pow(i,2)+8.5f* i - 10.5f, y - 2.5f*i, 0), Quaternion.identity));
                //allyinstance[i].transform.localScale = new Vector3(scale,scale,scale);
                damagex.Add(Random.Range(-4.5f * Mathf.Pow(i, 2) + 8.5f *i - 10.5f + 0.5f, -4.5f * Mathf.Pow(i, 2) + 8.5f *i - 10.5f + 1.5f));
                damagey.Add(Random.Range(y - 2.5f *i + 0.25f, y - 2.5f *i + 1.25f));
                allyhpinstance.Add(Instantiate(allyhpPrefab[i], new Vector2(-4.27f, -3.24f), Quaternion.identity, canvas.transform));
                //allyhpinstance.Add(Instantiate(allyhpPrefab[i], new Vector2(-270f*Mathf.Pow(i, 2)+510f*i -639f+960f, hpy+540f-150f*i), Quaternion.identity, canvas.transform));
                int index = allyhpinstance[i].name.IndexOf("(Clone)");
                allyhpinstance[i].name = allyhpinstance[i].name.Substring(0, index) + "" + i;
                animator.Add(allyinstance[i].GetComponent<Animator>());
            }
           
        }
    }

}
