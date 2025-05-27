using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager Instance { get; private set; }

    [Header("전체 아니마 데이터베이스")]
    public List<AnimaEntry> animaDatabase;  // 에디터에서 드래그할 ScriptableObject들

    private HashSet<string> discoveredAnimaIds = new();  // 발견된 animaId 목록

    void Start()
    {
        // 테스트용: 나중에 지울것
        var target = animaDatabase.Find(x => x.animaId == "gaudium1");
        if (target != null)
            MarkDiscovered(target);
    }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // 추후 LoadDiscoveredData(); 추가 가능
    }

    // 모든 Anima 반환
    public List<AnimaEntry> GetAllAnima()
    {
        return animaDatabase;
    }

    // 감정별 필터
    public List<AnimaEntry> GetByEmotion(EmotionType emotion)
    {
        return animaDatabase.Where(a => a.emotion == emotion).ToList();
    }

    // 발견 여부 조회
    public bool IsDiscovered(AnimaEntry entry)
    {
        return discoveredAnimaIds.Contains(entry.animaId);
    }

    // 발견 처리
    public void MarkDiscovered(AnimaEntry entry)
    {
        if (!discoveredAnimaIds.Contains(entry.animaId))
        {
            discoveredAnimaIds.Add(entry.animaId);
            // 추후 SaveDiscoveredData(); 연결 가능
        }
    }

    // 저장/불러오기 기능은 나중에 추가해도 됩니다
}