# LiSoft Backend - Arquitetura em Camadas

## 📁 Estrutura de Projetos

```
backend/
├── LiSoft.Api/              # Camada de Apresentação (API REST)
│   ├── Controllers/         # Endpoints da API
│   ├── Program.cs          # Configuração e startup
│   └── appsettings.json    # Configurações da aplicação
│
├── LiSoft.Application/      # Camada de Aplicação (Lógica de Negócio)
│   ├── Models/             # DTOs e Entidades
│   │   ├── Contact.cs
│   │   └── ContactDto.cs
│   └── Services/           # Serviços de negócio
│       ├── IContactService.cs
│       └── ContactService.cs
│
└── LiSoft.MongoDB/          # Camada de Infraestrutura (MongoDB)
    ├── Configuration/      # Configurações do MongoDB
    │   └── MongoDbSettings.cs
    └── Services/          # Serviço de acesso ao MongoDB
        └── MongoDbService.cs (Singleton)
```

## 🎯 Responsabilidades de Cada Camada

### 1. **LiSoft.Api** (Apresentação)
- **Responsabilidade**: Expor endpoints HTTP/REST
- **Dependências**: LiSoft.Application, LiSoft.MongoDB
- **Contém**:
  - Controllers (rotas da API)
  - Configuração de CORS, Swagger, etc.
  - Middleware e filtros

### 2. **LiSoft.Application** (Lógica de Negócio)
- **Responsabilidade**: Implementar regras de negócio
- **Dependências**: LiSoft.MongoDB
- **Contém**:
  - Interfaces de serviços
  - Implementações de serviços
  - Models e DTOs
  - Validações de negócio

### 3. **LiSoft.MongoDB** (Infraestrutura)
- **Responsabilidade**: Gerenciar acesso ao MongoDB
- **Dependências**: MongoDB.Driver
- **Contém**:
  - Configurações do MongoDB
  - Serviço Singleton de conexão
  - Abstrações para acesso a dados

## 🔄 Fluxo de Dependências

```
LiSoft.Api
    ↓ (depende de)
LiSoft.Application
    ↓ (depende de)
LiSoft.MongoDB
    ↓ (depende de)
MongoDB.Driver (NuGet)
```

## ✅ Vantagens desta Arquitetura

### 1. **Separação de Responsabilidades (SoC)**
- Cada projeto tem uma responsabilidade clara
- Facilita manutenção e evolução do código

### 2. **Escalabilidade**
- Fácil adicionar novos bancos de dados (ex: SQL Server, Redis)
- Fácil adicionar novos serviços de negócio
- Pode escalar horizontalmente cada camada independentemente

### 3. **Testabilidade**
- Cada camada pode ser testada isoladamente
- Fácil criar mocks das dependências
- Testes unitários mais simples

### 4. **Reutilização**
- `LiSoft.Application` pode ser usado por outras APIs (ex: gRPC, GraphQL)
- `LiSoft.MongoDB` pode ser usado por outros projetos

### 5. **Manutenibilidade**
- Mudanças no MongoDB não afetam a lógica de negócio
- Mudanças na lógica de negócio não afetam a API
- Código mais limpo e organizado

## 🚀 Como Adicionar Novos Recursos

### Adicionar um Novo Serviço

1. **Criar Model em `LiSoft.Application/Models/`**
```csharp
public class Product
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

2. **Criar Interface em `LiSoft.Application/Services/`**
```csharp
public interface IProductService
{
    Task<Product> CreateAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
}
```

3. **Implementar Serviço em `LiSoft.Application/Services/`**
```csharp
public class ProductService : IProductService
{
    private readonly IMongoCollection<Product> _products;

    public ProductService(IMongoDbService mongoDbService)
    {
        _products = mongoDbService.GetCollection<Product>("products");
    }

    // Implementar métodos...
}
```

4. **Registrar no `Program.cs` da API**
```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

5. **Criar Controller em `LiSoft.Api/Controllers/`**
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // Implementar endpoints...
}
```

### Adicionar um Novo Banco de Dados

1. **Criar novo projeto** (ex: `LiSoft.SqlServer`)
2. **Criar serviço de conexão** similar ao `MongoDbService`
3. **Registrar no `Program.cs`**
4. **Injetar onde necessário**

## 🔧 Configuração

### appsettings.json
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb+srv://user:pass@cluster.mongodb.net/",
    "DatabaseName": "system_lisoft",
    "Collections": {
      "Contacts": "contacts",
      "Products": "products"
    }
  }
}
```

### appsettings.Development.json
```json
{
  "MongoDbSettings": {
    "Collections": {
      "Contacts": "contacts_dev"
    }
  }
}
```

## 📦 Pacotes NuGet Utilizados

### LiSoft.Api
- `Swashbuckle.AspNetCore` (Swagger/OpenAPI)
- `MongoDB.Driver` (compatibilidade)

### LiSoft.Application
- `MongoDB.Driver` (para usar IMongoCollection)
- `Microsoft.Extensions.Logging.Abstractions`
- `Microsoft.Extensions.Options`

### LiSoft.MongoDB
- `MongoDB.Driver`
- `Microsoft.Extensions.Logging.Abstractions`
- `Microsoft.Extensions.Options`

## 🏃 Como Executar

```bash
# Navegar para o diretório da API
cd backend/LiSoft.Api

# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run
```

A API estará disponível em: `http://localhost:5000`

Swagger UI: `http://localhost:5000/swagger`

## 📝 Sobre o node_modules

O diretório `node_modules` do frontend **NÃO deve ser versionado** no Git:
- ❌ Ocupa muito espaço (centenas de MB)
- ❌ Causa conflitos entre sistemas operacionais
- ✅ Pode ser recriado com `npm install`
- ✅ Já está no `.gitignore`

## 🎓 Padrões Utilizados

- **Singleton Pattern**: MongoDbService (uma única instância de conexão)
- **Dependency Injection**: Todas as dependências injetadas via construtor
- **Repository Pattern**: Services abstraem o acesso a dados
- **DTO Pattern**: ContactDto separa modelo de transporte do modelo de domínio
- **Layered Architecture**: Separação clara de responsabilidades

## 🔐 Segurança

- Senhas mascaradas nos logs
- Configurações sensíveis em `appsettings.json` (não versionado em produção)
- Use variáveis de ambiente para produção
- CORS configurado para localhost:3000 (ajustar para produção)

## 📚 Próximos Passos

- [ ] Adicionar autenticação/autorização (JWT)
- [ ] Implementar paginação nos endpoints GET
- [ ] Adicionar validações com FluentValidation
- [ ] Implementar testes unitários
- [ ] Adicionar cache (Redis)
- [ ] Implementar logging estruturado (Serilog)
- [ ] Adicionar health checks
- [ ] Configurar CI/CD
