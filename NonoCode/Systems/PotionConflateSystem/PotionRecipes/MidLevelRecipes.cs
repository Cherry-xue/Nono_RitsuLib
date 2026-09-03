using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.PotionConflateSystem;

public static partial class PotionRecipeTable
{
    static partial void LoadMidLevelRecipes()
    {
        Recipes.AddRange(new[]
        {
            //2魔力药水->强效魔力药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(ManaPotion), 2}}, ModelDb.Potion<GreaterManaPotion>() ),
            //2治疗药水->强效治疗药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(HealingPotion), 2}}, ModelDb.Potion<GreaterHealingPotion>() ),

        });
    }
}