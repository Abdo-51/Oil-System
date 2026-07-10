using Oil_System.Models;
using Oil_System.Resource.Authentication;
using Oil_System.Resource.BrandDtos;
using Oil_System.Resource.CategoryDtos;
using Oil_System.Resource.OilPacketDtos;

namespace Oil_System.Helper.Mapping
{
    public partial class Profiling
    {
        private void MapDomainToResource()
        {
            #region Appuser to AppUserDto
            CreateMap<AppUser, AppUserDto>();
            #endregion

            #region AppUserDto to Appuser
            CreateMap<AppUserDto, AppUser>();

            #region Brand to BrandDto
            CreateMap<Brand, BrandDto>();
            #endregion

            #region Category to CategoryDto
            CreateMap<Category, CategoryDto>();
            #endregion
        }

    }
}
