using PolyglotShop.Data;
using PolyglotShop.Services;
using PolyglotShop.ViewModels;
using PolyglotShop.Views;

namespace PolyglotShop;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        // Camada de Dados (um repositório por paradigma de banco)
        builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>(); // SQL  (SQLite)
        builder.Services.AddSingleton<IProductRepository, ProductRepository>();   // NoSQL (LiteDB)

        // Camada de Negócio
        builder.Services.AddSingleton<ICustomerService, CustomerService>();
        builder.Services.AddSingleton<IProductService, ProductService>();

        // Camada de Apresentação
        builder.Services.AddTransient<ProductsViewModel>();
        builder.Services.AddTransient<CustomersViewModel>();
        builder.Services.AddTransient<ProductsPage>();
        builder.Services.AddTransient<CustomersPage>();

        return builder.Build();
    }
}
