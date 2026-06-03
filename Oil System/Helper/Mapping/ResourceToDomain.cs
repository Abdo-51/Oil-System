using Oil_System.Contract.Request.Authentcation;
using Oil_System.Models;

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
        }
    }
}
