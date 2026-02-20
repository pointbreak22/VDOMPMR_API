using Identity;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// 1. Настройка CORS для Angular
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
        "http://localhost:4200",
        "http://localhost:4000"   // ← SSR сервер
    )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Настройка Google (обязательно!)
builder.Services.AddAuthentication();
    //.AddGoogle(options =>
    //{
    //    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    //    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    //});

// DI-расширение для IdentityServer и OpenIddict
builder.Services.AddIdentityServer(builder.Configuration);

// External providers (Google) — уже настраивается внутри AddIdentityServer

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 3. Запуск Seed Data
await DbInitializer.SeedAsync(app.Services);

app.UseHttpsRedirection();

app.UseCors(); // Должен быть перед Auth
// 1. Обязательно для работы OpenIddict и Identity
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Генерирует JSON-файл спецификации
    app.MapScalarApiReference(); // Создает UI (по умолчанию доступен по /scalar/v1)
}

app.Run();

