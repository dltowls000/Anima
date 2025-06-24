using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerInBattle : MonoBehaviour
{
    private GameObject regionManager;
    public void backToTiles()
    {
        regionManager = GameObject.Find("RegionManager");
        var regionScr = regionManager.GetComponent<RegionManager>();
        var num = regionScr.stageType;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name.EndsWith("EliteBattleScene")){
            SceneManager.LoadScene("MapScene");
        }
        else
        {
            switch (num)
            {
                case 0:
                    SceneManager.LoadScene("FelixFieldScene");
                    break;
                case 1:
                    SceneManager.LoadScene("PhobiaFieldScene");
                    break;
                case 2:
                    SceneManager.LoadScene("OdiumFieldScene");
                    break;
                case 3:
                    SceneManager.LoadScene("AmareFieldScene");
                    break;
                case 4:
                    SceneManager.LoadScene("IrascorFieldScene");
                    break;
                case 5:
                    SceneManager.LoadScene("LacrimaFieldScene");
                    break;
                case 6:
                    SceneManager.LoadScene("HavetFieldScene");
                    break;
            }
        }
    }

    public void resetGame()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
