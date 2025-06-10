using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class EliteEnemyBattleSetting : MonoBehaviour
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
    public void SpawnEnemy()
    {
        animator = new List<Animator>();
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        canvas = GameObject.Find("Main Battle UI");
        Debug.Log(stage);
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
        
        int numberOfObjectsToAdd = Random.Range(1, 2);
        Debug.Log(objectfileList[0]);
        for (int i = 0; i < numberOfObjectsToAdd; i++)
        {
            int randomIndex = Random.Range(0, objectfileList.Count);
            enemyobjPrefab.Add(Resources.Load<GameObject>("Minwoo/Portrait/" + objectfileList[randomIndex]));
            enemyhpPrefab.Add(Resources.Load<GameObject>("Minwoo/EnemyAnimaHP"));
            enemyInfoPrefab.Add(Resources.Load<GameObject>($"Minwoo/Enemy{i}"));
            enemyParserPrefab.Add(Resources.Load<GameObject>($"Minwoo/Battle Parser/Enemy{i}Name"));
            battleEnemyAnima.Add(objectfileList[randomIndex]);
        }


        Debug.Log(enemyobjPrefab[0]);
        if (enemyobjPrefab.Count == 1 && enemyobjPrefab != null && enemyhpPrefab != null)
        {
            enemyinstance.Add(Instantiate(enemyobjPrefab[0], new Vector3(0f, 1.2f, 0), Quaternion.identity));
            int index = enemyinstance[0].name.IndexOf("(Clone)");
            enemyinstance[0].name = enemyinstance[0].name.Substring(0, index) + 3;
            //enemyinstance[0].transform.Rotate(0, 180f, 0);
            //damagex.Add(Random.Range(4.5f * Mathf.Pow(0, 2) - 8.5f * 0 + 10.5f - 1.5f, 4.5f * Mathf.Pow(0, 2) - 8.5f * i + 10.5f - 0.5f));
            //damagey.Add(Random.Range(y - 2.5f * i + 0.25f, y - 2.5f * i + 1.25f));
            enemyinstance[0].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);//-195f 185f 
            enemyhpinstance.Add(Instantiate(enemyhpPrefab[0], new Vector2(951f, 530f), Quaternion.identity, canvas.transform));
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
