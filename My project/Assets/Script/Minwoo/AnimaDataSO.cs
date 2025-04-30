using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGDatabase;
using UnityEngine;

public class AnimaDataSO : ScriptableObject
{
    public bool haveTurn = true;
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
    public string Image;
    public int location = -1;
    public float attackweight = 3;
    public float skillweight = 4;
    public float defense;
    public float EXP = 0;
    public float MAX_EXP = 0;
    public void Initialize(string name)
    {

        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        //var skillTable = database.GetMeta("Skill");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                
                Stamina = entity.Get<float>("Stamina");
                Maxstamina = Stamina;
                Damage = entity.Get<float>("Damage");
                DropGold = entity.Get<int>("DropGold");
                Speed = entity.Get<float>("Speed");
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                Image = entity.Get<string>("Image");
            }
        });
        //skillTable.ForEachEntity(entity =>
        //{
        //    if (entity.Get<string>("name") == Skill)
        //    {
        //        Skill_pp = entity.Get<int>("skill_pp");
        //        Max_pp = Skill_pp;
        //    }
        //});
    }
    public void GetAnima(string name)
    {
        Name = name;
        var database = BGRepo.I;
        var animaTable = database.GetMeta("Anima");
        //var skillTable = database.GetMeta("Skill");
        animaTable.ForEachEntity(entity => {
            if (entity.Get<string>("name") == name)
            {
                Stamina = entity.Get<float>("Stamina") * 0.4f;
                Maxstamina = Stamina;
                Damage = entity.Get<float>("Damage");
                DropGold = entity.Get<int>("DropGold");
                Speed = entity.Get<float>("Speed");
                DropRate = entity.Get<float>("DropRate");
                Objectfile = entity.Get<string>("Objectfile");
                Image = entity.Get<string>("Image");
            }
        });
    }
}
