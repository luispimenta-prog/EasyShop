using PolyglotShop.Models;
using SQLite;

namespace PolyglotShop.Data;

/// <summary>Acesso a dados RELACIONAL (SQLite via sqlite-net-pcl). Só faz CRUD, sem regra de negócio.</summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly SQLiteAsyncConnection _db;
    private readonly Task _init;

    public CustomerRepository()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "customers.db3");
        _db = new SQLiteAsyncConnection(path);
        _init = _db.CreateTableAsync<Customer>();
    }

    public async Task<int> InsertAsync(Customer customer)
    {
        await _init;
        return await _db.InsertAsync(customer);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        await _init;
        return await _db.Table<Customer>().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        await _init;
        var e = email.ToLowerInvariant();
        return await _db.Table<Customer>().Where(c => c.Email == e).CountAsync() > 0;
    }
}
