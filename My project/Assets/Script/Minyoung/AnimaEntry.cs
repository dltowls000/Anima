using UnityEngine;

public enum EmotionType
{
    Wrath,    // 분노 
    Void,     // 공허 
    Love,     // 사랑
    Desire,   // 욕망  
    Joy,      // 즐거움 
    Fear,     // 두려움
    Sorrow,   // 슬픔
    Gaudium   // 기쁨
}

[CreateAssetMenu(fileName = "NewAnima", menuName = "Corridor/Anima Entry")]
public class AnimaEntry : ScriptableObject
{
    [Header("기본 정보")]
    public string animaId;              // 고유 ID (저장/동기화용)
    public string animaName;
    [TextArea]
    public string description;
    public EmotionType emotion;        // 감정 종류로 도감 탭 필터링에 사용

    [Header("이미지")]
    public Sprite colorImage;          // 발견 후 표시용
    public Sprite silhouetteImage;     // 발견 전 표시용 (흑백 실루엣)

    // 확장 가능: 전투력, 패시브, 애니메이션 등 추가 가능
    // public int powerLevel;
    // public string passiveSkill;
}