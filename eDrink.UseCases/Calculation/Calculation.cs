using Blazored.LocalStorage;
using Blazored.Toast.Services;
using eDrink.Core.Models;

namespace eDrink.UseCases.Calculation
{
	public class Calculation : ICalculation
	{
		public event Action? OnChange;
		private readonly ILocalStorageService _localStorage;
		private readonly IToastService _toastService;

		public Calculation(ILocalStorageService localStorage, IToastService toastService)
		{
			_localStorage = localStorage;
			_toastService = toastService;
		}

		public async Task AddToMix(MixItem item)
		{
			var mix = await _localStorage.GetItemAsync<List<MixItem>>("mix") ?? new List<MixItem>();

			var sameItem = mix.Find(x => x.ProductId == item.ProductId);
			if (sameItem == null)
			{
				mix.Add(item);
			}
			else
			{
				sameItem.Quantity += item.Quantity;
			}

			await _localStorage.SetItemAsync("mix", mix);
			_toastService.ShowSuccess(item.Name, "Added to mix:");

			OnChange?.Invoke();
		}

		public async Task<List<MixItem>> GetMixItems()
		{
			var result = new List<MixItem>();
			var mix = await _localStorage.GetItemAsync<List<MixItem>>("mix");
			if (mix == null)
			{
				return result;
			}

			result.AddRange(mix);

			return result;
		}

		public async Task DeleteItem(MixItem item)
		{
			var mix = await _localStorage.GetItemAsync<List<MixItem>>("mix");
			if (mix == null)
			{
				return;
			}

			var mixItem = mix.Find(x => x.ProductId == item.ProductId);
			mix.Remove(mixItem);

			await _localStorage.SetItemAsync("mix", mix);
			OnChange.Invoke();
		}

		public async Task EmptyMix()
		{
			await _localStorage.RemoveItemAsync("mix");
			OnChange.Invoke();
		}
	}
}
