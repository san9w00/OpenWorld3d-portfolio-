using System.Collections;
using UnityEngine;

public class BossAI : EnemyAI
{
    [Header("Patterns")]
    public BossPatternSO[] patterns;

    [Header("Phase2")]
    [SerializeField] private GameObject bossModel;
    [SerializeField] private GameObject skeletonPrefab;
    [SerializeField] private Transform[] spawnPoints;
    private int aliveSkeletons;

    [HideInInspector] public bool isPhase2 = false;

    // ÇöÀç °ø°Ý µ¥¹ÌÁö
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
        return isPhase2 ? baseDamage * 1.5f : baseDamage;
    }

    private IEnumerator PhaseTransition()
    {
        agent.isStopped = true;
        animator.SetBool("IsMoving", false);

        // º¸½º ¼û±è
        bossModel.SetActive(false);

        // ÇØ°ñ »ý¼º
        SpawnSkeletons();

        yield return null;
    }

    public void OnBossDeath()
    {
        StartCoroutine(PhaseTransition());
    }

    private void ReviveBoss()
    {
        bossModel.SetActive(true);

        isPhase2 = true;

        GetComponent<EnemyStatus>().ResetEnemy();
    }

    private void SpawnSkeletons()
    {
        aliveSkeletons = spawnPoints.Length;

        foreach (var point in spawnPoints)
        {
            GameObject skel = Instantiate(skeletonPrefab, point.position, Quaternion.identity);

            EnemyStatus status = skel.GetComponent<EnemyStatus>();
            status.OnDeath += OnSkeletonDeath;
        }
    }

    private void OnSkeletonDeath()
    {
        aliveSkeletons--;

        if (aliveSkeletons <= 0)
        {
            ReviveBoss();
        }
    }
}
