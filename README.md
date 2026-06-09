# PortalHexagonalApi

API em ASP.NET Core organizada em camadas seguindo a ideia de arquitetura hexagonal, com `Domain`, `Application`, `Adapters` e `Api`.

## Dependencias

- .NET SDK 10.0 ou superior
- Git
- Um terminal com acesso ao comando `dotnet`

Para conferir a versao instalada:

```bash
dotnet --version
```

O projeto usa `net10.0`, entao versoes anteriores do SDK nao devem compilar a solucao.

## Estrutura

```text
PortalHexagonalApi/
  Portal.Api/          # Entrada HTTP e configuracao de DI
  Portal.Application/  # Casos de uso e portas
  Portal.Domain/       # Entidades de dominio
  Portal.Adapters/     # Implementacoes externas/fakes das portas
```

## Como restaurar e compilar

Na raiz do repositorio:

```bash
dotnet restore PortalHexagonalApi/PortalHexagonalApi.slnx
dotnet build PortalHexagonalApi/PortalHexagonalApi.slnx
```

## Como rodar a API

Execute o projeto `Portal.Api`:

```bash
dotnet run --project Portal.Api
```

Em ambiente de desenvolvimento, a API sobe em:

```text
http://localhost:5125
```

A documentacao OpenAPI fica disponivel em:

```text
http://localhost:5125/openapi/v1.json
```

## Endpoints disponiveis

```http
GET http://localhost:5125/api/quotes
GET http://localhost:5125/api/quotes/{id}
GET http://localhost:5125/api/lab-results
GET http://localhost:5125/api/lab-results/{id}
```

Exemplos:

```bash
curl http://localhost:5125/api/quotes
curl http://localhost:5125/api/quotes/quote-1
curl http://localhost:5125/api/lab-results
curl http://localhost:5125/api/lab-results/result-1
```
