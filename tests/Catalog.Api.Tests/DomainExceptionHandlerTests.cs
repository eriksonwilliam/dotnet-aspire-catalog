using Catalog.Api.Domain;
using Catalog.Api.Web;
using Microsoft.AspNetCore.Http;

namespace Catalog.Api.Tests;

public class DomainExceptionHandlerTests
{
    private static DefaultHttpContext NewContext() =>
        new() { Response = { Body = new MemoryStream() } };

    [Fact]
    public async Task Maps_not_found_to_404()
    {
        var context = NewContext();
        var handled = await new DomainExceptionHandler()
            .TryHandleAsync(context, new ProductNotFoundException("x"), CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(404, context.Response.StatusCode);
    }

    [Fact]
    public async Task Maps_validation_to_400()
    {
        var context = NewContext();
        var handled = await new DomainExceptionHandler()
            .TryHandleAsync(context, new InvalidPriceException(), CancellationToken.None);

        Assert.True(handled);
        Assert.Equal(400, context.Response.StatusCode);
    }

    [Fact]
    public async Task Ignores_non_domain_exceptions()
    {
        var context = NewContext();
        var handled = await new DomainExceptionHandler()
            .TryHandleAsync(context, new InvalidOperationException(), CancellationToken.None);

        Assert.False(handled);
    }
}
