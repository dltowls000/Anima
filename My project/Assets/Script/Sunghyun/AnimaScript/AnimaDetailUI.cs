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
        
        /*
        string imagePath = "Portraits/" + anima.Image;
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
        }
        
        if (nameText != null) nameText.text = anima.Name;
        if (levelText != null) levelText.text = $"Lv.{anima.Level}";
        if (typeText != null) typeText.text = anima.Type.ToString();

        if (hpText != null) hpText.text = anima.MaxHP.ToString();
        if (apText != null) apText.text = anima.AP.ToString();
        if (dpText != null) dpText.text = anima.DP.ToString();
        if (spText != null) spText.text = anima.SP.ToString();

        if (skill1NameText != null && anima.Skill.Length > 0) 
            skill1NameText.text = anima.Skill[0];
        if (skill2NameText != null && anima.Skill.Length > 1) 
            skill2NameText.text = anima.Skill[1];
        */
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