using UnityEngine;

public class SkillGizmoDrawer : MonoBehaviour
{
    private PlayerEquipment equipment;

    private void Awake()
    {
        equipment = GetComponent<PlayerEquipment>();
    }

    private void OnDrawGizmos()
    {
        if (equipment == null) return;
        if (equipment.CurrentWeapon == null) return;

        WeaponHitbox hitbox = equipment.CurrentWeapon.GetComponent<WeaponHitbox>();
        if (hitbox == null) return;

        WeaponItemSO weaponData = hitbox.WeaponData;
        if (weaponData == null) return;

        if (weaponData.skill != null)
        {
            weaponData.skill.DrawGizmo(transform.position);
        }
    }
}
