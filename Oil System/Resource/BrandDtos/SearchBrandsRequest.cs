using Oil_System.Contract.Pagination;
using Oil_System.Models;

namespace Oil_System.Resource.BrandDtos
{
    public class SearchBrandsRequest : PagedRequest<Brand>
    {
        public string? Name { get; set; }

        public void addBrandFilters()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                FilterBy.Add(b => b.Name.Contains(Name));
            }
        }
    }
}
