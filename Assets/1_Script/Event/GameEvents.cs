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
    // 데미지 이벤트
    public static Action<DamageEventData> OnUnitDamaged;

    // 적 리셋 이벤트
    public static Action OnReset;

    public static void Reset()
    {
        OnReset?.Invoke();
    }
}
