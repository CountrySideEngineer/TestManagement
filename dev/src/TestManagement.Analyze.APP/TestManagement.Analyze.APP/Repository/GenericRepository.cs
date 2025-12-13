using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestManagement.Analyze.APP.DBContext;

namespace TestManagement.Analyze.APP.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly RequestDbContext _context;

        private readonly DbSet<T> _dbSet;

        public GenericRepository(RequestDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T? GetById(int id) => _dbSet.Find(id);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate).ToList();

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity) => _dbSet.Remove(entity);

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();

        }
    }
}
