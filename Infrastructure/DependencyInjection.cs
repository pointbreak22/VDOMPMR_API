using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Настройка PostgreSQL с указанием сборки миграций
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)));

            // Регистрация реализаций инфраструктурных сервисов
            // Контекст как реализация интерфейса из Application
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Регистрация репозиториев
            services.AddScoped<Application.Common.Interfaces.IUserRepository, Infrastructure.Persistence.Repositories.UserRepository>();
            services.AddScoped<Application.Common.Interfaces.IProductRepository, Infrastructure.Persistence.Repositories.ProductRepository>();

            return services;
        }
    }
}
