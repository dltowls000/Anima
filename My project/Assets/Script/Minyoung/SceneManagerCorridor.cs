using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerCorridor : MonoBehaviour
{
    public string sceneName;

    void Awake()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnSceneUnloaded(Scene scene)
    {
        sceneName = scene.name;
    }
}
