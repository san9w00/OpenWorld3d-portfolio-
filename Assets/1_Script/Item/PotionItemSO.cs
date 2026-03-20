using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion")]
public class PotionItemSO : ItemSO
{
    public int healAmount = 30;
    public GameObject potionVFX;

    public override void Use(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();

        if (status != null )
        {
            status.Heal(healAmount);

            if (potionVFX != null)
            {
                EventBus.Publish(new VFXEvent(
                    status.transform.position + Vector3.up * 1.5f,
                    potionVFX
                ));
            }
        }
    }
}
