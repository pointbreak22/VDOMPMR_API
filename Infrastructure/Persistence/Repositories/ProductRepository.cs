using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Set<Product>().AddAsync(product, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<Product>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public async Task<List<Product>> ListAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<Product>().ToListAsync(cancellationToken);
        }

        public void Remove(Product product)
        {
            _context.Set<Product>().Remove(product);
        }

        public void Update(Product product)
        {
            _context.Set<Product>().Update(product);
        }

        public async Task<(List<Product> Items, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p => p.Parameters);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(p => p.Name) // ОБЯЗАТЕЛЬНО добавьте сортировку
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }
    }
}
