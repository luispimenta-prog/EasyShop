using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PolyglotShop.Models;
using PolyglotShop.Services;

namespace PolyglotShop.ViewModels;

public partial class ProductsViewModel : ObservableObject
{
    private readonly IProductService _service;

    [ObservableProperty] private string name = "";
    [ObservableProperty] private string priceText = "";
    [ObservableProperty] private string category = "";
    [ObservableProperty] private string attributesText = "";
    [ObservableProperty] private string message = "";

    public ObservableCollection<Product> Products { get; } = new();

    public ProductsViewModel(IProductService service) => _service = service;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Products.Clear();
        foreach (var p in await _service.ListAsync()) Products.Add(p);
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var result = await _service.RegisterAsync(Name, PriceText, Category, AttributesText);
        Message = result.Message;
        if (!result.Success) return;

        Name = PriceText = Category = AttributesText = "";
        await LoadAsync();
    }
}
