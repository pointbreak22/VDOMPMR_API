using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IProductCategoryRepository
    {
        // Метод возвращает сущности с уже подгруженными связями
        Task<List<ProductCategory>> GetCategoriesWithDetailsAsync(CancellationToken ct);
    }
}
