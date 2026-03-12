using UnityEngine;

public class BladeDamage : MonoBehaviour
{
    [SerializeField] private float damage = 50f;

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if(damageable != null )
        {
            damageable.TakeDamage(damage);
        }
    }
}
