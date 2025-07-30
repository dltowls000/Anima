using BansheeGz.BGDatabase;
using DamageNumbersPro;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IBattleManager
{
    void WinBattle();
    void LoseBattle();
    PlayerInfo PlayerInfo { get; }
    BattleState stat { get; set; }
    int TurnIndex { get; set; }
    int EnemyAnimaNum { get; set; }
    int AllyAnimaNum { get; set; }
    bool IsZKeyPressed { get; set; }
    public Coroutine RunningCoroutine { get; set; }

    List<int> DieAllyAnima { get; }
    List<AnimaActions> AllyActions { get; }
    List<EnemyActions> EnemyActions { get; }
    List<GameObject> Turn { get; }
    List<AnimaDataSO> TurnList { get;  }
    List<AnimaDataSO> TmpturnList { get; }
    List<GameObject> IsTurn { get; }
    List<AnimaDataSO> DropAnima { get; }

    Button AttackButton { get; }
    Button SkillButton { get; }
    GameObject AnimaActionUI { get; }
    GameObject Canvas { get; }
    CameraManager CameraManager { get; }
    DamageNumber DamageNumber { get; }

    IAllyBattleSetting AllyBattleSetting { get; }
    IEnemyBattleSetting EnemyBattleSetting { get; }

    float MaxValue { get; set; }
    List<ParserBar> AllyDamageBar { get; }
    List<ParserBar> EnemyDamageBar { get; }
    List<ParserBar> AllyHealBar { get; }
    List<ParserBar> EnemyHealBar { get; }
    List<TextMeshProUGUI> AllyDamageText { get; }
    List<TextMeshProUGUI> EnemyDamageText { get; }
    List<TextMeshProUGUI> AllyHealText { get; }
    List<TextMeshProUGUI> EnemyHealText { get; }
    List<HealthBar> AllyHealthBar { get; }
    List<HealthBar> EnemyHealthBar { get; }

    BGMetaEntity AnimaTable { get; }
    BattleLogManager BattleLogManager { get; }
    
    BuffManager BuffManager { get; }
}
