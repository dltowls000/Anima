using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBattleSetting 
{
    List<float> DamageX { get; }
    List<float> DamageY { get; }
    List<GameObject> EnemyObjPrefab { get; }
    List<GameObject> EnemyInstance { get; }
    string ObjName { get; }
    List<string> ObjectFileList { get; }
    List<string> BattleEnemyAnima { get; }
    List<GameObject> EnemyHpPrefab { get; }
    List<GameObject> EnemyHpInstance { get; }
    List<GameObject> EnemyInfoPrefab { get; }
    List<GameObject> EnemyInfoInstance { get; }
    List<GameObject> EnemyParserPrefab { get; }
    List<GameObject> EnemyParserInstance { get; }
    GameObject Canvas { get; }
    GameObject BattleParser { get; }
    string Stage { get; }
}
