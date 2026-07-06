namespace Catalog.Api.Domain;

/// <summary>Erro base do dominio. Cada erro carrega o status HTTP correspondente.</summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }

    public abstract int StatusCode { get; }
}

public sealed class ProductNotFoundException : DomainException
{
    public ProductNotFoundException(string id) : base($"Produto nao encontrado: {id}")
    {
    }

    public override int StatusCode => 404;
}

public abstract class ProductValidationException : DomainException
{
    protected ProductValidationException(string message) : base(message)
    {
    }

    public override int StatusCode => 400;
}

public sealed class InvalidProductNameException : ProductValidationException
{
    public InvalidProductNameException() : base("O nome do produto nao pode ser vazio.")
    {
    }
}

public sealed class InvalidPriceException : ProductValidationException
{
    public InvalidPriceException() : base("O preco (em centavos) deve ser maior que zero.")
    {
    }
}
