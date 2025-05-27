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
                }
            });
        }
    }

    void Refresh()
    {
        // 슬롯 모두 제거
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        // 감정 필터링 or 전체
        List<AnimaEntry> animaList = currentFilter == null
            ? CorridorManager.Instance.GetAllAnima()
            : CorridorManager.Instance.GetByEmotion(currentFilter.Value);

        foreach (var anima in animaList)
        {
            GameObject obj = Instantiate(slotPrefab, gridParent);
            AnimaSlot slot = obj.GetComponent<AnimaSlot>();

            bool discovered = CorridorManager.Instance.IsDiscovered(anima);
            slot.Setup(anima, discovered);
            slot.onClick.AddListener(ShowDetail);
        }

        // 첫 항목 자동 표시
        if (animaList.Count > 0)
            ShowDetail(animaList[0]);
    }

    void ShowDetail(AnimaEntry anima)
    {
        bool discovered = CorridorManager.Instance.IsDiscovered(anima);
        detailPanel.Display(anima, discovered);
    }
}