using PolyglotShop.Models;

namespace PolyglotShop.Data;

public interface ICustomerRepository
{
    Task<int> InsertAsync(Customer customer);
    Task<List<Customer>> GetAllAsync();
    Task<bool> EmailExistsAsync(string email);
}
