using UnityEngine;

public abstract class EnemySkillSO : ScriptableObject
{
    [Header("Animation")]
    public string animationTrigger;

    [Header("Skill")]
    public float duration;

    public abstract void Use(EnemyAI enemyAI, EnemyStatus status);
}
