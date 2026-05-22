using Oil_System.Repository.Data;

namespace Oil_System.BaseInterface.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        #endregion

        #region Constructors
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods
        public async Task<int> CommitChanges()
        {
            return await _context.SaveChangesAsync();
        }
        #endregion
    }
}
