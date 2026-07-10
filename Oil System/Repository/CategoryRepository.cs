using Oil_System.BaseInterface;
using Oil_System.BaseInterface.Implementation;
using Oil_System.Models;
using Oil_System.Repository.Data;

namespace Oil_System.Repository
{
    #region Interfaces
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }
    #endregion

    #region Implementations
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    #endregion
}
