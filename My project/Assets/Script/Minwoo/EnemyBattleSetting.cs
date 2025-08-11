using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class EnemyBattleSetting : MonoBehaviour, IEnemyBattleSetting
{
    public List<float> damagex;
    public List<float> damagey;
    public List<GameObject> enemyobjPrefab;
    public List<GameObject> enemyinstance;
    public string objname;
    private List<string> objectfileList;
    public List<string> battleEnemyAnima;
    public List<GameObject> enemyhpPrefab;
    public List<GameObject> enemyhpinstance;
    public List<GameObject> enemyInfoPrefab;
    public List<GameObject> enemyInfoInstance;
    List<GameObject> enemyParserPrefab;
    List<GameObject> enemyParserInstance;
    public GameObject canvas;
    GameObject battleParser;
    public string stage;
    public List<float> DamageX => damagex;
    public List<float> DamageY => damagey;
    public List<GameObject> EnemyObjPrefab => enemyobjPrefab;
    public List<GameObject> EnemyInstance => enemyinstance;
    public string ObjName => objname;
    public List<string> ObjectFileList => objectfileList;
    public List<string> BattleEnemyAnima => battleEnemyAnima;
    public List<GameObject> EnemyHpPrefab => enemyhpPrefab;
    public List<GameObject> EnemyHpInstance => enemyhpinstance;
    public List<GameObject> EnemyInfoPrefab => enemyInfoPrefab;
    public List<GameObject> EnemyInfoInstance => enemyInfoInstance;
    public List<GameObject> EnemyParserPrefab => enemyParserPrefab;
    public List<GameObject> EnemyParserInstance => enemyParserInstance;
    public GameObject Canvas => canvas;
    public GameObject BattleParser => battleParser;
    public string Stage => stage;
    public void SpawnEnemy(int level)
    {
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
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
            enemyInfoInstance = new List<GameObject>();
            enemyInfoPrefab = new List<GameObject>();
            enemyParserPrefab = new List<GameObject>();
            enemyParserInstance = new List<GameObject>();
            damagex = new List<float>();
            damagey = new List<float>();
            battleEnemyAnima = new List<string>();
            enemyobjPrefab = new List<GameObject>();
            battleParser = GameObject.Find("Battle Parser");
        }
        
        animaTable.ForEachEntity(entity =>
        {
            if(entity.Get<string>("Type") == stage)
                objectfileList.Add(entity.Get<string>("Objectfile"));
        });
        int numberOfObjectsToAdd = Random.Range(1,4);
        int mood = 0;
        if (level <= 8)
        {
            mood = 2;
        }
        else if (level <= 12)
        {
            mood = 3;
        }
        else if (level <= 16)
        {
            mood = 4;
        }
        else if (level <= 20)
        {
            mood = 5;
        }
        else
        {
            mood = 6;
        }
        for (int i = 0; i < numberOfObjectsToAdd; i++)
        {
            int randomIndex = Random.Range(mood-1 , mood + 2);
            enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/Portrait/" + objectfileList[randomIndex]));
            enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
        }


        if (enemyobjPrefab.Count == 3 && enemyobjPrefab != null && enemyhpPrefab != null)
        {
            for (int i = 0; i < enemyobjPrefab.Count; i++)
            {
                enemyinstance.Add(Instantiate(enemyobjPrefab[i], new Vector3((i * 3.5f) - 3.5f, 1.2f, 0), Quaternion.identity));
                enemyinstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = enemyinstance[i].name.IndexOf("(Clone)");
                enemyinstance[i].name = enemyinstance[i].name.Substring(0, index) + (i+3);
                enemyinstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                enemyhpinstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 380f, -22f, 0f);
                enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[i],canvas.transform));
                index = enemyhpinstance[i].name.IndexOf("(Clone)");
                enemyhpinstance[i].name = enemyhpinstance[i].name.Substring(0, index) + i;
                index = enemyInfoInstance[i].name.IndexOf("(Clone)");
                enemyInfoInstance[i].name = enemyInfoInstance[i].name.Substring(0, index);
                enemyParserInstance.Add(Instantiate(enemyParserPrefab[i], battleParser.transform));
                index = enemyParserInstance[i].name.IndexOf("(Clone)");
                enemyParserInstance[i].name = enemyParserInstance[i].name.Substring(0, index);
            }

        }
        if (enemyobjPrefab.Count == 2 && enemyobjPrefab != null && enemyhpPrefab != null)
        {
            for (int i = 0; i < enemyobjPrefab.Count; i++)
            {
                enemyinstance.Add(Instantiate(enemyobjPrefab[i], new Vector3( ( i * 3.5f ) - 1.75f, 1.2f, 0), Quaternion.identity));
                enemyinstance[i].GetComponent<SpriteRenderer>().sortingOrder = -1;
                int index = enemyinstance[i].name.IndexOf("(Clone)");
                enemyinstance[i].name = enemyinstance[i].name.Substring(0, index) + (i + 3);
                enemyinstance[i].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], Vector3.zero, Quaternion.identity, canvas.transform));
                enemyhpinstance[i].GetComponent<RectTransform>().anchoredPosition = new Vector3((i * 380f) - 200f, -22f, 0f);
                enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[i],canvas.transform));
                index = enemyhpinstance[i].name.IndexOf("(Clone)");
                enemyhpinstance[i].name = enemyhpinstance[i].name.Substring(0, index) + i;
                index = enemyInfoInstance[i].name.IndexOf("(Clone)");
                enemyInfoInstance[i].name = enemyInfoInstance[i].name.Substring(0, index);
                enemyParserInstance.Add(Instantiate(enemyParserPrefab[i], battleParser.transform));
                index = enemyParserInstance[i].name.IndexOf("(Clone)");
                enemyParserInstance[i].name = enemyParserInstance[i].name.Substring(0, index);
            }

        }
        else if (enemyobjPrefab.Count == 1 &&enemyobjPrefab != null && enemyhpPrefab != null)
        {
            enemyinstance.Add(Instantiate(enemyobjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
            enemyinstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
            int index = enemyinstance[0].name.IndexOf("(Clone)");
            enemyinstance[0].name = enemyinstance[0].name.Substring(0, index) + 3;
            enemyinstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);//-195f 185f 
            enemyhpinstance.Add(Instantiate(enemyhpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
            enemyhpinstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
            enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0],canvas.transform));
            index = enemyhpinstance[0].name.IndexOf("(Clone)");
            enemyhpinstance[0].name = enemyhpinstance[0].name.Substring(0, index) + 0;
            index = enemyInfoInstance[0].name.IndexOf("(Clone)");
            enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
            enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
            index = enemyParserInstance[0].name.IndexOf("(Clone)");
            enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
            

        }
        

    }
    //public void BossSpawn()
    //{
    //    animator = new List<Animator>();
    //    var database = BGRepo.I;
    //    var animaTable = database.GetMeta("Anima");
    //    canvas = GameObject.Find("Main Battle UI");
    //    objectfileList = new List<string>();
    //    enemyobjPrefab = new List<GameObject>();
    //    enemyinstance = new List<GameObject>();
    //    enemyhpPrefab = new List<GameObject>();
    //    enemyhpinstance = new List<GameObject>();
    //    damagex = new List<float>();
    //    damagey = new List<float>();
    //    battleEnemyAnima = new List<string>();
    //    animaTable.ForEachEntity(entity =>
    //    {
    //        objectfileList.Add(entity.Get<string>("Objectfile"));
    //    });

    //    enemyobjPrefab = new List<GameObject>();

    //        int randomIndex = Random.Range(1, 2);
    //        enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/" + objectfileList[randomIndex]));
    //        enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
    //        battleEnemyAnima.Add(objectfileList[randomIndex]);
       


    //    if (enemyobjPrefab != null && enemyhpPrefab != null)
    //    {
    //        for (int i = 0; i < enemyobjPrefab.Count; i++)
    //        {
    //            enemyinstance.Add(Instantiate(enemyobjPrefab[i], new Vector3(0f, 0.75f, 0f), Quaternion.identity));
    //            enemyinstance[i].transform.Rotate(0, 180f, 0);
    //            damagex.Add(Random.Range(4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f - 1.5f, 4.5f * Mathf.Pow(i, 2) - 8.5f * i + 10.5f - 0.5f));
    //            damagey.Add(Random.Range(y - 2.5f * i + 0.25f, y - 2.5f * i + 1.25f));
    //            enemyinstance[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    //            enemyhpinstance.Add(Instantiate(enemyhpPrefab[i], new Vector2(960f + 270f * Mathf.Pow(i, 2) - 510f * i + 639f, hpy + 900f - 150f * i), Quaternion.identity, canvas.transform));
    //            int index = enemyhpinstance[i].name.IndexOf("(Clone)");
    //            enemyhpinstance[i].name = enemyhpinstance[i].name.Substring(0, index) + "" + i;
    //            animator.Add(enemyinstance[i].GetComponent<Animator>());
    //            animator[i].runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Boss_Controller");
    //        }

    //    }
    //}
}
