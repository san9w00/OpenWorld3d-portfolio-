using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    [Header("Patterns")]
    public BossPatternSO[] patterns;

    [Header("Boss Model")]
    [SerializeField] public GameObject bossModel;

    // 현재 공격 데미지
    [HideInInspector] public float currentAttackDamage;

    protected override void Awake()
    {
        base.Awake();
    }

    public BossPatternSO GetRandomPattern()
    {
        int index = Random.Range(0, patterns.Length);
        return patterns[index];
    }

    public float GetFinalDamage(float baseDamage)
    {
        return baseDamage;
    }
}
