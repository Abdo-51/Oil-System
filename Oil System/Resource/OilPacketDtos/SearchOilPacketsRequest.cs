using Oil_System.Contract.Pagination;
using Oil_System.Models;

namespace Oil_System.Resource.OilPacketDtos
{
    public class SearchOilPacketsRequest : PagedRequest<OilPacket>
    {
        public Guid BrandId { get; set; } = Guid.Empty;
        public Guid CategoryId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }

        public void AddOilPacketFilter()
        {
            if (BrandId != Guid.Empty)
            {
                FilterBy.Add(oilPacket => oilPacket.BrandId == BrandId);
            }
            if (CategoryId != Guid.Empty)
            {
                FilterBy.Add(oilPacket => oilPacket.CategoryId == CategoryId);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                FilterBy.Add(oilPacket => oilPacket.Name.Contains(Name));
            }
            if (Capacity > 0)
            {
                FilterBy.Add(oilPacket => oilPacket.Capacity == Capacity);
            }
        }
    }
}
