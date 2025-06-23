using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InteractableBuilding : MonoBehaviour
{
    [Header("건물 설정")]
    [SerializeField] private string buildingName;
    [SerializeField] private BuildingType buildingType;
    [SerializeField] private Vector3 nameDisplayOffset = new Vector3(0, 1.5f, 0);
    
    private VillageManager villageManager;
    
    public enum BuildingType
    {
        Inn,
        Shop,
        Corridor,
        MagicTree
    }
    
    private void Awake()
    {
        villageManager = FindObjectOfType<VillageManager>();
    }
    
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        villageManager.PlayClickSound();
        
        switch (buildingType)
        {
            case BuildingType.Inn:
                break;
                
            case BuildingType.Shop:
                villageManager.OpenShop();
                break;
                
            case BuildingType.Corridor:
                LoadCorridorScene();
                break;
                
            case BuildingType.MagicTree:
                LoadMixScene();
                break;
        }
    }
    
    private void LoadCorridorScene()
    {
        SceneManager.LoadScene("CorridorScene");
    }
    
    private void LoadMixScene()
    {
        SceneManager.LoadScene("MixScene");
    }
    
    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
            
        Vector3 displayPosition = transform.position + nameDisplayOffset;
        villageManager.ShowBuildingName(buildingName, displayPosition);
    }

    private void OnMouseExit()
    {
        if (villageManager != null)
            villageManager.HideBuildingName();
    }
}