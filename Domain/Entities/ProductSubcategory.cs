using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ProductSubcategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // FK to Category
        public Guid CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;

        public List<Product> Products { get; set; } = new();
    }
}
