using PolyglotShop.Models;

namespace PolyglotShop.Services;

public interface IProductService
{
    Task<ServiceResult> RegisterAsync(string? name, string? priceText, string? category, string? attributesText);
    Task<List<Product>> ListAsync();
}
