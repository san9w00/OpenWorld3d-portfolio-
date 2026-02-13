using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private Collider hitbox;

    private void Awake()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
        hitbox = GetComponent<Collider>();

        hitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        if (target == null) return;

        float damage = GetComponentInParent<PlayerStatus>().curAtkDamage;
        target.TakeDamage(damage);
    }

    // 애니메이션이벤트로 한단계 건너서 호출될예정
    public void EnableHitbox()
    {
        hitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitbox.enabled = false;
    }
}
