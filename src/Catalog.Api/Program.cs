using System.Diagnostics.CodeAnalysis;
using Catalog.Api.Application;
using Catalog.Api.Infrastructure.Persistence;
using Catalog.Api.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseInMemoryDatabase("catalog"));
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API");
    options.RoutePrefix = "docs";
});

app.MapProductEndpoints();

app.Run();

/// <summary>Exposto para os testes de integracao; excluido da cobertura (bootstrap).</summary>
[ExcludeFromCodeCoverage]
public partial class Program
{
}
