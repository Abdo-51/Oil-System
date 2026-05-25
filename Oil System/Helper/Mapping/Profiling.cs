using AutoMapper;
using Oil_System.Contract.Pagination;

namespace Oil_System.Helper.Mapping
{
    public partial class Profiling : Profile
    {
        public Profiling()
        {
            // Add your mappings here
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            MapResourceToDomain();
            MapDomainToResource();
        }
    }
}
