using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public enum VFXActionType
{
    Hit,
    Skill,
    Skill_Second,
    Heal,
    LevelUp,
    PlayerHit,
    BossAOE,
}

public enum VFXSwordType
{
    None,
    Origin,
    Fire,
    Ice,
    Dragon,
    PeaceHeal,
}

[CreateAssetMenu(menuName = "VFX/VFX Database")]
public class VFXDatabase : ScriptableObject
{
    public List<VFXEntry> vfxList;

    private Dictionary<(VFXActionType, VFXSwordType), GameObject> vfxDict;

    public void Init()
    {
        vfxDict = new Dictionary<(VFXActionType, VFXSwordType), GameObject>();

        if (vfxList == null)
        {
            Debug.LogError("vfxList 없음");
            return;
        }

        foreach (var vfx in vfxList)
        {
            var key = (vfx.actionType, vfx.swordType);

            if (!vfxDict.ContainsKey(key))
            {
                vfxDict.Add(key, vfx.prefab);
            }
        }
    }

    public GameObject GetPrefab(VFXActionType action, VFXSwordType sword)
    {
        if (vfxDict == null) Init();

        if (vfxDict.TryGetValue((action, sword), out var prefab))
            return prefab;

        // 속성없는 기본
        if (vfxDict.TryGetValue((action, VFXSwordType.None), out prefab))
            return prefab;

        Debug.LogWarning($"VFX 없음: {action}, {sword}");
        return null;
    }
}

[System.Serializable]
public class VFXEntry
{
    public VFXActionType actionType;
    public VFXSwordType swordType;

    public GameObject prefab;
    public int poolSize = 5;
}