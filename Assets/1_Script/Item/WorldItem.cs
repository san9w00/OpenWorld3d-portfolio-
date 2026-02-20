using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                inventory.AddItem(itemData, amount);
                Destroy(gameObject);
            }
        }
    }
}
