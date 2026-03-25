using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData Data;

    private float CurHP;
    private float Defense => Data.defense;

    private EnemySpawner spawner;

    public Action<float, float> OnHPChanged;

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

        if (CurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerLevelSystem levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        levelSystem.AddExp((int)Data.rewardExp);

        EventBus.Publish(new GoldRewardEvent(Data.rewardGold, transform.position));

        spawner.ReturnToPool(this);
    }

    public void ResetEnemy()
    {
        CurHP = Data.maxHP;
        OnHPChanged?.Invoke(CurHP, Data.maxHP);
    }
}
