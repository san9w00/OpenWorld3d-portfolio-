using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponItemSO : ItemSO
{
    public float damage;
    public GameObject hitEffectVFX;

    public override void Use(GameObject player)
    {
        PlayerEquipment equipment = player.GetComponent<PlayerEquipment>();

        if (equipment != null)
        {
            equipment.EquipWeapon(this);
        }
    }
}
