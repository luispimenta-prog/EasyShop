using PolyglotShop.Models;

namespace PolyglotShop.Services;

public interface ICustomerService
{
    Task<ServiceResult> RegisterAsync(string? name, string? email);
    Task<List<Customer>> ListAsync();
}
