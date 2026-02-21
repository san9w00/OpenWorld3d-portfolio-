using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private Collider hitbox;
    private WeaponItemSO currentWeapon;

    private void Awake()
    {
        playerStatus = GetComponentInParent<PlayerStatus>();
        hitbox = GetComponent<Collider>();

        hitbox.enabled = false;
    }

    public void SetWeaponData(WeaponItemSO weaponData)
    {
        currentWeapon = weaponData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentWeapon == null) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null) return;

        float damage = playerStatus.AtkDamage + currentWeapon.damage;
        target.TakeDamage(damage);

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        DamageEventData data = new DamageEventData(
            hitPoint,
            currentWeapon.hitEffectVFX
        );

        GameEvents.OnUnitDamaged?.Invoke(data);
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
