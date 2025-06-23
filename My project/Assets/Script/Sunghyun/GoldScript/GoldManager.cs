using BansheeGz.BGDatabase;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }
    
    private TextMeshProUGUI goldText;
    [SerializeField] private string goldTextObjectName = "GoldText";
    [SerializeField] private string goldFormat = "{0:N0}";
    
    private BGRepo database = BGRepo.I;
    private BGMetaEntity goldTable;
    private BGEntity entity;
    private int currentGold;
    
    public event System.Action<int> OnGoldChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            goldTable = database.GetMeta("GoldData");
            entity = goldTable.FirstOrDefault(e => e.Get<string>("name").Equals("GoldData"));
            currentGold = entity.Get<int>("Gold");
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGoldTextInScene();
    }
    
    private void FindGoldTextInScene()
    {
        GameObject goldTextObj = GameObject.Find(goldTextObjectName);
        
        if (goldTextObj != null)
        {
            goldText = goldTextObj.GetComponent<TextMeshProUGUI>();
            if (goldText != null)
                UpdateGoldDisplay();
            else
                Debug.LogWarning($"GameObject '{goldTextObjectName}' found but doesn't have TextMeshProUGUI component");
        }
        else
        {
            goldText = null;
        }
    }
    
    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        
        currentGold += amount;
        entity.Set<int>("Gold", currentGold);
        OnGoldChanged?.Invoke(currentGold);
        UpdateGoldDisplay();
    }
    
    public bool SpendGold(int amount)
    {
        if (amount <= 0) return false;
        
        if (currentGold >= amount)
        {
            currentGold -= amount;
            entity.Set<int>("Gold", currentGold);
            OnGoldChanged?.Invoke(currentGold);
            UpdateGoldDisplay();
            return true;
        }
        
        return false;
    }
    
    public int GetCurrentGold()
    {
        return currentGold;
    }
    
    private void UpdateGoldDisplay()
    {
        if (goldText != null)
            goldText.text = string.Format(goldFormat, currentGold);
    }
}