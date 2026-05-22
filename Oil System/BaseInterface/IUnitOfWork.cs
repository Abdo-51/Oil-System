namespace Oil_System.BaseInterface
{
    public interface IUnitOfWork
    {
        Task<int> CommitChanges();
    }
}
