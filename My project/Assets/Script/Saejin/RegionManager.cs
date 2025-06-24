using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionManager : MonoBehaviour
{
    public RegionController startRegion;
    public int stageType;
    public GameObject tileMap;
    public GameObject cameraRegion;
    public GameObject cameraSet;
    private RegionController currentRegion;
    private Vector3 pointerDownPos;
    private bool isDragging = false;
    public float dragThreshold = 10f;
    public GameObject enterTileManager;
    public EnterTiles enterTiles;
    private GameObject managerOB;
    private DontDesManager manager;

    [SerializeField] RegionManager regionPrefab;

    void Start()
    {
        int randomSelectTile = Random.Range(0, 3);
        cameraSet = GameObject.Find("Main Camera");
        var camSet = cameraSet.GetComponent<CameraController>();
        if (stageType == 0)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Felix1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-5.4f, -2.91f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-1.1532f, 0.0288f, 1);
                cameraRegion.transform.localScale = new Vector3(16.30603f, 10.21243f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Felix2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-5.4f, 3.9f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-2.07f, 0, 1);
                cameraRegion.transform.localScale = new Vector3(13.1517f, 12, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Felix3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-1, 0, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-2.7677f, -0.2595f, 1);
                cameraRegion.transform.localScale = new Vector3(13.07725f, 14.13319f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 1)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Phobia1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-7.17f, 0, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-2.5082f, -0.519f, 1);
                cameraRegion.transform.localScale = new Vector3(17.28692f, 11.76941f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Phobia2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(0, 0, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(0.5478f, 0.6054f, 1);
                cameraRegion.transform.localScale = new Vector3(18.32446f, 12.63486f, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Phobia3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(5.91f, 4.59f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(2.2488f, 0.7495f, 1);
                cameraRegion.transform.localScale = new Vector3(14.92197f, 12.80818f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 2)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Odium1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(0, 0, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-2.8252f, -0.6055f, 1);
                cameraRegion.transform.localScale = new Vector3(15.84424f, 12.51997f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Odium2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(5.36f, -5.06f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(0.7785f, -1.7298f, 1);
                cameraRegion.transform.localScale = new Vector3(16.59402f, 12.8079f, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Odium3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(0.17f, -5.06f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(0.2883f, -0.2883f, 1);
                cameraRegion.transform.localScale = new Vector3(15.61437f, 16.03652f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 3)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Amare1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-1.8f, 0.81f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.9225f, 0.8937f, 1);
                cameraRegion.transform.localScale = new Vector3(19.53519f, 11.25219f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Amare2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(0.33f, 4.68f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.2594f, -0.0577f, 1);
                cameraRegion.transform.localScale = new Vector3(13.13397f, 15.23115f, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Amare3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(6.2f, -4.3f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(1.0667f, -0.7496f, 1);
                cameraRegion.transform.localScale = new Vector3(17.97808f, 12.58015f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 4)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Irascor1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-6.581469f, -4.221349f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-5.626893f, -4.260386f, 1);
                cameraRegion.transform.localScale = new Vector3(18.20955f, 13.32993f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Irascor2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(7.7f, -0.43f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.3749f, -0.4613f, 1);
                cameraRegion.transform.localScale = new Vector3(25.01435f, 11.4265f, 1);
                camSet.setMaxMin();
            }
            else 
            {
                tileMap = Resources.Load<GameObject>("Saejin/Irascor3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(4.47f, 4.55f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(0.7783f, 0.0865f, 1);
                cameraRegion.transform.localScale = new Vector3(18.09665f, 14.13547f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 5)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Lacrima1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-7.1f, 3.87f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.8073f, 0.3748f, 1);
                cameraRegion.transform.localScale = new Vector3(22.07382f, 13.55875f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Lacrima2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(4.992178f, -5.217116f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(1.8816f, -0.0576f, 1);
                cameraRegion.transform.localScale = new Vector3(17.50384f, 15.80637f, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Lacrima3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-7.02f, -4.43f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.4037f, -0.7495f, 1);
                cameraRegion.transform.localScale = new Vector3(22.65117f, 14.42331f, 1);
                camSet.setMaxMin();
            }
        }
        else if (stageType == 6)
        {
            if (randomSelectTile == 0)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Havet1");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-7.02f, -0.3f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.1731f, 0.4325f, 1);
                cameraRegion.transform.localScale = new Vector3(21.95878f, 10.21416f, 1);
                camSet.setMaxMin();
            }
            else if (randomSelectTile == 1)
            {
                tileMap = Resources.Load<GameObject>("Saejin/Havet2");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(-0.63f, -0.16f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(-0.6056f, -0.0865f, 1);
                cameraRegion.transform.localScale = new Vector3(20.63311f, 11.82863f, 1);
                camSet.setMaxMin();
            }
            else
            {
                tileMap = Resources.Load<GameObject>("Saejin/Havet3");
                GameObject map = Instantiate(tileMap, new Vector3(0, 0, 0), Quaternion.identity);
                map.name = "Tiles";
                startRegion = map.GetComponentInChildren<RegionController>();
                Camera.main.transform.position = new Vector3(9.06f, 0.21f, -10);
                cameraRegion = GameObject.Find("CameraLocation");
                cameraRegion.transform.position = new Vector3(0.5995f, -1.5627f, 1);
                cameraRegion.transform.localScale = new Vector3(24.36836f, 12.97393f, 1);
                camSet.setMaxMin();
            }
        }
        foreach (var reg in Object.FindObjectsByType<RegionController>(FindObjectsSortMode.None))
        {
            reg.gameObject.SetActive(reg == startRegion);
        }
        currentRegion = startRegion;
        managerOB = GameObject.Find("DontDesManager");
        manager = managerOB.GetComponent<DontDesManager>();
        manager.setDesGrid();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointerDownPos = Input.mousePosition;
            isDragging = false;
        }

        if (Input.GetMouseButton(0))
        {
            if (!isDragging &&
                Vector3.Distance(pointerDownPos, Input.mousePosition) > dragThreshold)
            {
                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!isDragging)
            {
                HandleClick();
            }
        }
    }

    void HandleClick()
    {
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(wp, Vector2.zero);
        if (hit.collider == null) return;

        var target = hit.collider.GetComponentInParent<RegionController>();
        if (target == null || !target.gameObject.activeSelf) return;

        EnterBattle(target);
    }

    void EnterBattle(RegionController target)
    {
        if (target.isCleared && !target.isVillaged)
        {
            return;
        }
        else if (target.isVillaged)
        {
            string a = target.name;
            target.name.EndsWith("VillageArea");
            target.name.EndsWith("VillageArea 2");
            //VillageDataManager.Instance.SetCurrentVillageID(villageTile.name);
            SceneManager.LoadScene("VillageScene"); 
        }
        else if (target.name.StartsWith("Boss"))
        {
            if (SceneManager.GetActiveScene().name.EndsWith("BossFieldScene"))
            {
                SceneManager.LoadScene("BossBattleScene");
            }
            else
            {
                switch (stageType)
                {
                    case 0:
                        SceneManager.LoadScene("FelixEliteBattleScene");
                        break;
                    case 1:
                        SceneManager.LoadScene("PhobiaEliteBattleScene");
                        break;
                    case 2:
                        SceneManager.LoadScene("OdiumEliteBattleScene");
                        break;
                    case 3:
                        SceneManager.LoadScene("AmareEliteBattleScene");
                        break;
                    case 4:
                        SceneManager.LoadScene("IrascorEliteBattleScene");
                        break;
                    case 5:
                        SceneManager.LoadScene("LacrimaEliteBattleScene");
                        break;
                    case 6:
                        SceneManager.LoadScene("HavetEliteBattleScene");
                        break;
                }
            }
        }
        else
        {
            switch (stageType)
            {
                case 0:
                    SceneManager.LoadScene("FelixBattleScene");
                    break;
                case 1:
                    SceneManager.LoadScene("PhobiaBattleScene");
                    break;
                case 2:
                    SceneManager.LoadScene("OdiumBattleScene");
                    break;
                case 3:
                    SceneManager.LoadScene("AmareBattleScene");
                    break;
                case 4:
                    SceneManager.LoadScene("IrascorBattleScene");
                    break;
                case 5:
                    SceneManager.LoadScene("LacrimaBattleScene");
                    break;
                case 6:
                    SceneManager.LoadScene("HavetBattleScene");
                    break;
            }
        }
        manager.SetTile(target, this);
        target.isCleared = true;
        currentRegion = target;
    }

    public void SetNextTile(RegionController target)
    {
        foreach (var nb in target.neighbors)
        {
            if (nb.name.StartsWith("Village"))
            {
                nb.isVillaged = true;
            }
            nb.gameObject.SetActive(true);
        }
    }
}
