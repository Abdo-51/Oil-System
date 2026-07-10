using Oil_System.Repository;

namespace Oil_System.BaseInterface
{
    public interface IUnitOfWork
    {
        Task<int> CommitChanges();
        public IBrandRepository brandRepository { get; }
        Task<bool> CommitChanges();
    }
}
