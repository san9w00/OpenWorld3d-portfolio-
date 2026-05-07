using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Craft/Craft Recipe")]
public class CraftRecipeSO : ScriptableObject
{
    [Header("Result")]
    public ItemSO resultItem;

    [Header("Need Resources")]
    public List<RequiredResource> requiredResources;

    [TextArea]
    public string description;
}
