using UnityEngine;
using System.Collections;

public class VillageController : MonoBehaviour
{
    [Header("오디오")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip clickSound;
    
    [Header("UI 관리")]
    [SerializeField] private GameObject buildingNamePanel;
    [SerializeField] private TMPro.TextMeshProUGUI buildingNameText;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private InnUIManager innUIManager;
    
    [SerializeField] private float fadeSpeed = 5f;
    private CanvasGroup _nameCanvasGroup;

    private AudioSource _sfxSource;
    private Camera _mainCamera;
    
    private void Awake()
    {
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _mainCamera = Camera.main;
        
        if (buildingNamePanel != null)
            buildingNamePanel.SetActive(false);
        
        if (buildingNamePanel != null)
        {
            _nameCanvasGroup = buildingNamePanel.GetComponent<CanvasGroup>();
            if (_nameCanvasGroup == null)
                _nameCanvasGroup = buildingNamePanel.AddComponent<CanvasGroup>();
        }
    }

    
    private void Start()
    {
        if (bgmSource != null && bgmSource.clip != null)
            bgmSource.Play();
    }
    
    public void PlayClickSound()
    {
        if (clickSound != null && _sfxSource != null)
            _sfxSource.PlayOneShot(clickSound);
    }
    
    public void ShowBuildingName(string buildingName, Vector3 worldPosition)
    {
        if (buildingNamePanel == null || buildingNameText == null)
            return;
            
        buildingNameText.text = buildingName;
        
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(worldPosition);
        
        screenPosition.y += 50f;
        buildingNamePanel.transform.position = screenPosition;
        
        buildingNamePanel.SetActive(true);
        
        _nameCanvasGroup.alpha = 0f;
        StopAllCoroutines();
        StartCoroutine(FadePanelIn());
    }

    
    public void HideBuildingName()
    {
        StopAllCoroutines();
        StartCoroutine(FadePanelOut());
    }
    
    private IEnumerator FadePanelIn()
    {
        while (_nameCanvasGroup.alpha < 1f)
        {
            _nameCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _nameCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadePanelOut()
    {
        while (_nameCanvasGroup.alpha > 0f)
        {
            _nameCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _nameCanvasGroup.alpha = 0f;
        buildingNamePanel.SetActive(false);
    }

    public void OpenShop()
    {
        if (ShopUIManager.Instance != null)
        {
            ShopUIManager.Instance.OpenShopPanel();
        }
    }
    
    public void CloseShop()
    {
        if (ShopUIManager.Instance != null)
        {
            ShopUIManager.Instance.CloseShopPanel();
        }
    }
    
    public void OpenInn()
    {
        if (innUIManager != null)
        {
            innUIManager.OpenInnPanel();
        }
    }
}