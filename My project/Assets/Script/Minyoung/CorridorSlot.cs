using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorridorSlot : MonoBehaviour
{
    public Image animaImage;
    public TextMeshProUGUI nameText;
    public Material grayscaleMaterial; //흑백처리용도
    
    private CorridorEntry currentEntry;
    private bool isCollected;

    public void Initialize(CorridorEntry entry, bool collected)
    {
        currentEntry = entry;
        isCollected = collected;
        animaImage.sprite = entry.colorImage;

        if (collected)
        {
            animaImage.material = null;
            nameText.text = entry.animaName;
        }
        else
        {
            if (grayscaleMaterial != null)
               animaImage.material = grayscaleMaterial;

            nameText.text = "???";

        }
    }
    public void OnSlotClicked()
    {
        if (isCollected)
        {
            CorridorDetailUI.Instance.ShowEntry(currentEntry);
        }
    }
}
