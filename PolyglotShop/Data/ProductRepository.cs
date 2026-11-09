using LiteDB;
using PolyglotShop.Models;

namespace PolyglotShop.Data;

/// <summary>Acesso a dados NoSQL de DOCUMENTOS (LiteDB). Só faz CRUD, sem regra de negócio.</summary>
public class ProductRepository : IProductRepository, IDisposable
{
    private readonly LiteDatabase _db;
    private readonly ILiteCollection<Product> _products;

    public ProductRepository()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "products.db");
        _db = new LiteDatabase($"Filename={path};Connection=shared");
        _products = _db.GetCollection<Product>("products");
        _products.EnsureIndex(p => p.Name);
    }

    public Task InsertAsync(Product product) => Task.Run(() => { _products.Insert(product); });

    public Task<List<Product>> GetAllAsync() =>
        Task.Run(() => _products.FindAll().OrderBy(p => p.Name).ToList());

    public void Dispose() => _db.Dispose();
}
