using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Attack")]
public class BossAttackSO : ScriptableObject
{
    public string animationTrigger;
    public float damage;
    public float duration; // 애니메이션 길이 or 딜레이
}
