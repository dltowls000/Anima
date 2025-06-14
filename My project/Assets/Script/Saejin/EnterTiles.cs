using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTiles : MonoBehaviour
{
    public void enterTile(RegionController target)
    {
        if (target.isVillaged)
        {
            SceneManager.LoadScene("VillageScene");
        }
        else if (target.isCleared)
        {
            return;
        }
        else
        {
            SceneManager.LoadScene("BattleScene");
        }
    }
}
