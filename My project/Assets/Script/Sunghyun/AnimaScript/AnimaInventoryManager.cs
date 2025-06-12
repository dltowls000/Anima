using System.Collections.Generic;
using UnityEngine;

public class AnimaInventoryManager : MonoBehaviour
{
    public static AnimaInventoryManager Instance { get; private set; }
    
    [SerializeField] private List<AnimaDataSO> playerAnima = new List<AnimaDataSO>();
    [SerializeField] private List<AnimaDataSO> activeAnima = new List<AnimaDataSO>();
    
    public event System.Action OnAnimaInventoryChanged;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public List<AnimaDataSO> GetAllAnima()
    {
        return playerAnima;
    }
    
    public List<AnimaDataSO> GetActiveAnima()
    {
        return activeAnima;
    }
    
    public void AddAnima(AnimaDataSO anima)
    {
        if (anima != null && !playerAnima.Contains(anima))
        {
            playerAnima.Add(anima);
            OnAnimaInventoryChanged?.Invoke();
        }
    }
    
    public void SetActiveAnima(List<AnimaDataSO> selected)
    {
        activeAnima.Clear();
        foreach (var anima in selected)
        {
            if (playerAnima.Contains(anima))
            {
                activeAnima.Add(anima);
            }
        }
        OnAnimaInventoryChanged?.Invoke();
    }
}