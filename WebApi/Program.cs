using Application;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.Authority = "https://localhost:5001"; // URL вашего Identity проекта
        options.Audience = "resource_api";
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            // Это заставляет .NET искать роли в Claim "role", который шлет OpenIddict
            RoleClaimType = "role",
            NameClaimType = "name"
        };
    });

// Add services to the container.

// Подключаем слои архитектуры
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// Identity server registration moved to Identity/DependencyInjection

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "VDOMPMR API";
        document.Info.Version = "v1";
        document.Info.Description = "API для работы с VDOMPMR";
        // 1. Инициализация компонентов
        if (document.Components == null)
        {
            document.Components = new OpenApiComponents();
        }

        // 2. Описываем схему
        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Введите JWT токен"
        };

        // ИСПРАВЛЕНИЕ: Приведение к интерфейсу IOpenApiSecurityScheme
        // Используем явное приведение словаря или просто добавляем через интерфейс
        if (document.Components.SecuritySchemes == null)
        {
            document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();
        }

        document.Components.SecuritySchemes["Bearer"] = (IOpenApiSecurityScheme)securityScheme;

        // 3. Глобальное требование авторизации
        if (document.Security == null)
        {
            document.Security = new List<OpenApiSecurityRequirement>();
        }

        var schemeReference = new OpenApiSecuritySchemeReference("Bearer", document);
        var requirement = new OpenApiSecurityRequirement();

        // Передаем пустой список строк для Scopes
        requirement.Add(schemeReference, new List<string>());

        document.Security.Add(requirement);

        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // <-- 2. Создает UI по адресу /scalar/v1
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
