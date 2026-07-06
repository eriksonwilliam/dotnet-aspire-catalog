using Catalog.Api.Domain;

namespace Catalog.Api.Application;

/// <summary>Port de saida para persistencia de produtos.</summary>
public interface IProductRepository
{
    void Save(Product product);

    Product? FindById(string id);

    IReadOnlyList<Product> FindAll();

    bool DeleteById(string id);
}
