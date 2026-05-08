using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft/Crafting Database")]
public class CraftingDatabaseSO : ScriptableObject
{
    public List<CraftRecipeSO> recipes;
}
