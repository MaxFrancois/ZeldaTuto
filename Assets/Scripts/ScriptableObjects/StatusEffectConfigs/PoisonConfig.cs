using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoisonConfig", menuName = "StatusEffectConfigs/PoisonConfig")]
public class PoisonConfig : StatusEffectConfig
{
    [Header("Poison")]
    public float DotTickDamage;
    public float DotTickSpeed;
    public Color PoisonedEnemyColor;
}
