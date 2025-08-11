using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : ScriptableObject
{
    List <AnimaDataSO> turnList;
   
    public void ResetTurnList()
    {
        turnList = new List <AnimaDataSO>();
    }
    public void InsertAnimaData(AnimaDataSO animaData)
    {
        turnList.Add(animaData);
    }
    public List<AnimaDataSO> UpdateTurnList()
    {
        turnList.Sort((a,b) => b.Speed.CompareTo(a.Speed));
        return turnList;
    }
    
}
