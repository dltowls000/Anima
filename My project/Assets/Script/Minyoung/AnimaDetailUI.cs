using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimaDetailUI : MonoBehaviour
{
    [Header("UI 참조")]
    public Image animaImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public void Display(AnimaEntry anima, bool discovered)
    {
        animaImage.sprite = anima.colorImage; // 실루엣이 필요하면 조건 처리 가능

        nameText.text = discovered ? anima.animaName : "???";
        descriptionText.text = discovered ? anima.description : "???";
    }
}