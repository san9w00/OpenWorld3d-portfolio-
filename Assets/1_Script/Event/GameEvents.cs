using System;
using UnityEngine;

// 플레이어 피격 이벤트
public struct PlayerHitEvent
{
    public float Intensity;

    public PlayerHitEvent(float intensity)
    {
        Intensity = intensity;
    }
}

// --- 전투 및 효과 관련 이벤트 ---
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

public struct EnemyKilledEvent
{
    public EnemyType enemyType;

    public EnemyKilledEvent(EnemyType enemyType)
    {
        this.enemyType = enemyType;
    }
}

// --- 경제 및 보상 관련 이벤트 ---
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

// --- 월드 및 체크포인트 관련 이벤트 ---
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

// --- 인벤토리 및 아이템 관련 이벤트 ---
// UI 갱신이나 아이템 흭득 알림 등에 사용
public struct InventoryChangedEvent
{
    public readonly System.Collections.Generic.List<InventoryItem> Items;
    public InventoryChangedEvent(System.Collections.Generic.List<InventoryItem> items) => Items = items;
}

public struct ItemAddedEvent
{
    public readonly ItemSO Item;
    public readonly int Amount;
    public ItemAddedEvent(ItemSO item, int amount) { Item = item; Amount = amount; }
}

public struct ItemUseRequestedEvent
{
    public readonly InventoryItem Item;
    public ItemUseRequestedEvent(InventoryItem item) { Item = item; }
}

// --- 시스템 및 UI 관련 이벤트 ---
public struct QuickSlotChangedEvent
{
    public readonly InventoryItem CurrentItem;
    public QuickSlotChangedEvent(InventoryItem item) => CurrentItem = item;
}
