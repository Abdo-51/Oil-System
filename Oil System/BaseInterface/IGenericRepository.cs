using Oil_System.Contract.Pagination;
using Oil_System.Models;

namespace Oil_System.BaseInterface
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<PagedResult<T>> GetAllAsync(PagedRequest<T> request);
        Task<T?> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
