using System.Diagnostics.CodeAnalysis;
using Catalog.Api.Domain;

namespace Catalog.Api.Web;

[ExcludeFromCodeCoverage]
public sealed record CreateProductRequest(string Name, long PriceCents);

[ExcludeFromCodeCoverage]
public sealed record UpdatePriceRequest(long PriceCents);

[ExcludeFromCodeCoverage]
public sealed record ProductResponse(string Id, string Name, long PriceCents)
{
    public static ProductResponse From(Product product) =>
        new(product.Id, product.Name, product.PriceCents);
}
