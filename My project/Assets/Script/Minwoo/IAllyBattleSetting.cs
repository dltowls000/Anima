using System.Collections.Generic;
using UnityEngine;

public interface IAllyBattleSetting
{
    GameObject Canvas { get; }
    List<GameObject> AllyObjPrefab { get; }
    List<GameObject> AllyInstance { get; }
    List<GameObject> AllyInfoPrefab { get; }
    List<GameObject> AllyInfoInstance { get; }
    string ObjName { get; }
    PlayerInfo PlayerInfo { get; }
    GameObject Prefab { get; }
    List<GameObject> AllyHpPrefab { get; }
    List<GameObject> AllyHpInstance { get; }
    List<float> DamageX { get; }
    List<float> DamageY { get; }

    List<GameObject> AllyParserPrefab { get; }
    List<GameObject> AllyParserInstance { get; }
    GameObject BattleParser { get; }
}
