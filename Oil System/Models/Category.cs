namespace Oil_System.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<OilPacket> Products { get; set; } = new List<OilPacket>();
    }
}
