using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PolyglotShop.Models;
using PolyglotShop.Services;

namespace PolyglotShop.ViewModels;

public partial class CustomersViewModel : ObservableObject
{
    private readonly ICustomerService _service;

    [ObservableProperty] private string name = "";
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string message = "";

    public ObservableCollection<Customer> Customers { get; } = new();

    public CustomersViewModel(ICustomerService service) => _service = service;

    [RelayCommand]
    public async Task LoadAsync()
    {
        Customers.Clear();
        foreach (var c in await _service.ListAsync()) Customers.Add(c);
    }

    [RelayCommand]
    private async Task AddAsync()
    {
        var result = await _service.RegisterAsync(Name, Email);
        Message = result.Message;
        if (!result.Success) return;

        Name = Email = "";
        await LoadAsync();
    }
}
