using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Set<User>().AddAsync(user, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<User>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        }

        public async Task<List<User>> ListAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<User>().ToListAsync(cancellationToken);
        }

        public void Remove(User user)
        {
            _context.Set<User>().Remove(user);
        }

        public void Update(User user)
        {
            _context.Set<User>().Update(user);
        }
    }
}
