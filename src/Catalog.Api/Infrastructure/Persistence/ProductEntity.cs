namespace Catalog.Api.Infrastructure.Persistence;

/// <summary>Registro de persistencia (EF Core), separado do aggregate de dominio.</summary>
public sealed class ProductEntity
{
    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public long PriceCents { get; set; }
}
