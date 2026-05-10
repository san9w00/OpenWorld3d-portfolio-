using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    [Header("Patterns")]
    public BossPatternSO[] patterns;

    [Header("Boss Model")]
    [SerializeField] public GameObject bossModel;

    [Header("Breath")]
    public GameObject fireBreathObject;

    [Header("HP UI")]
    [SerializeField] private BossHPUI bossHPUI;

    // 현재 공격 데미지
    [HideInInspector] public float currentAttackDamage;

    protected override void Awake()
    {
        base.Awake();

        EnemyStatus status = GetComponent<EnemyStatus>();
        bossHPUI.Init(status);
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
