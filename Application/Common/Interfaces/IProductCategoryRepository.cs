using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces
{
    public interface IProductCategoryRepository
    {
        // Метод возвращает сущности с уже подгруженными связями
        Task<List<ProductCategory>> GetCategoriesWithDetailsAsync(CancellationToken ct);
    }
}
