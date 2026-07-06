using Catalog.Api.Domain;

namespace Catalog.Api.Application;

/// <summary>Casos de uso do catalogo. Depende apenas do port IProductRepository.</summary>
public sealed class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository) => _repository = repository;

    public Product Create(string name, long priceCents)
    {
        var product = Product.Create(name, priceCents);
        _repository.Save(product);
        return product;
    }

    public IReadOnlyList<Product> List() => _repository.FindAll();

    public Product Get(string id) =>
        _repository.FindById(id) ?? throw new ProductNotFoundException(id);

    public Product ChangePrice(string id, long priceCents)
    {
        var product = Get(id);
        product.ChangePrice(priceCents);
        _repository.Save(product);
        return product;
    }

    public void Delete(string id)
    {
        if (!_repository.DeleteById(id))
        {
            throw new ProductNotFoundException(id);
        }
    }
}
