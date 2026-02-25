using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ICategoryRepository
    {
        // Метод возвращает сущности с уже подгруженными связями
        Task<List<Category>> GetCategoriesWithDetailsAsync(CancellationToken ct);
    }
}
