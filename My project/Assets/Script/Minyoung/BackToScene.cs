using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToScene : MonoBehaviour
{
    private GameObject gameManager;
    public void backToScenes()
    {
        gameManager = GameObject.Find("Game Manager");
        var gameM = gameManager.GetComponent<SceneManagerCorridor>();
        SceneManager.LoadScene(gameM.sceneName);
    }
}
