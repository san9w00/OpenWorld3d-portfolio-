using System;
using UnityEngine;

public struct DamageEventData
{
    public Vector3 hitPosition;
    public GameObject hitEffectPrefab;

    public DamageEventData(Vector3 position, GameObject effect)
    {
        hitPosition = position;
        hitEffectPrefab = effect;
    }
}

public static class GameEvents
{
    public static Action<DamageEventData> OnUnitDamaged;
}
