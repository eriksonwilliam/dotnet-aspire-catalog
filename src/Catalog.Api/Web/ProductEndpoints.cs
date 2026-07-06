using Catalog.Api.Application;

namespace Catalog.Api.Web;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products").WithTags("products");

        group.MapPost("/", (CreateProductRequest request, ProductService service) =>
        {
            var product = service.Create(request.Name, request.PriceCents);
            return Results.Created($"/products/{product.Id}", ProductResponse.From(product));
        });

        group.MapGet("/", (ProductService service) =>
            Results.Ok(service.List().Select(ProductResponse.From)));

        group.MapGet("/{id}", (string id, ProductService service) =>
            Results.Ok(ProductResponse.From(service.Get(id))));

        group.MapPut("/{id}/price", (string id, UpdatePriceRequest request, ProductService service) =>
            Results.Ok(ProductResponse.From(service.ChangePrice(id, request.PriceCents))));

        group.MapDelete("/{id}", (string id, ProductService service) =>
        {
            service.Delete(id);
            return Results.NoContent();
        });
    }
}
