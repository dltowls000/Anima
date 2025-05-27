using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class AnimaSlot : MonoBehaviour
{
    [Header("UI 요소")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;

    private AnimaEntry animaEntry;

    // 클릭 이벤트: 외부에서 리스너 등록 가능
    public UnityEvent<AnimaEntry> onClick = new();

    // 슬롯 초기화
    public void Setup(AnimaEntry entry, bool isDiscovered)
    {
        animaEntry = entry;

        iconImage.sprite = entry.colorImage; // 추후 실루엣 처리 추가 가능
        nameText.text = isDiscovered ? entry.animaName : "???";
    }

    // UI 버튼에서 이걸 연결
    public void OnSlotClicked()
    {
        onClick.Invoke(animaEntry);
    }
}