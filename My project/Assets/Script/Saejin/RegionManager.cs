using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegionManager : MonoBehaviour
{
    public RegionController startRegion;
    private RegionController currentRegion;
    private Vector3 pointerDownPos;
    private bool isDragging = false;
    public float dragThreshold = 10f;
    public GameObject enterTileManager;
    public EnterTiles enterTiles;
    private GameObject managerOB;
    private DontDesManager manager;

    void Start()
    {
        foreach (var reg in Object.FindObjectsByType<RegionController>(FindObjectsSortMode.None))
        {
            reg.gameObject.SetActive(reg == startRegion);
        }
        currentRegion = startRegion;
        managerOB = GameObject.Find("DontDesManager");
        manager = managerOB.GetComponent<DontDesManager>();
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
            SceneManager.LoadScene("VillageScene");
        }
        else if (target.name.StartsWith("Boss"))
        {
            SceneManager.LoadScene("BossBattleScene");
        }
        manager.SetTile(target, this);
        target.isCleared = true;
        currentRegion = target;
        SceneManager.LoadScene("BattleScene");
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
