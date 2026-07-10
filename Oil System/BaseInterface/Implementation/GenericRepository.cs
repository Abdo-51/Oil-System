using Microsoft.EntityFrameworkCore;
using Oil_System.Contract.Pagination;
using Oil_System.Models;
using Oil_System.Repository.Data;
using System.Data.Common;

namespace Oil_System.BaseInterface.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        #endregion

        #region Constructors
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #endregion

        #region Methods
        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }
            _dbSet.Remove(entity);
        }

        public async Task<PagedResult<T>> GetAllAsync(PagedRequest<T> request)
        {
            var query = _dbSet.AsQueryable();

            // Apply filtering
            query = query.ApplyFiltering(request.FilterBy);
            //apply sorting
            query = query.ApplySorting(request.SortBy, request.SortDirection);

            return await query.ToPagedResultAsync(request.PageNumber, request.PageSize);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        #endregion

    }
}
