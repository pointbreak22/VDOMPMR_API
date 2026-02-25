namespace Domain.Entities
{
  

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
        public Category Category { get; set; } = null!;

                                                                  
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
