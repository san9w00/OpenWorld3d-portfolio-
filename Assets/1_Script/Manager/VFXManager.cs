using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnUnitDamaged += SpawnHitEffect;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitDamaged -= SpawnHitEffect;
    }

    private void SpawnHitEffect(DamageEventData data)
    {
        if (data.hitEffectPrefab == null) return;

        Instantiate(data.hitEffectPrefab, data.hitPosition, Quaternion.identity);
    }
}
