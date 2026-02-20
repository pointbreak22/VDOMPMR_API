using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> ListAsync(CancellationToken cancellationToken);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        void Update(Product product);
        void Remove(Product product);

        Task<(List<Product> Items, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, CancellationToken ct);
    }
}
