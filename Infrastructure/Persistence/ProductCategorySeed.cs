using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public static class ProductCategorySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { Id = new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3"), Name = "носки" },
                new ProductCategory { Id = new Guid("557e5a88-f46b-4129-a889-bc6007adf22c"), Name = "нижнее белье" },
                new ProductCategory { Id = new Guid("f171cc4f-ed02-42f8-8732-4e2b8741d08a"), Name = "колготки" },
                new ProductCategory { Id = new Guid("91aeebc4-07ea-483a-bb96-54ec78d7993e"), Name = "полотенца" },
                new ProductCategory { Id = new Guid("9ca149ab-b4a6-4c1b-8d38-96f1cee244b3"), Name = "перчатки" },
                new ProductCategory { Id = new Guid("1f117b18-7973-40fe-b435-2302b7485441"), Name = "футболки" },
                new ProductCategory { Id = new Guid("af93e152-d14d-470b-8c82-1e56eac15817"), Name = "шапки" }
            );
        }
    }
}
