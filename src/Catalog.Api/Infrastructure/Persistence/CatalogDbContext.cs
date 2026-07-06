using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
}
