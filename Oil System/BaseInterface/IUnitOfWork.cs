using Oil_System.Repository;

namespace Oil_System.BaseInterface
{
    public interface IUnitOfWork
    {
        Task<int> CommitChanges();
        public IBrandRepository brandRepository { get; }
        public ICategoryRepository categoryRepository { get; }
        Task<bool> CommitChanges();
    }
}
