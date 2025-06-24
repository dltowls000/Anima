using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AnimaInventoryDetailUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text apText;
    [SerializeField] private TMP_Text dpText;
    [SerializeField] private TMP_Text spText;

    [SerializeField] private TMP_Text skill1NameText;
    [SerializeField] private TMP_Text skill1DescriptionText;
    [SerializeField] private TMP_Text skill2NameText;
    [SerializeField] private TMP_Text skill2DescriptionText;
    
    [SerializeField] private Material grayscaleMaterial;
    [SerializeField] private Color normalHpColor = Color.white;
    [SerializeField] private Color defeatedHpColor = Color.red;

    private static Dictionary<string, Sprite> portraitCache = new Dictionary<string, Sprite>();

    public void Show(AnimaDataSO anima)
    {
        if (anima == null)
        {
            Clear();
            return;
        }

        if (gameObject != null)
        {
            gameObject.SetActive(true);
        }
    
        string imagePath = "Minwoo/Portrait/" + anima.Objectfile;
        if (portraitImage != null)
        {
            if (!portraitCache.TryGetValue(imagePath, out Sprite sprite))
            {
                sprite = Resources.Load<Sprite>(imagePath);
                if (sprite != null)
                {
                    portraitCache[imagePath] = sprite;
                }
            }
            portraitImage.sprite = sprite;
            
            bool isDefeated = anima.Animadie || anima.Stamina <= 0;
            if (isDefeated)
            {
                portraitImage.material = grayscaleMaterial;
            }
            else
            {
                portraitImage.material = null;
            }
        }
    
        if (nameText != null) nameText.text = anima.Name;
        if (levelText != null) levelText.text = anima.level.ToString();
        // if (typeText != null) typeText.text = anima.type;

        if (hpText != null) 
        {
            hpText.text = $"{anima.Stamina} / {anima.Maxstamina}";
            
            bool isDefeated = anima.Animadie || anima.Stamina <= 0;
            hpText.color = isDefeated ? defeatedHpColor : normalHpColor;
        }

        if (apText != null) apText.text = anima.Damage.ToString();
        if (dpText != null) dpText.text = anima.defense.ToString();
        if (spText != null) spText.text = anima.Speed.ToString();

        if (skill1NameText != null) skill1NameText.text = anima.skillName;
        // if (skill2NameText != null) skill2NameText.text = "두 번째 스킬";
    
        // if (skill1DescriptionText != null) skill1DescriptionText.text = "스킬1 설명";
        // if (skill2DescriptionText != null) skill2DescriptionText.text = "스킬2 설명";
    }

    public void Clear()
    {
        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
    
    public static void ClearPortraitCache()
    {
        portraitCache.Clear();
    }
}