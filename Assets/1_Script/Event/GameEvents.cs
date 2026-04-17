using System;
using UnityEngine;

public struct EnemyKilledEvent
{
    public EnemyType enemyType;

    public EnemyKilledEvent(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }
}

public struct VFXEvent
{
    public Vector3 Position;
    public VFXActionType ActionType;
    public VFXSwordType SwordType;

    public VFXEvent(Vector3 position, VFXActionType action, VFXSwordType sword)
    {
        Position = position;
        ActionType = action;
        SwordType = sword;
    }
}

public struct GoldRewardEvent
{
    public int Amount;
    public Vector3 Position;

    public GoldRewardEvent(int amount, Vector3 position)
    {
        Amount = amount;
        Position = position;
    }
}

public struct ResetEvent
{
    public Vector3 Position;

    public ResetEvent(Vector3 positon)
    {
        Position = positon;
    }
}

public struct CampFireActivatedEvent
{
    public CampFire campFire;

    public CampFireActivatedEvent(CampFire campFire)
    {
        this.campFire = campFire;
    }
}
