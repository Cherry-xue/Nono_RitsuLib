#nullable enable
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace Nono.NonoCode.PotionConflateSystem;

public static class PotionConflateService
{
	public static PotionRecipe? FindFirstCraftableRecipe(IEnumerable<PotionModel?> potionSlots)
	{
		return PotionRecipeTable.Recipes.FirstOrDefault((PotionRecipe r) => r.CanCraft(potionSlots));
	}

	public static async Task<bool> TryCraft(Player owner, IEnumerable<PotionModel?> potionSlots, PotionRecipe? recipe)
	{
		if (owner == null || potionSlots == null || recipe == null)
		{
			return false;
		}
		List<PotionModel> potions = potionSlots.OfType<PotionModel>().ToList();
		foreach (KeyValuePair<Type, int> ingredient in recipe.Ingredients)
		{
			for (int i = 0; i < ingredient.Value; i++)
			{
				PotionModel potion = potions.FirstOrDefault(p => p.GetType() == ingredient.Key);
				if (potion == null)
				{
					return false;
				}
				potions.Remove(potion);
				await PotionCmd.Discard(potion);
			}
		}
		await PotionCmd.TryToProcure(recipe.ResultPotionType.ToMutable(), owner, -1);
		return true;
	}
}
