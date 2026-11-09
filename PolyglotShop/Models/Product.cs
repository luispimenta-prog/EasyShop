using LiteDB;

namespace PolyglotShop.Models;

/// <summary>Documento BSON da coleção "products" no LiteDB (atributos sem schema fixo).</summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Category { get; set; }

    /// <summary>Atributos dinâmicos: cada produto pode ter chaves diferentes (cor, voltagem, RAM...).</summary>
    public Dictionary<string, string> Attributes { get; set; } = new();

    [BsonIgnore]
    public string AttributesDisplay =>
        Attributes.Count == 0 ? "Sem atributos" : string.Join(" • ", Attributes.Select(a => $"{a.Key}: {a.Value}"));

    [BsonIgnore]
    public string PriceDisplay => Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

    [BsonIgnore]
    public string Header => string.IsNullOrWhiteSpace(Category) ? Name : $"{Name}  [{Category}]";
}
