using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using JetBrains.Annotations;
using UnityEngine;

public class AnimaDataSO : ScriptableObject
{
    public bool Animadie = false;
    public bool isAlly = false;
    public int level = 1;
    public string Name; 
    public float Maxstamina = 1;
    public float Stamina = 1;
    public float Damage = 1;
    public int DropGold = 1;
    public float Speed = 1;
    public float DropRate = 1;
    public int MaxSkill_pp = 10;
    public int Skill_pp = 10;
    public int Max_pp = 1;
    public string Objectfile;
    public int location = -1;
    public float defense = 0;
    public float EXP = 0;
    public float MAX_EXP = 0;
    public float weight;
    public int enemyIndex= -1;
    
    public string skillName = "";
    public string attackName = "";
    public void Initialize(string name, int level)
    {
        
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                this.level = level;
                weight = entity.Get<float>("Weight");
                Maxstamina = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("HP")));
                Stamina = Maxstamina;
                Damage = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("AP")));
                defense = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("DP")));
                DropGold = entity.Get<int>("DropGold");
                Speed = Mathf.Ceil(entity.Get<float>("SP"));
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                attackName = entity.Get<string>("Attack");
                skillName = entity.Get<string>("Skill");
            }
        });
    }
    public void GetAnima(string name, int level)
    {
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                this.level = level;
                weight = entity.Get<float>("Weight");
                Maxstamina = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("HP")));
                Stamina = Maxstamina * 0.4f;
                Damage = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("AP")));
                defense = Mathf.Ceil(CalcStat(level, weight, entity.Get<float>("DP")));
                DropGold = entity.Get<int>("DropGold");
                Speed = Mathf.Ceil(entity.Get<float>("SP"));
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                attackName = entity.Get<string>("Attack");
                skillName = entity.Get<string>("Skill");
            }
        });
    }
    public float CalcStat(int level, float weight, float stat)
    {
        //math.ceil(((2*j)*(j+0.9))*(k * math.sqrt(math.sqrt(pow(i,3))) + k*math.sqrt(math.sqrt(pow(j, i)))))
        float a = ((2f * weight) * (weight + 0.9f));
        if (float.IsNaN(a)) { Debug.Log("a"); }
        float b = (stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(level, 3f))));
        if (float.IsNaN(b)) { Debug.Log("b"); }
        float c = stat * Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(weight, level)));
        if (float.IsNaN(a)) { Debug.Log("c"); }
        return Mathf.Ceil(a * b + c);
    }
}
