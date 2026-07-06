using Catalog.Api.Application;
using Catalog.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Infrastructure.Persistence;

/// <summary>Adapter de persistencia com EF Core.</summary>
public sealed class EfProductRepository : IProductRepository
{
    private readonly CatalogDbContext _db;

    public EfProductRepository(CatalogDbContext db) => _db = db;

    public void Save(Product product)
    {
        var existing = _db.Products.FirstOrDefault(p => p.Id == product.Id);
        if (existing is null)
        {
            _db.Products.Add(ToEntity(product));
        }
        else
        {
            existing.Name = product.Name;
            existing.PriceCents = product.PriceCents;
        }

        _db.SaveChanges();
    }

    public Product? FindById(string id)
    {
        var entity = _db.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
        return entity is null ? null : ToDomain(entity);
    }

    public IReadOnlyList<Product> FindAll() =>
        _db.Products.AsNoTracking().ToList().Select(ToDomain).ToList();

    public bool DeleteById(string id)
    {
        var entity = _db.Products.FirstOrDefault(p => p.Id == id);
        if (entity is null)
        {
            return false;
        }

        _db.Products.Remove(entity);
        _db.SaveChanges();
        return true;
    }

    private static ProductEntity ToEntity(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        PriceCents = product.PriceCents,
    };

    private static Product ToDomain(ProductEntity entity) =>
        Product.Restore(entity.Id, entity.Name, entity.PriceCents);
}
