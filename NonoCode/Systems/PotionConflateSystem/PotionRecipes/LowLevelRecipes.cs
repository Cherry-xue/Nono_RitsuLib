using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using Nono.NonoCode.Potions;

namespace Nono.NonoCode.PotionConflateSystem;

public static partial class PotionRecipeTable
{
    static partial void LoadLowLevelRecipes()
    {
        Recipes.AddRange(new[]
        {
            //2弱效魔力药水->魔力药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(LesserManaPotion), 2}}, ModelDb.Potion<ManaPotion>() ),
            //2弱效敏捷药水->敏捷药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(LesserSwiftPotion), 2}}, ModelDb.Potion<SwiftPotion>() ),
            //2弱效治疗药水->治疗药水
            new PotionRecipe(new Dictionary<Type, int> {{typeof(LesserHealingPotion), 2}}, ModelDb.Potion<HealingPotion>() ),
        });
    }
}
