using UnityEngine;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager instance {  get; private set; }

    public Transform contentParent;
    public GameObject slotPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        var entries = Resources.LoadAll<CorridorEntry>("Minyoung/CorridorEntries");

        foreach (var entry in entries) 
        {
            GameObject slotObj = Instantiate(slotPrefab, contentParent);
            var slot = slotObj.GetComponent<CorridorSlot>();

            bool isCollected = LoadPlayerData(entry); // 추후 개인 저장에따라(DB) 변경예정
            slot.Initialize(entry, isCollected);
        }
    }
    private bool LoadPlayerData(CorridorEntry entry)
    {
        //임시 
        return true;
    }
    public void OnSlotSelected(CorridorSlot slot)
    {
        if (!slot.isCollected)
        {
            Debug.Log("수집되지않음");
            return;
        }
        CorridorUIManager.instance.Show_DetailUI(slot.currentEntry);
    }

}
