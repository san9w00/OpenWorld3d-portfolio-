using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private PlayerStatus playerStatus;
    private Collider hitbox;
    private WeaponItemSO currentWeapon;

    public WeaponItemSO WeaponData => currentWeapon;

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
        if (other.CompareTag("Player")) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target == null) return;

        ResourceObject resource = other.GetComponentInChildren<ResourceObject>();

        float damage = playerStatus.AtkDamage + currentWeapon.damage;

        if (resource != null)
        {
            if (currentWeapon.weaponType == WeaponType.Axe_TwoHand)
            {
                damage *= 4f;
            }
        }

        target.TakeDamage(damage);

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        EventBus.Publish(new VFXEvent(hitPoint, VFXActionType.Hit, currentWeapon.swordType));
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
