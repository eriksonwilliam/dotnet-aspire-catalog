namespace Catalog.Api.Domain;

/// <summary>Aggregate root. Invariantes: nome nao vazio e preco (centavos) positivo.</summary>
public sealed class Product
{
    private Product(string id, string name, long priceCents)
    {
        Id = id;
        Name = name;
        PriceCents = priceCents;
    }

    public string Id { get; }

    public string Name { get; private set; }

    public long PriceCents { get; private set; }

    public static Product Create(string name, long priceCents)
    {
        var normalized = NormalizeName(name);
        ValidatePrice(priceCents);
        return new Product(Guid.NewGuid().ToString(), normalized, priceCents);
    }

    /// <summary>Reconstroi a partir de dados persistidos (sem revalidar).</summary>
    public static Product Restore(string id, string name, long priceCents) =>
        new(id, name, priceCents);

    public void ChangePrice(long priceCents)
    {
        ValidatePrice(priceCents);
        PriceCents = priceCents;
    }

    private static string NormalizeName(string name)
    {
        var normalized = name.Trim();
        if (normalized.Length == 0)
        {
            throw new InvalidProductNameException();
        }

        return normalized;
    }

    private static void ValidatePrice(long priceCents)
    {
        if (priceCents <= 0)
        {
            throw new InvalidPriceException();
        }
    }
}
