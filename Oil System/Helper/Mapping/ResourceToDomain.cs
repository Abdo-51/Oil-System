using Oil_System.Contract.Request.Authentcation;
using Oil_System.Models;
using Oil_System.Resource.Authentication;
using Oil_System.Resource.BrandDtos;
using Oil_System.Resource.CategoryDtos;
using Oil_System.Resource.OilPacketDtos;

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

            #region CreateCategoryRequest To Category
            CreateMap<CreateCategoryRequest, Category>();
            #endregion

            #region UpdateCategoryRequest To Category
            CreateMap<UpdateCategoryRequest, Category>()
                 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            #endregion

            #region CreateBrandRequest To Brand
            CreateMap<CreateBrandRequest, Brand>();
            #endregion

            #region UpdateBrandRequest To Brand
            CreateMap<UpdateBrandRequest, Brand>()
                 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            #endregion

            #region CreateOilPacketRequest To OilPacket
            CreateMap<CreateOilPacketsRequest, OilPacket>();
            #endregion

            #region UpdateOilPacketRequest To OilPacket
            CreateMap<UpdateOilPacketsRequest, OilPacket>()
                 .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            #endregion
        }
    }
}
