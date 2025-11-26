using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestManagement.Data.Repositories;

namespace TestManagement.API.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TestManagementDbContext _context;
        private readonly DbSet<T> _dbSet;

        public  GenericRepository(TestManagementDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entiry) => _dbSet.Update(entiry);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();
    }
}
