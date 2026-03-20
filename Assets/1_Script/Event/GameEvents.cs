using System;
using UnityEngine;

public struct VFXEvent
{
    public Vector3 Position;
    public GameObject VFXPrefab;

    public VFXEvent(Vector3 position, GameObject vfxPrefab)
    {
        Position = position;
        VFXPrefab = vfxPrefab;
    }
}

public struct LevelUpEvent
{
    public Vector3 Position;

    public LevelUpEvent(Vector3 position)
    {
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
