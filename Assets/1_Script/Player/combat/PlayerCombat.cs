using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private SwordHitbox currentHitbox;

    public void SetWeapon(SwordHitbox newHitbox)
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
