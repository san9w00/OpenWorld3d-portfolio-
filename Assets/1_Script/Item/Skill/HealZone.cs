using System.Collections;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    public float radius = 5f;
    public float duration = 3f;
    public float healPerTick = 10f;
    public float tickInterval = 1f;

    private GameObject owner;

    public void Init(GameObject owner)
    {
        this.owner = owner;
        StartCoroutine(HealCoroutine());
    }

    private IEnumerator HealCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (owner == null) yield break;

            float distance = Vector3.Distance(transform.position, owner.transform.position);

            if (distance <= radius)
            {
                PlayerStatus status = owner.GetComponent<PlayerStatus>();

                if (status != null)
                {
                    status.Heal(healPerTick);
                    Debug.Log("Èú Àû¿ë!");
                }
            }

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
