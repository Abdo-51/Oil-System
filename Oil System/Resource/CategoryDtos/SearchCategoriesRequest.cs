using Oil_System.Contract.Pagination;
using Oil_System.Models;

namespace Oil_System.Resource.CategoryDtos
{
    public class SearchCategoriesRequest : PagedRequest<Category>
    {
        public string? Name { get; set; }

        public void AddCategoryFilter()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                FilterBy.Add(x => x.Name.Contains(Name));
            }
        }
    }
}
