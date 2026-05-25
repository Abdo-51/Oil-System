using Oil_System.Contract.Request.Authentcation;
using Oil_System.Models;

namespace Oil_System.Helper.Mapping
{
    public partial class Profiling
    {
        private void MapResourceToDomain()
        {
            CreateMap<RegisterRequest, AppUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
