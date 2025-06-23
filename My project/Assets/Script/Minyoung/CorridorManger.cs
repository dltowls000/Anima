using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager Instance { get; private set; }

    [Header("전체 아니마 데이터베이스")]
    public List<AnimaEntry> animaDatabase = new();  // BGDatabase에서 로드된 데이터

    // 발견 여부: name 기준, 0 = 미발견, 1 = 발견
    private Dictionary<string, int> meetStatusMap = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Init()
    {
        animaDatabase = AnimaEntry.LoadAll();

        // 발견 정보 불러오기 (지금은 임시)
        LoadMeetedData();

        // 예시: 강제로 하나 발견 처리 (테스트용)
        if (animaDatabase.Count > 0)
        {
            MarkDiscovered(animaDatabase[0]);
        }
    }

    public bool IsDiscovered(AnimaEntry entry)
    {
        return meetStatusMap.TryGetValue(entry.name, out int value) && value >= 1;
    }

    public void MarkDiscovered(AnimaEntry entry)
    {
        if (!meetStatusMap.ContainsKey(entry.name) || meetStatusMap[entry.name] == 0)
        {
            meetStatusMap[entry.name] = 1;
            SaveMeetedData();  // 저장은 추후 구현
        }
    }

    public List<AnimaEntry> GetAllAnima()
    {
        return animaDatabase;
    }

    public List<AnimaEntry> GetByEmotion(EmotionType emotion)
    {
        return animaDatabase.Where(a => a.emotion == emotion).ToList();
    }

    // 추후 구현: 발견 정보 저장
    private void SaveMeetedData()
    {
        // 예: PlayerPrefs 또는 JSON
    }

    // 추후 구현: 발견 정보 불러오기
    private void LoadMeetedData()
    {
        // 예: PlayerPrefs 또는 JSON
        // 초기화 시 모든 meet 상태를 0으로 시작
        foreach (var anima in animaDatabase)
        {
            if (!meetStatusMap.ContainsKey(anima.name))
                meetStatusMap[anima.name] = 0;
        }
    }
}
