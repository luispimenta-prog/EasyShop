using System.Globalization;
using PolyglotShop.Data;
using PolyglotShop.Models;

namespace PolyglotShop.Services;

/// <summary>
/// Regras de negócio do produto: valida nome/preço e converte o texto
/// "Cor=Azul; Tamanho=M" em atributos dinâmicos (documento flexível).
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<ServiceResult> RegisterAsync(string? name, string? priceText, string? category, string? attributesText)
    {
        name = name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return ServiceResult.Fail("Informe o nome do produto.");

        if (!TryParsePrice(priceText, out var price) || price <= 0)
            return ServiceResult.Fail("Informe um preço válido maior que zero (ex.: 199,90).");

        if (!TryParseAttributes(attributesText, out var attrs, out var error))
            return ServiceResult.Fail(error);

        await _repo.InsertAsync(new Product
        {
            Name = name,
            Price = price,
            Category = string.IsNullOrWhiteSpace(category) ? null : category.Trim(),
            Attributes = attrs
        });
        return ServiceResult.Ok("Produto cadastrado com sucesso!");
    }

    public Task<List<Product>> ListAsync() => _repo.GetAllAsync();

    private static bool TryParsePrice(string? text, out decimal price)
    {
        price = 0;
        if (string.IsNullOrWhiteSpace(text)) return false;
        var t = text.Trim().Replace("R$", "").Replace(" ", "");
        // aceita "1.299,90" (pt-BR) e "1299.90"
        if (t.Contains(',')) t = t.Replace(".", "").Replace(',', '.');
        return decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out price);
    }

    private static bool TryParseAttributes(string? text, out Dictionary<string, string> attrs, out string error)
    {
        attrs = new(StringComparer.OrdinalIgnoreCase);
        error = string.Empty;
        if (string.IsNullOrWhiteSpace(text)) return true;

        foreach (var part in text.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var kv = part.Split('=', 2, StringSplitOptions.TrimEntries);
            if (kv.Length != 2 || kv[0].Length == 0 || kv[1].Length == 0)
            {
                error = $"Atributo inválido: \"{part}\". Use o formato chave=valor; chave=valor.";
                return false;
            }
            attrs[kv[0]] = kv[1];
        }
        return true;
    }
}
