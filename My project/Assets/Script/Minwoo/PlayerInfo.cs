using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using BansheeGz.BGDatabase;
using UnityEditor.Analytics;
using UnityEngine;


public class PlayerInfo : ScriptableObject
{
    public int maxAnimaNum = 3;
    public List<AnimaDataSO> battleAnima = new List<AnimaDataSO>();
    public List<AnimaDataSO> haveAnima = new List<AnimaDataSO>();
    public AnimaDataSO animaData;
    public bool onBossStage = false;
    int tmp = 0;
    
    public void Initialize()
    {
        
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");

        int a = Random.Range(0, 7);
        int b;
        do
        {
            b = Random.Range(0, 7);
        } while (a == b);

        animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
        animaData.Initialize(animaTable[a].Get<string>("name"),5);
        animaData.location = tmp;
        GetAnima(animaData);
        BattleSetting(haveAnima[tmp]);

        animaData = ScriptableObject.CreateInstance<AnimaDataSO>();
        animaData.Initialize(animaTable[b].Get<string>("name"), 5);
        animaData.location = tmp;
        GetAnima(animaData);
        BattleSetting(haveAnima[tmp]);
        for(int i = 0; i < 2; i++)
        {
            animaTable.ForEachEntity(entity =>
            {
                if (entity.Get<string>("name") == battleAnima[i].Name && entity.Get<int>("Meeted") == 0)
                {
                    entity.Set<int>("Meeted", 1);
                    DBUpdater.Save();
                }
            });
        }
        
    }
    public void BattleSetting(AnimaDataSO animaData)
    {
        if (haveAnima.Contains(animaData) && battleAnima.Count < maxAnimaNum)
        {
            battleAnima.Add(animaData);
            haveAnima.Remove(animaData);
        }
    }
    
    public void GetAnima(AnimaDataSO animaData)
    {
        haveAnima.Add(animaData);
    }
    public void DieAnima(AnimaDataSO animaData)
    {
        if (battleAnima.Contains(animaData))
        {
            haveAnima.Add(animaData);
            battleAnima.Remove(animaData);
        }
    }
}
