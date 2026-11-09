using PolyglotShop.Models;

namespace PolyglotShop.Data;

public interface IProductRepository
{
    Task InsertAsync(Product product);
    Task<List<Product>> GetAllAsync();
}
