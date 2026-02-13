using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private SwordHitbox hitbox;

    public void OnHitbox()
    {
        hitbox.EnableHitbox();
    }

    public void OffHitbox()
    {
        hitbox.DisableHitbox();
    }
}
