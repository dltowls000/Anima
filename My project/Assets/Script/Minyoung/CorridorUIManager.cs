using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CorridorUIManager : MonoBehaviour
{
    public static CorridorUIManager instance;

    [Header("UI")]
    public GameObject corridorDetailUI;

    [Header("디테일 UI 요소")]
    public Image animaImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private void Awake()
    {
        instance = this;
        corridorDetailUI.SetActive(false);

    }
    private void Update()
    {
        if (corridorDetailUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            Close_DetailUI();
        }
    }
    public void Show_DetailUI(CorridorEntry currentEntry)
    {
        animaImage.sprite = currentEntry.colorImage;
        nameText.text = currentEntry.animaName;
        descriptionText.text = currentEntry.description;
        corridorDetailUI.SetActive(true);
    }
    public void Close_DetailUI()
    {
        corridorDetailUI.SetActive(false);
    }
}
