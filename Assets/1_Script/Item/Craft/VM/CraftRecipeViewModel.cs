using UnityEngine;

public class CraftRecipeViewModel 
{
    public CraftRecipeSO Recipe { get; private set; }

    public Sprite Icon => Recipe.resultItem.itemIcon;

    public string ItemName => Recipe.resultItem.itemName;

    public string Description => Recipe.description;

    public bool CanCraft =>
        CraftingSystem.Instance.CanCraft(Recipe);

    public CraftRecipeViewModel(CraftRecipeSO recipe)
    {
        Recipe = recipe;
    }
}
