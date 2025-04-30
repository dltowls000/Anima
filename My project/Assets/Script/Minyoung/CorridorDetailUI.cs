using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorridorDetailUI : MonoBehaviour
{
    public static CorridorDetailUI Instance;

    public GameObject panel;
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowEntry(CorridorEntry entry)
    {
        panel.SetActive(true);
        image.sprite = entry.colorImage;
        nameText.text = entry.name;
        descriptionText.text = entry.description;
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
