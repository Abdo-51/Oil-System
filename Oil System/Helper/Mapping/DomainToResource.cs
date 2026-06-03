using Oil_System.Models;
using Oil_System.Resource.Authentication;

namespace Oil_System.Helper.Mapping
{
    public partial class Profiling
    {
        private void MapDomainToResource()
        {
            #region Appuser to AppUserDto
            CreateMap<AppUser, AppUserDto>();
            #endregion
        }

    }
}
