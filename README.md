# dotnet-aspire-catalog

Catálogo de produtos em **.NET 9** com **Minimal APIs**, **EF Core** e
orquestração via **.NET Aspire**. Fecha a série de serviços da mesma API
(Spring, Quarkus, Ktor e agora .NET), com arquitetura em camadas e **cobertura
de testes 100%**.

O .NET Aspire é o stack da Microsoft para apps distribuídos: um projeto
*AppHost* orquestra os serviços e um projeto *ServiceDefaults* injeta
telemetria (OpenTelemetry), health checks, service discovery e resiliência —
tudo pronto para observabilidade e nuvem.

## Destaques

- **Minimal APIs** — endpoints enxutos, sem controllers.
- **EF Core** — persistência com `DbContext` (provider InMemory por padrão),
  atrás de um port de repositório. Trocar por PostgreSQL/SQL Server não toca o
  domínio.
- **.NET Aspire** — AppHost + ServiceDefaults (OpenTelemetry, health, service
  discovery, resiliência de HttpClient).
- **Cobertura 100%** — linha **e** branch (coverlet), incluindo testes de
  integração com `WebApplicationFactory`.
- **OpenAPI/Swagger** — documentação interativa em `/docs`.

## Stack

- **Linguagem:** C# / .NET 9
- **API:** ASP.NET Core Minimal APIs
- **Persistência:** EF Core (InMemory)
- **Orquestração:** .NET Aspire 9 (AppHost + ServiceDefaults)
- **Docs:** Swashbuckle (Swagger UI em `/docs`)
- **Testes:** xUnit + WebApplicationFactory, cobertura 100% (coverlet)

## Arquitetura

```
src/
├── Catalog.Api/                # Minimal API
│   ├── Domain/                 # Product, exceções — C# puro
│   ├── Application/            # ProductService + IProductRepository (port)
│   ├── Infrastructure/         # EF Core: DbContext, entidade, repositório
│   └── Web/                    # Endpoints, DTOs, handler de exceção
├── Catalog.ServiceDefaults/    # Aspire: telemetria, health, resiliência
└── Catalog.AppHost/            # Aspire: orquestração dos serviços
tests/
└── Catalog.Api.Tests/          # xUnit (unit + integração), gate de cobertura
```

## Como rodar

### API isolada

```bash
dotnet run --project src/Catalog.Api
# Swagger UI em /docs
```

### Com Aspire (orquestração + dashboard)

```bash
dotnet run --project src/Catalog.AppHost
# abre o dashboard do Aspire (requer um runtime de contêiner, ex.: Docker)
```

### Docker (só a API)

```bash
docker compose up --build
```

## Endpoints

| Método | Rota                    | Descrição                      |
| ------ | ----------------------- | ------------------------------ |
| POST   | `/products`             | Cria um produto                |
| GET    | `/products`             | Lista os produtos              |
| GET    | `/products/{id}`        | Busca um produto               |
| PUT    | `/products/{id}/price`  | Altera o preço (em centavos)   |
| DELETE | `/products/{id}`        | Remove um produto              |
| GET    | `/docs`                 | Swagger UI                     |
| GET    | `/health`               | Health check                   |

## Testes e cobertura

```bash
dotnet test
```

A cobertura é exigida em **100%** (linha e branch) sobre o assembly
`Catalog.Api`; o bootstrap (`Program`) e os DTOs ficam fora da métrica. O gate
está configurado no `.csproj` de testes (coverlet) e falha abaixo de 100%.

## Licença

MIT.
