using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Category> Categories => Set<Category>();

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

            // 🔹 Category self-reference (Parent-Children)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);


            // 🔹 Category -> Product (1:M)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                                                                 
         

            // Seed категорий
            CategorySeed.Seed(modelBuilder);

            // Глобальное преобразование имён в snake_case
            modelBuilder.UseSnakeCaseNames();
        }

        // Task<int> SaveChangesAsync уже есть в базовом DbContext, 
        // поэтому его достаточно просто реализовать неявно.
    }
}