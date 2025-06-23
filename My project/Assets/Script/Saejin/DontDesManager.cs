using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDesManager : MonoBehaviour
{
    public static DontDesManager Instance { get; private set; }

    public GameObject manager;
    public GameObject mapScreen;
    public GameObject grid;
    private StageNode stageNode;
    private RegionController tile;
    private RegionManager tileManager;
    private string lastUnloaded;
    private Camera tileCam;
    private Vector3 camPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(manager);
            DontDestroyOnLoad(mapScreen);

            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
        else
        {
            Destroy(gameObject);
            Destroy(mapScreen);
        }
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MapScene")
        {
            mapScreen.SetActive(true);
            EnterStage es = stageNode.GetComponent<EnterStage>();
            es.SetLineColor(stageNode);
        }

        if (scene.name == "FelixFieldScene" && lastUnloaded == "BattleScene")
        {
            if (grid == null)
            {
                var tilePrefab = GameObject.Find("Tiles");
                if (tilePrefab != null)
                {
                    grid = tilePrefab;
                    DontDestroyOnLoad(grid);
                }
            }
            else
            {
                foreach (var root in scene.GetRootGameObjects())
                {
                    if (root.name == "Tiles" && root != grid)
                    {
                        Destroy(root);
                        break;
                    }
                }
            }
            var cam = GameObject.Find("Main Camera");
            tileCam = cam.GetComponent<Camera>();
            tileCam.transform.position = camPosition;
            grid.SetActive(true);
            tileManager.SetNextTile(tile);
        }
        else if (scene.name == "FelixFieldScene")
        {
            mapScreen.SetActive(false);
            var tilePrefab = GameObject.Find("Tiles");
            grid = tilePrefab;
            DontDestroyOnLoad(grid);
        }

        if (scene.name == "BattleScene" && lastUnloaded == "FelixFieldScene")
        {
            grid.SetActive(false);
        }
    }
    public void OnSceneUnloaded(Scene scene)
    {
        lastUnloaded = scene.name;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void SetStage(StageNode node)
    {
        stageNode = node;
    }

    public void SetTile(RegionController target, RegionManager m)
    {
        tileManager = m;
        tile = target;
    }

    public void setCamPosition(Vector3 cp)
    {
        Debug.Log($"Camera Position : {cp}");
        camPosition = cp;
    }
}
