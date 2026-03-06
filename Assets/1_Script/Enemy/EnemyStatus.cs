using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData Data;

    private float CurHP;
    private float Defense => Data.defense;

    private EnemySpawner spawner;

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

        if (CurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        PlayerLevelSystem levelSystem = FindAnyObjectByType<PlayerLevelSystem>();
        levelSystem.AddExp((int)Data.rewardExp);

        spawner.ReturnToPool(this);
    }

    public void ResetEnemy()
    {
        CurHP = Data.maxHP;
    }
}
