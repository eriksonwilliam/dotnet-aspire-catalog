using Catalog.Api.Domain;
using Catalog.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Tests;

public class EfProductRepositoryTests
{
    private static CatalogDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new CatalogDbContext(options);
    }

    [Fact]
    public void Save_and_find()
    {
        var repo = new EfProductRepository(NewContext());
        var product = Product.Create("Teclado", 12990);
        repo.Save(product);
        Assert.Equal(product.Id, repo.FindById(product.Id)!.Id);
    }

    [Fact]
    public void Save_updates_existing()
    {
        var repo = new EfProductRepository(NewContext());
        var product = Product.Create("Teclado", 12990);
        repo.Save(product);
        product.ChangePrice(15000);
        repo.Save(product);
        Assert.Equal(15000, repo.FindById(product.Id)!.PriceCents);
    }

    [Fact]
    public void FindById_missing_returns_null()
    {
        var repo = new EfProductRepository(NewContext());
        Assert.Null(repo.FindById("nao-existe"));
    }

    [Fact]
    public void FindAll_returns_all()
    {
        var repo = new EfProductRepository(NewContext());
        repo.Save(Product.Create("A", 100));
        repo.Save(Product.Create("B", 200));
        Assert.Equal(2, repo.FindAll().Count);
    }

    [Fact]
    public void DeleteById_true_then_false()
    {
        var repo = new EfProductRepository(NewContext());
        var product = Product.Create("Mouse", 5000);
        repo.Save(product);
        Assert.True(repo.DeleteById(product.Id));
        Assert.False(repo.DeleteById(product.Id));
    }
}
