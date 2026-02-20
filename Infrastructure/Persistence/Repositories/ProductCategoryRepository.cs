using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<ProductCategory>> GetCategoriesWithDetailsAsync(CancellationToken ct)
        {
            return await _context.ProductCategories
                .AsNoTracking()
                .Include(c => c.Products)
                    .ThenInclude(p => p.Parameters)
                .Include(c => c.Subcategories)
                .ToListAsync(ct);
        }
    }
}
