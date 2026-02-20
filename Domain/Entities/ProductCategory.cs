namespace Domain.Entities
{
    public class ProductCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
        public List<ProductSubcategory> Subcategories { get; set; } = new();
    }
}
