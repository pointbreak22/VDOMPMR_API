using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Infrastructure.Persistence
{
    public static class ModelBuilderExtensions
    {
        public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Таблица
                entity.SetTableName(ToSnakeCase(entity.GetTableName()));

                // Колонки
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.Name));
                }

                // FK
                foreach (var fk in entity.GetForeignKeys())
                {
                    fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
                }

                // PK
                var pk = entity.FindPrimaryKey();
                if (pk != null)
                    pk.SetName(ToSnakeCase(pk.GetName()));
            }
        }

        private static string ToSnakeCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            var sb = new StringBuilder();
            var prevLower = false;
            foreach (var c in name)
            {
                if (char.IsUpper(c))
                {
                    if (sb.Length > 0 && prevLower)
                        sb.Append('_');
                    sb.Append(char.ToLower(c, CultureInfo.InvariantCulture));
                    prevLower = false;
                }
                else
                {
                    sb.Append(c);
                    prevLower = true;
                }
            }
            return sb.ToString();
        }
    }
}
