using UnityEngine;

public class CorridorManager : MonoBehaviour
{
    public Transform contentParent;
    public GameObject slotPrefab;

    void Start()
    {
        CorridorEntry[] entries = Resources.LoadAll<CorridorEntry>("Minyoung/CorridorEntries");

        foreach (CorridorEntry entry in entries) 
        {
            GameObject slotObj = Instantiate(slotPrefab, contentParent);
            CorridorSlot slot = slotObj.GetComponent<CorridorSlot>();

            bool isCollected = true; // 나중에 SaveSystem.Load()로 대체
            slot.Initialize(entry, isCollected);
        }
    }
}
