using System.Collections;
using UnityEngine;

public class BossBreathDamage : MonoBehaviour
{
    [SerializeField] private float damage = 4f;
    [SerializeField] private float tickRate = 0.2f;

    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            timer = 0f;

            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnDisable()
    {
        timer = 0f;
    }
}
