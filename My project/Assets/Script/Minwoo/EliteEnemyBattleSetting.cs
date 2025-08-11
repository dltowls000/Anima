using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class EliteEnemyBattleSetting : MonoBehaviour, IEnemyBattleSetting 
{
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
    public List<Animator> AnimatorList => animator;
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
        animator = new List<Animator>();
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
            if (entity.Get<string>("Type") == stage)
            {
                objectfileList.Add(entity.Get<string>("Objectfile"));
                
            }
            
        });
        int mood = 0;
        if (level <= 8)
        {
            mood = 3;
        }
        else if (level <= 12)
        {
            mood = 4;
        }
        else if (level <= 16)
        {
            mood = 5;
        }
        else if (level <= 20)
        {
            mood = 6;
        }
        else
        {
            mood = 7;
        }
        int numberOfObjectsToAdd = Random.Range(1, 2);
        for (int i = 0; i < numberOfObjectsToAdd; i++)
        {
            int randomIndex = Random.Range(mood, mood+1);
            enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/Portrait/" + objectfileList[randomIndex]));
            enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
        }


        if (enemyobjPrefab.Count == 1 && enemyobjPrefab != null && enemyhpPrefab != null)
        {
            enemyinstance.Add(Instantiate(enemyobjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
            enemyinstance[0].GetComponent<SpriteRenderer>().sortingOrder = -1;
            int index = enemyinstance[0].name.IndexOf("(Clone)");
            enemyinstance[0].name = enemyinstance[0].name.Substring(0, index) + 3;
            //enemyinstance[0].transform.Rotate(0, 180f, 0);
            //damagex.Add(Random.Range(4.5f * Mathf.Pow(0, 2) - 8.5f * 0 + 10.5f - 1.5f, 4.5f * Mathf.Pow(0, 2) - 8.5f * i + 10.5f - 0.5f));
            //damagey.Add(Random.Range(y - 2.5f * i + 0.25f, y - 2.5f * i + 1.25f));
            enemyinstance[0].transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);//-195f 185f 
            enemyhpinstance.Add(Instantiate(enemyhpPrefab[0], Vector3.zero, Quaternion.identity, canvas.transform));
            enemyhpinstance[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -22f, 0f);
            enemyInfoInstance.Add(Instantiate(enemyInfoPrefab[0], canvas.transform));
            index = enemyhpinstance[0].name.IndexOf("(Clone)");
            enemyhpinstance[0].name = enemyhpinstance[0].name.Substring(0, index) + 0;
            index = enemyInfoInstance[0].name.IndexOf("(Clone)");
            enemyInfoInstance[0].name = enemyInfoInstance[0].name.Substring(0, index);
            enemyParserInstance.Add(Instantiate(enemyParserPrefab[0], battleParser.transform));
            index = enemyParserInstance[0].name.IndexOf("(Clone)");
            enemyParserInstance[0].name = enemyParserInstance[0].name.Substring(0, index);
            animator.Add(enemyinstance[0].GetComponent<Animator>());


        }


    }
    
}
