using System;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public int Gold { get; private set; } = 500;

    public Action OnGoldChanged;

    public bool TrySpendGold(int amount)
    {
        if (Gold < amount)
            return false;

        Gold -= amount;
        OnGoldChanged?.Invoke();
        return true;
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        OnGoldChanged?.Invoke();
    }
}
