using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalScrollByWheel : MonoBehaviour, IScrollHandler
{
    public ScrollRect scrollRect;
    public float scrollSensitivity = 0.03f; // 이 값을 높이면 더 빨라짐

    public void OnScroll(PointerEventData eventData)
    {
        // eventData.scrollDelta.y는 휠 위(+1), 아래(-1)
        scrollRect.horizontalNormalizedPosition -= eventData.scrollDelta.y * scrollSensitivity;
    }
}