using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
    
public class ParserData : MonoBehaviour
{
    bool damagecheck = false;
    bool healcheck = false;
    bool logcheck = false;
    [SerializeField]
    GameObject battleParser;
    [SerializeField]
    GameObject battleLog;
    [SerializeField]
    GameObject maxButton;
    [SerializeField]
    GameObject minButton;
    public void OnClickParser()
    {
        if (!damagecheck)
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 1;
            battleParser.GetComponent<CanvasGroup>().interactable = true;
            damagecheck = true;
        }
        else
        {
            battleParser.GetComponent<CanvasGroup>().alpha = 0;
            battleParser.GetComponent<CanvasGroup>().interactable = false;
            damagecheck = false;
        }
    }
    public void OpenLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 1;
        battleLog.GetComponent <CanvasGroup>().interactable = true;
        maxButton.SetActive(false);
        minButton.SetActive(true);
    }
    public void CloseLog()
    {
        battleLog.GetComponent<CanvasGroup>().alpha = 0;
        battleLog.GetComponent<CanvasGroup>().interactable = false;
        minButton.SetActive(false);
        maxButton.SetActive(true) ;
    }
}
