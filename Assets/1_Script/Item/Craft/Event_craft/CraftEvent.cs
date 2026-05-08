// ------- Craft 구조체 ----------


// UI 선택
public struct CraftRecipeSelectedEvent
{
    public CraftRecipeViewModel ViewModel;

    public CraftRecipeSelectedEvent(CraftRecipeViewModel vm)
    {
        ViewModel = vm;
    }
}

// 제작 요청
public struct CraftRequestEvent
{
    public CraftRecipeSO Recipe;

    public CraftRequestEvent(CraftRecipeSO recipe)
    {
        Recipe = recipe;
    }
}
