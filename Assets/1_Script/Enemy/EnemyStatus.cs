using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData Data;

    private float CurHP;
    private float Defense => Data.defense;

    private EnemySpawner spawner;

    public Action<float, float> OnHPChanged;

    public Action OnPhase2Trigger;
    private bool phaseTriggered = false;

    public Action OnDeath;

    [Header("Drop")]
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private int dropCount = 3;
    [SerializeField] private float dropForce = 4f;

    private void Awake()
    {
        CurHP = Data.maxHP;       
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        this.spawner = spawner;
    }

    public void TakeDamage(float damage)
    {
        float finalAttack = Mathf.Max(1, damage - Defense);

        CurHP -= finalAttack;

        Debug.Log($"남은체력: {CurHP}");

        OnHPChanged?.Invoke(CurHP, Data.maxHP);

        if (!phaseTriggered && CurHP <= Data.maxHP * 0.3f)
        {
            phaseTriggered = true;
            OnPhase2Trigger?.Invoke();
        }

        if (CurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();

        DropResources();

        PlayerLevelSystem levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        levelSystem.AddExp((int)Data.rewardExp);

        EventBus.Publish(new GoldRewardEvent(Data.rewardGold, transform.position));
        EventBus.Publish(new EnemyKilledEvent(Data.enemyType));
        EventBus.Publish(new VFXEvent(transform.position, VFXActionType.EnemyDie, VFXSwordType.None));

        if (spawner != null)
        {
            spawner.ReturnToPool(this);
        }
        else
        {
            Destroy(gameObject); // 보스는 그냥 삭제
        }
    }

    private void DropResources()
    {
        if (dropPrefab == null)
            return;

        for (int i = 0; i < dropCount; i++)
        {
            Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;
            GameObject obj = Instantiate(dropPrefab, spawnPos, Quaternion.identity);

            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 randomDir =
                    new Vector3(
                        UnityEngine.Random.Range(-1f, 1f),
                        1f,
                        UnityEngine.Random.Range(-1f, 1f));

                rb.AddForce(
                    randomDir.normalized * dropForce,
                    ForceMode.Impulse);
            }
        }
    }

    public void ResetEnemy()
    {
        CurHP = Data.maxHP;
        OnHPChanged?.Invoke(CurHP, Data.maxHP);
    }
}
