using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.Api.Tests;

public class ProductEndpointsTests
{
    private static async Task<string> CreateAsync(HttpClient client, string name, long priceCents)
    {
        var response = await client.PostAsJsonAsync("/products", new { name, priceCents });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        return body.GetProperty("id").GetString()!;
    }

    [Fact]
    public async Task Create_and_get()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var id = await CreateAsync(client, "Teclado", 12990);

        var response = await client.GetAsync($"/products/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Teclado", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task List_products()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        await CreateAsync(client, "A", 100);
        await CreateAsync(client, "B", 200);

        var response = await client.GetAsync("/products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_invalid_returns_400()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/products", new { name = "", priceCents = 100 });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_missing_returns_404()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/products/nao-existe");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_price()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var id = await CreateAsync(client, "Mouse", 5000);
        var response = await client.PutAsJsonAsync($"/products/{id}/price", new { priceCents = 9000 });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(9000, body.GetProperty("priceCents").GetInt64());
    }

    [Fact]
    public async Task Update_price_invalid_returns_400()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var id = await CreateAsync(client, "Mouse", 5000);
        var response = await client.PutAsJsonAsync($"/products/{id}/price", new { priceCents = 0 });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_product()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var id = await CreateAsync(client, "Mouse", 5000);
        Assert.Equal(HttpStatusCode.NoContent, (await client.DeleteAsync($"/products/{id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync($"/products/{id}")).StatusCode);
    }

    [Fact]
    public async Task Delete_missing_returns_404()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.DeleteAsync("/products/nao-existe");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Health_endpoint()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task OpenApi_spec_available()
    {
        using var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
