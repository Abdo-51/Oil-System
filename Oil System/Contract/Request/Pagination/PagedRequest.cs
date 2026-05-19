using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Oil_System.Contract.Request.Pagination
{
    public class PagedRequest<T> where T : class
    {
        [JsonIgnore]
        public int MaxPageSize { get; set; } = 100;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "ASC";
        [JsonIgnore]
        public List<Expression<Func<T, bool>>> FilterBy { get; set; } = new List<Expression<Func<T, bool>>>();

        public void AddFilter(Expression<Func<T, bool>> filter)
        {
            FilterBy.Add(filter);
        }
    }
}
