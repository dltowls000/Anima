using UnityEngine;

public class CorridorUIManager : MonoBehaviour
{
    [Header("UI List")]
    public GameObject[] corridorPanels;  

    public void SwitchPanel(int index)
    {
        for (int i = 0; i < corridorPanels.Length; i++)
            corridorPanels[i].SetActive(i == index);
    }

}