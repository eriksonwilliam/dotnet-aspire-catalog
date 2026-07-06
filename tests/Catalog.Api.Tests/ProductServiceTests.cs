using Catalog.Api.Application;
using Catalog.Api.Domain;
using Catalog.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Tests;

public class ProductServiceTests
{
    private static ProductService NewService()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ProductService(new EfProductRepository(new CatalogDbContext(options)));
    }

    [Fact]
    public void Create_and_get()
    {
        var service = NewService();
        var created = service.Create("Teclado", 12990);
        Assert.Equal(created.Id, service.Get(created.Id).Id);
    }

    [Fact]
    public void List_returns_all()
    {
        var service = NewService();
        service.Create("A", 100);
        service.Create("B", 200);
        Assert.Equal(2, service.List().Count);
    }

    [Fact]
    public void Get_missing_throws()
    {
        var service = NewService();
        Assert.Throws<ProductNotFoundException>(() => service.Get("nao-existe"));
    }

    [Fact]
    public void ChangePrice_updates()
    {
        var service = NewService();
        var created = service.Create("Mouse", 5000);
        Assert.Equal(9000, service.ChangePrice(created.Id, 9000).PriceCents);
    }

    [Fact]
    public void Delete_removes()
    {
        var service = NewService();
        var created = service.Create("Mouse", 5000);
        service.Delete(created.Id);
        Assert.Throws<ProductNotFoundException>(() => service.Get(created.Id));
    }

    [Fact]
    public void Delete_missing_throws()
    {
        var service = NewService();
        Assert.Throws<ProductNotFoundException>(() => service.Delete("nao-existe"));
    }
}
