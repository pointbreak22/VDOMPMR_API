using System;

namespace Domain.Entities
{
    public enum ParameterType
    {
        Age,
        Size,
        Number
        // Можно добавить другие типы
    }

    public class ProductParameter
    {
        public Guid Id { get; set; }
        public ParameterType Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public int Stock { get; set; }

        // Внешний ключ на товар
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
