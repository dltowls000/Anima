using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimaDetailUI : MonoBehaviour
{
    [Header("UI ÂüÁ¶")]
    public Image animaImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text typeText;

    public void Display(AnimaEntry anima, bool discovered)
    {
        animaImage.sprite = anima.colorImage;
        animaImage.color = discovered ? Color.white : Color.black;

        nameText.text = discovered ? anima.animaName : "???";
        descriptionText.text = discovered ? anima.description : "???";
        typeText.text = discovered ? anima.emotion.ToString() : "???";
    }
}