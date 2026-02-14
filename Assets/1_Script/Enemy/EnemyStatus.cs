using UnityEngine;

public class EnemyStatus : MonoBehaviour, IDamageable
{
    public EnemyData Data;
    private Animator animator;

    private float curHP;
    private float curAtkDamage => Data.atkDamage;
    private float defense => Data.defense;

    private void Awake()
    {
        curHP = Data.maxHP;

        animator = GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        float finalAttack = Mathf.Max(1, damage - defense);

        curHP -= finalAttack;
        animator.SetTrigger("Hit");

        Debug.Log($"°õ ³²ÀºÃ¼·Â: {curHP}");

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
