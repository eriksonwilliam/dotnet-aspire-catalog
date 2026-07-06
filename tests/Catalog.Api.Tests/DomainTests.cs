using Catalog.Api.Domain;

namespace Catalog.Api.Tests;

public class DomainTests
{
    [Fact]
    public void Create_valid_product()
    {
        var product = Product.Create("  Teclado  ", 12990);
        Assert.False(string.IsNullOrWhiteSpace(product.Id));
        Assert.Equal("Teclado", product.Name);
        Assert.Equal(12990, product.PriceCents);
    }

    [Fact]
    public void Create_rejects_blank_name()
    {
        Assert.Throws<InvalidProductNameException>(() => Product.Create("   ", 100));
    }

    [Fact]
    public void Create_rejects_non_positive_price()
    {
        Assert.Throws<InvalidPriceException>(() => Product.Create("Mouse", 0));
        Assert.Throws<InvalidPriceException>(() => Product.Create("Mouse", -5));
    }

    [Fact]
    public void ChangePrice_validates()
    {
        var product = Product.Create("Mouse", 5000);
        product.ChangePrice(7000);
        Assert.Equal(7000, product.PriceCents);
        Assert.Throws<InvalidPriceException>(() => product.ChangePrice(0));
    }

    [Fact]
    public void Restore_rebuilds_without_validation()
    {
        var product = Product.Restore("id-1", "Persistido", 999);
        Assert.Equal("id-1", product.Id);
        Assert.Equal("Persistido", product.Name);
        Assert.Equal(999, product.PriceCents);
    }
}
