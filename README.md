# hj-dentlike-api

controller based api for dentlike based on dotnet core

### Project setup

```sh
src/
  ├── Api/                                # Web API 层
  │    ├── Api.csproj
  │    ├── Program.cs
  │    ├── Startup.cs
  │    ├── Controllers/
  │    │     ├── WeatherForecastController.cs
  │    │     ├── UsersController.cs
  │    │     └── ProductsController.cs
  │    ├── Filters/
  │    │     └── GlobalExceptionFilter.cs
  │    ├── Middlewares/
  │    │     └── ErrorHandlingMiddleware.cs
  │    ├── DTOs/
  │    │     ├── UserDto.cs
  │    │     └── ProductDto.cs
  │    └── appsettings.json
  │
  ├── Application/                         # 应用层
  │    ├── Application.csproj
  │    ├── Interfaces/
  │    │     ├── IUserService.cs
  │    │     └── IProductService.cs
  │    ├── Services/
  │    │     ├── UserService.cs
  │    │     └── ProductService.cs
  │    └── DTOs/
  │          ├── CreateUserRequest.cs
  │          └── ProductResponse.cs
  │
  ├── Domain/                              # 领域层
  │    ├── Domain.csproj
  │    ├── Entities/
  │    │     ├── User.cs
  │    │     └── Product.cs
  │    ├── ValueObjects/
  │    │     └── Email.cs
  │    ├── Enums/
  │    │     └── UserRole.cs
  │    └── Events/
  │          └── UserCreatedEvent.cs
  │
  ├── Infrastructure/                      # 基础设施层
  │    ├── Infrastructure.csproj
  │    ├── Data/
  │    │     ├── AppDbContext.cs
  │    │     └── Migrations/
  │    ├── Repositories/
  │    │     ├── UserRepository.cs
  │    │     └── ProductRepository.cs
  │    ├── Configurations/
  │    │     ├── UserConfiguration.cs
  │    │     └── ProductConfiguration.cs
  │    └── Services/
  │          └── EmailService.cs
  │
  └── Tests/                               # 测试层
       ├── Api.Tests/
       │     └── UsersControllerTests.cs
       ├── Application.Tests/
       │     └── UserServiceTests.cs
       └── Infrastructure.Tests/
             └── UserRepositoryTests.cs
```

### Tech stack

1. Web API 基础

- ASP.NET Core Web API (Controller) → 核心框架
- Routing → MapControllers()，支持 RESTful 风格
- Model Binding & Validation → 内置模型绑定 + DataAnnotations 验证

2. 数据库 & ORM

- Microsoft SQL Server → 数据库
- Entity Framework Core (EF Core) → ORM 框架
- EF Core Migrations → 数据库迁移工具
- Fluent API 配置 → 更灵活的实体映射 -

3. API 文档 & 调试

- Swagger / Swashbuckle.AspNetCore → 自动生成 API 文档 & UI
- NSwag（可选）→ 如果需要生成客户端 SDK

4. 日志 & 监控

- Serilog → 高级日志框架（支持写入 SQL Server、ElasticSearch、文件等）
- Serilog.Sinks.Console / File / Seq → 日志输出通道
- AppInsights (Azure) 或 Prometheus + Grafana → 应用监控

5. 身份验证 & 授权

- ASP.NET Core Identity → 用户身份管理
- JWT (Json Web Token) → Token 认证
- IdentityServer4 / Duende IdentityServer（如果需要 OAuth2 / OpenID Connect）

6. 配置 & 缓存

- IOptions Pattern → 配置管理
- MemoryCache → 内存缓存
- Distributed Cache (Redis) → 分布式缓存

7. 错误处理 & 异常

- 全局异常中间件 → 统一处理错误
- ProblemDetails → 标准化错误响应

8. 性能优化

- Response Caching → 响应缓存
- Rate Limiting (AspNetCoreRateLimit) → API 限流
- Output Caching Middleware (.NET 7+)

9. 单元测试 & 集成测试

- xUnit / NUnit → 单元测试框架
- Moq → Mock 工具
- FluentAssertions → 更友好的断言语法
- Microsoft.AspNetCore.Mvc.Testing → 集成测试支持

10. CI/CD & 部署

- Docker + Docker Compose → 容器化
- GitHub Actions / Azure DevOps Pipelines → 持续集成
- Kubernetes（可选，大型项目）

### Migration naming convention

| 变更类型       | 描述                            | 迁移名称示例                         |
| -------------- | ------------------------------- | ------------------------------------ |
| 增加长度限制   | 修改某表某列的最大长度          | Alter{Table}{Column}Length           |
| 修改列数据类型 | 将某表某列的数据类型从 A 改为 B | Change{Table}{Column}TypeTo{NewType} |
| 重命名列       | 重命名某表某列                  | Rename{Table}{Column}To{NewName}     |
| 新增列         | 在某表中新增某列                | Add{Column}To{Table}                 |
| 删除列         | 从某表中删除某列                | Remove{Column}From{Table}            |
| 新增表         | 创建新表                        | Create{Table}                        |
| 删除表         | 删除表                          | Drop{Table}                          |
| 创建索引       | 在某表的某列上创建索引          | CreateIndexOn{Table}{Column}         |
| 删除索引       | 删除某表上的某个索引            | DropIndexOn{Table}{Column}           |
