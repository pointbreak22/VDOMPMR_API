using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Persistence.Seeds;

namespace Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet доступны инфраструктуре через Set<TEntity>()
        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductParameter> ProductParameters => Set<ProductParameter>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Здесь настраиваются связи, индексы и Fluent API
            // Важно: в DDD/Clean Arch настройки лучше делать здесь, 
            // чтобы не загрязнять Domain Entity атрибутами [Key], [Required]
            base.OnModelCreating(modelBuilder);

            // Пример: modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // Связь Product -> ProductParameter (один ко многим)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Parameters)
                .WithOne(pp => pp.Product)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь ProductCategory -> Product (один ко многим)
            modelBuilder.Entity<ProductCategory>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь ProductCategory -> ProductSubcategory (один ко многим)
            modelBuilder.Entity<ProductCategory>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasMany(c => c.Subcategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSubcategory>()
                .HasMany(sc => sc.Products)
                .WithOne(p => p.Subcategory)
                .HasForeignKey(p => p.SubcategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed категорий
            ProductCategorySeed.Seed(modelBuilder);

            // Глобальное преобразование имён в snake_case
            modelBuilder.UseSnakeCaseNames();
        }

        // Task<int> SaveChangesAsync уже есть в базовом DbContext, 
        // поэтому его достаточно просто реализовать неявно.
    }
}