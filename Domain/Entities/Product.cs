using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public enum GenderType
    {
        Male,
        Female,
        Child
    }

    public enum UnitType
    {
        Piece, // "шт."
        Pair   // "пар."
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;


        // Категория
        public Guid CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;

        // Подкатегория (может быть null)
        public Guid? SubcategoryId { get; set; }
        public ProductSubcategory? Subcategory { get; set; }

        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerPack { get; set; }
        public string Article { get; set; } = string.Empty;
        public int ItemsPerPack { get; set; }
        public UnitType Unit { get; set; }

        // Связь с параметрами
        public List<ProductParameter> Parameters { get; set; } = new();
    }
}
