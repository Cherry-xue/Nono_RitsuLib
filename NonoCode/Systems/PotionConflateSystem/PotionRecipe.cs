#nullable enable
using MegaCrit.Sts2.Core.Models;

namespace Nono.NonoCode.PotionConflateSystem;

public class PotionRecipe
{
	public IReadOnlyDictionary<Type, int> Ingredients { get; }

	public PotionModel ResultPotionType { get; }

	public PotionRecipe(Dictionary<Type, int> ingredients, PotionModel resultPotionType)
	{
		Ingredients = ingredients;
		ResultPotionType = resultPotionType;
	}

	public bool CanCraft(IEnumerable<PotionModel?> potionSlots)
	{
		Dictionary<Type, int> dictionary = (from p in potionSlots.OfType<PotionModel>()
			group p by ((object)p).GetType()).ToDictionary((IGrouping<Type, PotionModel> g) => g.Key, (IGrouping<Type, PotionModel> g) => g.Count());
		foreach (KeyValuePair<Type, int> ingredient in Ingredients)
		{
			if (!dictionary.TryGetValue(ingredient.Key, out var value) || value < ingredient.Value)
			{
				return false;
			}
		}
		return true;
	}
}
