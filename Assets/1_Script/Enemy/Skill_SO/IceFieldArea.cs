using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IceFieldArea : MonoBehaviour
{
    [SerializeField] private float damage = 4f;
    [SerializeField] private float tickInterval = 0.2f;
    [SerializeField] private float duration = 6f;

    private Coroutine damageRoutine;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerStatus player = other.GetComponent<PlayerStatus>();

        if (player == null)
            return;

        damageRoutine = StartCoroutine(DamageRoutine(player));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }
    }

    private IEnumerator DamageRoutine(PlayerStatus player)
    {
        while (true)
        {
            player.TakeDamage(damage);

            yield return new WaitForSeconds(tickInterval);
        }
    }
}
