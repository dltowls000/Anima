using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class EnemyBattleSetting : MonoBehaviour
{
    private float y = 0.75f;
    private float hpy = -99f;
    public List<float> damagex;
    public List<float> damagey;
    public List<GameObject> enemyobjPrefab;
    public List<GameObject> enemyinstance;
    public string objname;
    private List<string> objectfileList;
    public List<string> battleEnemyAnima;
    public List<Animator> animator;
    public List<GameObject> enemyhpPrefab;
    public List<GameObject> enemyhpinstance;
    public GameObject canvas;

    public void SpawnEnemy()
    {
        animator = new List<Animator>();
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("BattleCanvas");
        if (objectfileList != null)
        {
            objectfileList.Clear();
            enemyobjPrefab.Clear();
            enemyinstance.Clear();
            enemyhpPrefab.Clear();
            enemyhpinstance.Clear();
            damagex.Clear();
            damagey.Clear();
            battleEnemyAnima.Clear();
            enemyobjPrefab.Clear();
        }
        else
        {
            objectfileList = new List<string>();
            enemyobjPrefab = new List<GameObject>();
            enemyinstance = new List<GameObject>();
            enemyhpPrefab = new List<GameObject>();
            enemyhpinstance = new List<GameObject>();
            damagex = new List<float>();
            damagey = new List<float>();
            battleEnemyAnima = new List<string>();
            enemyobjPrefab = new List<GameObject>();
        }
        
        animaTable.ForEachEntity(entity =>
        {
            objectfileList.Add(entity.Get<string>("Objectfile"));
        });
        int numberOfObjectsToAdd = Random.Range(1, 2); 

        for (int i = 0; i < numberOfObjectsToAdd; i++)
        {
            int randomIndex = Random.Range(0, 2);
            enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/" + objectfileList[randomIndex]));
            enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
        }


        if (enemyobjPrefab != null && enemyhpPrefab != null)
        {
            for (int i = 0; i < enemyobjPrefab.Count; i++)
            {
                enemyinstance.Add(Instantiate(enemyobjPrefab[i], new Vector3(5.7f,0.75f, 0), Quaternion.identity));
                enemyinstance[i].transform.Rotate(0, 180f, 0);
                damagex.Add(Random.Range(4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f-1.5f, 4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f-0.5f));
                damagey.Add(Random.Range(y - 2.5f * i + 0.25f, y - 2.5f * i + 1.25f));
                enemyinstance[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                //enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], new Vector2(-4.3f, 0.74f), Quaternion.identity, canvas.transform));
                enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], new Vector2(960f+270f * Mathf.Pow(i, 2) - 510f * i + 639f, hpy +540f - 150f * i),Quaternion.identity, canvas.transform));
                int index = enemyhpinstance[i].name.IndexOf("(Clone)");
                enemyhpinstance[i].name = enemyhpinstance[i].name.Substring(0, index) + "" + i;
                animator.Add(enemyinstance[i].GetComponent<Animator>()); 
            }

        }
    }
    public void BossSpawn()
    {
        animator = new List<Animator>();
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
        objectfileList = new List<string>();
        enemyobjPrefab = new List<GameObject>();
        enemyinstance = new List<GameObject>();
        enemyhpPrefab = new List<GameObject>();
        enemyhpinstance = new List<GameObject>();
        damagex = new List<float>();
        damagey = new List<float>();
        battleEnemyAnima = new List<string>();
        animaTable.ForEachEntity(entity =>
        {
            objectfileList.Add(entity.Get<string>("Objectfile"));
        });

        enemyobjPrefab = new List<GameObject>();

            int randomIndex = Random.Range(1, 2);
            enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/" + objectfileList[randomIndex]));
            enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
       


        if (enemyobjPrefab != null && enemyhpPrefab != null)
        {
            for (int i = 0; i < enemyobjPrefab.Count; i++)
            {
                enemyinstance.Add(Instantiate(enemyobjPrefab[i], new Vector3(0f, 0.75f, 0f), Quaternion.identity));
                enemyinstance[i].transform.Rotate(0, 180f, 0);
                damagex.Add(Random.Range(4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f - 1.5f, 4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f - 0.5f));
                damagey.Add(Random.Range(y - 2.5f * i + 0.25f, y - 2.5f * i + 1.25f));
                enemyinstance[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], new Vector2(960f + 270f * Mathf.Pow(i, 2) - 510f * i + 639f, hpy + 900f - 150f * i), Quaternion.identity, canvas.transform));
                int index = enemyhpinstance[i].name.IndexOf("(Clone)");
                enemyhpinstance[i].name = enemyhpinstance[i].name.Substring(0, index) + "" + i;
                animator.Add(enemyinstance[i].GetComponent<Animator>());
                animator[i].runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Boss_Controller");
            }

        }
    }
}
