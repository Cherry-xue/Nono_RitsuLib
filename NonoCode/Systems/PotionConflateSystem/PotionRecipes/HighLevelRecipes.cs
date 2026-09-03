using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.PotionConflateSystem;

public static partial class PotionRecipeTable
{
    static partial void LoadHighLevelRecipes()
    {
        Recipes.AddRange(new[]
        {
            //2强效魔力药水->超级魔力药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(GreaterManaPotion), 2}}, ModelDb.Potion<SuperManaPotion>() ),
            //2强效治疗药水->超级治疗药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(GreaterHealingPotion), 2}}, ModelDb.Potion<SuperHealingPotion>() ),
        });
    }
}