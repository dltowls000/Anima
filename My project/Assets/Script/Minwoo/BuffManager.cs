using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager
{
    private List<Buff> buffList = new List<Buff>();
    public event Action<Buff> OnBuffExpired;
    public void AddOrRenuwBuff(Buff buff)
    {

        if (IsExistBuff(buff)) return;

        else buffList.Add(buff);
        
    }
    public void TickAll()
    {
        for (int i = 0; i < buffList.Count; i++) 
        {
            buffList[i].Tick();
            if (buffList[i].isExpired())
            {
                OnBuffExpired?.Invoke(buffList[i]);
                buffList.RemoveAt(i);
            }
        }
    }
    public bool IsExistBuff(Buff buff)
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

    public IEnumerable<Buff> GetBuffList() => buffList;
}
