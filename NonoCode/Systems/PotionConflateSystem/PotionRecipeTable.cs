namespace Nono.NonoCode.PotionConflateSystem;

public static partial class PotionRecipeTable
{
    private static List<PotionRecipe> _recipes;

    public static List<PotionRecipe> Recipes
    {
        get
        {
            if (_recipes == null)
            {
                _recipes = new List<PotionRecipe>();
                LoadAllRecipes();
            }
            return _recipes;
        }
    }

    private static void LoadAllRecipes()
    {
        // 按优先级顺序加载
        // 先加载特殊配方的（索引0）

        // 再加载高级的（追加到末尾）
        LoadHighLevelRecipes();
        // 再加载中级的（追加到末尾）
        LoadMidLevelRecipes();
        // 最后加载低级的（追加到末尾）
        LoadLowLevelRecipes();
    }
    static partial void LoadHighLevelRecipes();
    static partial void LoadMidLevelRecipes();
    static partial void LoadLowLevelRecipes();


}
