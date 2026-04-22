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
    private bool isDead = false;

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

        PlayerLevelSystem levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        levelSystem.AddExp((int)Data.rewardExp);

        EventBus.Publish(new GoldRewardEvent(Data.rewardGold, transform.position));
        EventBus.Publish(new EnemyKilledEvent(Data.enemyType));

        if (spawner != null)
        {
            spawner.ReturnToPool(this);
        }
        else
        {
            Destroy(gameObject); // 보스는 그냥 삭제
        }
    }

    public void ResetEnemy()
    {
        CurHP = Data.maxHP;
        isDead = false;
        OnHPChanged?.Invoke(CurHP, Data.maxHP);
    }
}
