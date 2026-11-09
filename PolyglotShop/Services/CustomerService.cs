using System.Text.RegularExpressions;
using PolyglotShop.Data;
using PolyglotShop.Models;

namespace PolyglotShop.Services;

/// <summary>Regras de negócio do cliente: validação estrita (nome, e-mail válido e único).</summary>
public class CustomerService : ICustomerService
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly ICustomerRepository _repo;
    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public async Task<ServiceResult> RegisterAsync(string? name, string? email)
    {
        name = name?.Trim();
        email = email?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name) || name.Length < 3)
            return ServiceResult.Fail("Informe o nome completo (mínimo 3 caracteres).");
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex.IsMatch(email))
            return ServiceResult.Fail("Informe um e-mail válido.");
        if (await _repo.EmailExistsAsync(email))
            return ServiceResult.Fail("Já existe um cliente com este e-mail.");

        await _repo.InsertAsync(new Customer { Name = name, Email = email });
        return ServiceResult.Ok("Cliente cadastrado com sucesso!");
    }

    public Task<List<Customer>> ListAsync() => _repo.GetAllAsync();
}
