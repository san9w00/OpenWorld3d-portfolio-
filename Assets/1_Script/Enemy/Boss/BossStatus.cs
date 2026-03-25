using UnityEngine;

public class BossStatus : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private float maxHP = 250f;
    [SerializeField] private float defence = 3f;
    public float attackDamage = 20f;

    private float curHP;

    private void Awake()
    {
        curHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        float finalAttack = Mathf.Max(1, damage - defence);

        curHP -= finalAttack;

        Debug.Log($"남은체력: {curHP}");

        if (curHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
