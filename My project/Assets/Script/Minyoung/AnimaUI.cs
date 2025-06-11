using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimaUI : MonoBehaviour
{
    [Header("도감 슬롯 관련")]
    public Transform gridParent;       // ScrollView > Content
    public GameObject slotPrefab;      // AnimaSlot 프리팹

    [Header("상세 정보 패널")]
    public AnimaDetailUI detailPanel;

    [Header("탭 버튼 (전체 + 감정 8개)")]
    public Toggle[] emotionToggles;    // 0: 전체, 1~8: 감정순

    private EmotionType? currentFilter = null;

    void OnEnable()
    {
        SetupTabs();
        Refresh();
    }

    void SetupTabs()
    {
        for (int i = 0; i < emotionToggles.Length; i++)
        {
            int index = i;

            emotionToggles[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    currentFilter = (index == 0) ? (EmotionType?)null : (EmotionType)(index - 1);
                    Refresh();

                    for (int j = 0; j < emotionToggles.Length; j++)
                    {
                        emotionToggles[j].interactable = (j != index);
                    }
                }
            });
        }
    }

    void Refresh()
    {
        // 슬롯 제거
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // 감정별 필터링 및 전체
        List<AnimaEntry> animaList = currentFilter == null
            ? CorridorManager.Instance.GetAllAnima()
            : CorridorManager.Instance.GetByEmotion(currentFilter.Value);

        foreach (var anima in animaList)
        {
            bool discovered = CorridorManager.Instance.IsDiscovered(anima);

            // 발견되지 않은 애는 감정 필터에서 제외
            if (currentFilter != null && !discovered)
                continue;

            GameObject obj = Instantiate(slotPrefab, gridParent);
            AnimaSlot slot = obj.GetComponent<AnimaSlot>();
            slot.Setup(anima, discovered);
            slot.onClick.AddListener(ShowDetail);
        }

        // 첫 항목 자동 표시
        if (gridParent.childCount > 0)
        {
            AnimaEntry firstAnima = animaList.Find(a => currentFilter == null || CorridorManager.Instance.IsDiscovered(a));
            if (firstAnima != null)
                ShowDetail(firstAnima);
        }
    }

    void ShowDetail(AnimaEntry anima)
    {
        bool discovered = CorridorManager.Instance.IsDiscovered(anima);
        detailPanel.Display(anima, discovered);
    }

    public void OnBackButton()
    {
        this.gameObject.SetActive(false);
    }
}