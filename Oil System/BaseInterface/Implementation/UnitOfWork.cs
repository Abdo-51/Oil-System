using Oil_System.Repository;
using Oil_System.Repository.Data;

namespace Oil_System.BaseInterface.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly ApplicationDbContext _context;
        public IBrandRepository brandRepository => new BrandRepository(_context);
        public ICategoryRepository categoryRepository => new CategoryRepository(_context);

        #endregion

        #region Constructors
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods
        public async Task<bool> CommitChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
