using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Boss/Pattern")]
public class BossPatternSO : ScriptableObject
{
    public List<BossAttackSO> attacks;
}
