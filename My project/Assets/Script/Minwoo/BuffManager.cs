using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffManager
{
    private List<Buff> buffList = new List<Buff>();
    private List<string> expiredBuff = new List<string>();
    public event Action<Buff> OnBuffExpired;
    public void AddOrRenuwBuff(Buff buff)
    {

        if (IsExistAndRenewBuff(buff)) return;

        else buffList.Add(buff);
        
    }
    public List<string> TickOne(AnimaDataSO target)
    {
        expiredBuff.Clear();
        for(int i = 0; i < buffList.Count; i++)
        {
            if(ReferenceEquals(buffList[i].target, target))
            {
                buffList[i].Tick();
                if (buffList[i].isExpired())
                {
                    for(int j = 0; j < buffList[i].type.Count; j++)
                    {
                        expiredBuff.Add(buffList[i].type[j]);
                    }
                    buffList.RemoveAt(i--);
                }
            }   
        }
        return expiredBuff;
    }
    public void TickAll()
    {
        for (int i = 0; i < buffList.Count; i++) 
        {
            buffList[i].Tick();
            if (buffList[i].isExpired())
            {
                OnBuffExpired?.Invoke(buffList[i]);
                buffList.RemoveAt(i--);
            }
        }
    }
    public bool IsExistAndRenewBuff(Buff buff)
    {
        foreach (var existbuff in buffList)
        {
            if(existbuff.type == buff.type && ReferenceEquals(existbuff.target, buff.target))
            {
                existbuff.Renew(buff);
                return true;
            }
        }
        return false;
    }

    public List<Buff> GetBuffList() => buffList;
}
