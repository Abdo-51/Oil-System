using Oil_System.BaseInterface;
using Oil_System.BaseInterface.Implementation;
using Oil_System.Models;
using Oil_System.Repository.Data;

namespace Oil_System.Repository
{
    #region Interfaces
    public interface IBrandRepository : IGenericRepository<Brand>
    {
    }
    #endregion

    #region Implementations
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    #endregion
}
