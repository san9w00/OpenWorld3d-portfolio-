using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private WeaponHitbox currentHitbox;

    public void SetWeapon(WeaponHitbox newHitbox)
    {
        currentHitbox = newHitbox;
    }

    public void OnHitbox()
    {
        if (currentHitbox != null)
            currentHitbox.EnableHitbox();
    }

    public void OffHitbox()
    {
        if (currentHitbox != null)
            currentHitbox.DisableHitbox();
    }
}
