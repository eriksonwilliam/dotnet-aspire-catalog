using System.Text.Json;
using Catalog.Api.Domain;
using Microsoft.AspNetCore.Diagnostics;

namespace Catalog.Api.Web;

/// <summary>Traduz erros de dominio em respostas HTTP consistentes.</summary>
public sealed class DomainExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainException domain)
        {
            return false;
        }

        httpContext.Response.StatusCode = domain.StatusCode;
        httpContext.Response.ContentType = "application/json";
        var payload = JsonSerializer.Serialize(
            new { error = ReasonPhrase(domain.StatusCode), message = domain.Message });
        await httpContext.Response.WriteAsync(payload, cancellationToken);
        return true;
    }

    private static string ReasonPhrase(int statusCode) =>
        statusCode == 404 ? "Not Found" : "Bad Request";
}
