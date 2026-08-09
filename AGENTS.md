# Repository Guidelines

## Project Structure & Module Organization
OpenAdm is a multi-project .NET solution (`OpenAdm.sln`) organized by layer. `OpenAdm.Api/` contains the ASP.NET Core entry point, controllers, middleware, HTML email templates, Dockerfile, and `.env.example`. `OpenAdm.Application/` holds DTOs, services, interfaces, mappers, queries, and view models. `OpenAdm.Domain/` contains entities, enums, helpers, exceptions, and repository contracts. `OpenAdm.Data/` contains EF Core contexts, entity configurations, and migrations. `OpenAdm.Infra/` and `OpenAdm.IoC/` provide repository, cache, HTTP, Azure, and dependency injection wiring. Worker code lives in `OpenAdm.Worker/`, `OpenAdm.Worker.Application/`, and `OpenAdm.Worker.Infra/`. PDF generation is isolated in `OpenAdm.Pdf/`. Tests are in `OpenAdm.Test/`, with builders under `OpenAdm.Test/Domain/Builder/`.

## Build, Test, and Development Commands
- `dotnet restore OpenAdm.sln`: restore NuGet packages.
- `dotnet build OpenAdm.sln`: compile all projects.
- `dotnet test OpenAdm.Test/OpenAdm.Test.csproj`: run the xUnit test suite.
- `dotnet test OpenAdm.Test/OpenAdm.Test.csproj --collect:"XPlat Code Coverage"`: run tests with coverlet coverage collection.
- `dotnet run --project OpenAdm.Api/OpenAdm.Api.csproj`: start the API locally after configuring `OpenAdm.Api/.env` from `.env.example`.
- `dotnet run --project OpenAdm.Worker/OpenAdm.Worker.csproj`: start the background worker.

## Coding Style & Naming Conventions
Projects target `net10.0` with nullable reference types and implicit usings enabled. Use four-space indentation and standard C# conventions: PascalCase for classes, methods, properties, DTOs, and enums; camelCase for locals and parameters; interfaces prefixed with `I`. Prefer `DateTime.UtcNow` over `DateTime.Now` when recording application timestamps. Keep Portuguese domain names consistent with existing files, for example `PedidoService`, `ConfiguracaoDeFreteController`, and `PaginacaoPedidoDto`.

## Testing Guidelines
Tests use xUnit with Moq, Bogus, ExpectedObjects, EF Core InMemory, and coverlet. Place new tests under `OpenAdm.Test/<Layer>/Test/` and name classes with the `*Test` suffix. Prefer descriptive Portuguese test method names that state expected behavior, such as `DeveGerarUmToken`. Use builders from `OpenAdm.Test/Domain/Builder/` for reusable entity setup.

## Commit & Pull Request Guidelines
Recent history uses short Portuguese messages such as `fix`, `ajuste`, and brief feature notes. Prefer concise imperative messages that name the area changed, for example `ajusta cobranca de pedido`. Pull requests should include a short summary, affected projects, test results, linked issue when applicable, and screenshots or sample requests for API behavior changes.

## Security & Configuration Tips
Do not commit real secrets. Keep local configuration in `OpenAdm.Api/.env` based on `.env.example`; required settings include JWT, PostgreSQL, Redis, Azure Storage, email, and external API values. Review migration changes in `OpenAdm.Data/Migrations/` before enabling `RODAR_MIGRATION=TRUE`.
