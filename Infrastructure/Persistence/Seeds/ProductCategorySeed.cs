using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeds
{
    public static class CategorySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // ===== Уровень 1 (Root) =====
            var socksId = new Guid("5c29b657-af3f-4939-ae3b-92da7be157a3");
            var underwearId = new Guid("557e5a88-f46b-4129-a889-bc6007adf22c");
            var tightsId = new Guid("f171cc4f-ed02-42f8-8732-4e2b8741d08a");
            var towelsId = new Guid("91aeebc4-07ea-483a-bb96-54ec78d7993e");
            var glovesId = new Guid("9ca149ab-b4a6-4c1b-8d38-96f1cee244b3");
            var tshirtsId = new Guid("1f117b18-7973-40fe-b435-2302b7485441");
            var hatsId = new Guid("af93e152-d14d-470b-8c82-1e56eac15817");

            // ===== Уровень 2 =====
            var mensSocksId = new Guid("11111111-1111-1111-1111-111111111111");
            var womensSocksId = new Guid("22222222-2222-2222-2222-222222222222");
            var kidsSocksId = new Guid("33333333-3333-3333-3333-333333333333");

            var mensUnderwearId = new Guid("44444444-4444-4444-4444-444444444444");
            var womensUnderwearId = new Guid("55555555-5555-5555-5555-555555555555");

            var sportTshirtsId = new Guid("66666666-6666-6666-6666-666666666666");
            var casualTshirtsId = new Guid("77777777-7777-7777-7777-777777777777");

            var thermoSocksId = new Guid("88888888-8888-8888-8888-888888888888");
            var winterGlovesId = new Guid("99999999-9999-9999-9999-999999999999");

            // ===== Уровень 3 =====
            var runningId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var fitnessId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            modelBuilder.Entity<Category>().HasData(

                // Root
                new Category { Id = socksId, Name = "носки", ParentId = null },
                new Category { Id = underwearId, Name = "нижнее белье", ParentId = null },
                new Category { Id = tightsId, Name = "колготки", ParentId = null },
                new Category { Id = towelsId, Name = "полотенца", ParentId = null },
                new Category { Id = glovesId, Name = "перчатки", ParentId = null },
                new Category { Id = tshirtsId, Name = "футболки", ParentId = null },
                new Category { Id = hatsId, Name = "шапки", ParentId = null },

                // Level 2
                new Category { Id = mensSocksId, Name = "мужские носки", ParentId = socksId },
                new Category { Id = womensSocksId, Name = "женские носки", ParentId = socksId },
                new Category { Id = kidsSocksId, Name = "детские носки", ParentId = socksId },

                new Category { Id = mensUnderwearId, Name = "мужское белье", ParentId = underwearId },
                new Category { Id = womensUnderwearId, Name = "женское белье", ParentId = underwearId },

                new Category { Id = sportTshirtsId, Name = "спортивные футболки", ParentId = tshirtsId },
                new Category { Id = casualTshirtsId, Name = "повседневные футболки", ParentId = tshirtsId },

                new Category { Id = thermoSocksId, Name = "термо носки", ParentId = socksId },
                new Category { Id = winterGlovesId, Name = "зимние перчатки", ParentId = glovesId },

                // Level 3
                new Category { Id = runningId, Name = "для бега", ParentId = sportTshirtsId },
                new Category { Id = fitnessId, Name = "для фитнеса", ParentId = sportTshirtsId }
            );
        }
    }
}