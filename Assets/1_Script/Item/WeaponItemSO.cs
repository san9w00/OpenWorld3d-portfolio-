using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItemSO : ItemSO
{
    [Header("Weapon Settings")]
    public float damage;
    public GameObject hitEffectVFX;

    [Header("Skills")]
    public WeaponSkillSO skill;

    public override void Use(GameObject player)
    {
        PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();

        if (equipment != null)
        {
            equipment.EquipWeapon(this);
        }
    }

    public void UseSkill(GameObject player)
    {
        skill?.UseSkill(player);
    }
}
