using Dentlike.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();
app.UseCors("AllowAll");

var summaries = new[] { "Freezing", "Bracing" };

var adminGroup = app.MapGroup("/").RequireAuthorization("admin");

adminGroup
    .MapGet(
        "/weatherforecast",
        () =>
        {
            var forecast = Enumerable
                .Range(1, 5)
                .Select(index => new WeatherForecast(
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        }
    )
    .WithName("GetWeatherForecast");

app.MapGet("/ping", () => new { code = 0, msg = "pong" });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
