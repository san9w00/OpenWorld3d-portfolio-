using UnityEngine;

public enum BossAttackType
{
    Normal,
    TeleportBite
}

[CreateAssetMenu(menuName = "Boss/Attack")]
public class BossAttackSO : ScriptableObject
{
    public BossAttackType attackType;
    public string animationTrigger;
    public float damage;
    public float duration; // 애니메이션 길이 or 딜레이

    // 순간이동 전용
    public float teleportDelay = 1f;
    public float aoeDamage = 5f;
    public float aoeRadius = 3f;
}
