using eDrink.Core.Models;

namespace eDrink.UseCases.Calculation
{
	public interface ICalculation
	{
		event Action OnChange;
		Task AddToMix(MixItem item);
		Task<List<MixItem>> GetMixItems();
		Task DeleteItem(MixItem item);
		Task EmptyMix();
	}
}
