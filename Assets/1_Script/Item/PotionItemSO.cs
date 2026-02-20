using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion")]
public class PotionItemSO : ItemSO
{
    public int healAmount = 30;

    public override void Use(GameObject player)
    {
        PlayerStatus status = player.GetComponent<PlayerStatus>();

        if (status != null )
        {
            status.Heal(healAmount);
        }
    }
}
