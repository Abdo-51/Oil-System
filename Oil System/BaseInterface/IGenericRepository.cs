using Oil_System.Contract.Pagination;

namespace Oil_System.BaseInterface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PagedResult<T>> GetAllAsync(PagedRequest<T> request);
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
