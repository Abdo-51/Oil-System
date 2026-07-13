using Oil_System.BaseInterface;
using Oil_System.BaseInterface.Implementation;
using Oil_System.Models;
using Oil_System.Repository.Data;

namespace Oil_System.Repository
{
    #region Interfaces
    public interface IOilPacketsRepository : IGenericRepository<OilPacket>
    {

    }
    #endregion

    #region Implementations
    public class OilPacketsRepository : GenericRepository<OilPacket>, IOilPacketsRepository
    {

        public OilPacketsRepository(ApplicationDbContext context) : base(context)
        {

        }


    }
    #endregion
}
