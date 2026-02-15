using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData Data;
    private Animator animator;

    private float CurHP;
    private float curAtkDamage => Data.atkDamage;
    private float Defense => Data.defense;

    private void Awake()
    {
        CurHP = Data.maxHP;

        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        float finalAttack = Mathf.Max(1, damage - Defense);

        CurHP -= finalAttack;
        animator.SetTrigger("Hit");

        Debug.Log($"°õ ³²ÀºÃ¼·Â: {CurHP}");

        if (CurHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
