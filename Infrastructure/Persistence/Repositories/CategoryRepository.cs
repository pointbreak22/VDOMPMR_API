using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) => _context = context;

        public async Task<List<Category>> GetCategoriesWithDetailsAsync(CancellationToken ct)
        {
            return await _context.Categories
                   .AsNoTracking()       
                   .Include(c => c.Products)     
                   .ThenInclude(p => p.Parameters)     
                   .ToListAsync(ct);
        }
    }
}
