using Oil_System.Contract.Request.Authentcation;
using Oil_System.Models;
using Oil_System.Resource.Authentication;

namespace Oil_System.Helper.Mapping
{
    public partial class Profiling
    {
        private void MapResourceToDomain()
        {
            #region RegisterRequest To AppUser
            CreateMap<RegisterRequest, AppUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.BirthDate)));
            #endregion

            #region UpdateRequest To AppUser
            CreateMap<AppUserDto, AppUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                 .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            #endregion
        }
    }
}
