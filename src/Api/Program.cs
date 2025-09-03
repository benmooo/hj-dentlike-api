using AspNetCoreRateLimit;
using Dentlike.Application.Interfaces;
using Dentlike.Application.Services;
using Dentlike.Domain.Interfaces;
using Dentlike.Infrastructure.Data;
using Dentlike.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    // .ReadFrom.Configuration(builder.Configuration)
    // .Enrich.FromLogContext()
    // .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    // .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/applog-.txt", rollingInterval: RollingInterval.Day) // each log file per day
    .CreateLogger();
builder.Logging.ClearProviders().AddSerilog();

// type-safe access to configuration settings
builder.Services.AddOptions();

// needed to store rate limit counters and ip rules
builder.Services.AddMemoryCache();

// load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

// inject counter and rules stores
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

// configuration (resolvers, counter key builders)
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// client IP resolvers use it
builder.Services.AddHttpContextAccessor();

// add health checks
builder.Services.AddHealthChecks();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// controller based api
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// swagger
builder.Services.AddEndpointsApiExplorer(); // minimal api
builder.Services.AddSwaggerGen();

// database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql =>
        {
            sql.MigrationsAssembly("Infrastructure"); // 迁移文件放到 Infrastructure
            sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); // 瞬断重试
            sql.CommandTimeout(30);
        }
    )
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// authentication and authorization
builder.Services.AddAuthentication().AddJwtBearer();
builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy("admin_policy", policy => policy.RequireRole("admin"))
    .AddPolicy("user_policy", policy => policy.RequireRole("user"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable IP Rate Limiting Middleware
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/ping", () => new { code = 0, msg = "pong" });

var apiGroup = app.MapGroup("api");

// auth group
var authGroup = apiGroup.MapGroup("auth");
var adminGroup = apiGroup.MapGroup("").RequireAuthorization("admin");

authGroup.MapPost("/login", () => Results.Ok(new { token = "fake-jwt-token" })).WithName("Login");
authGroup.MapPost("/register", () => Results.Ok()).WithName("Register");

// Configure custom health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();
app.Run();
