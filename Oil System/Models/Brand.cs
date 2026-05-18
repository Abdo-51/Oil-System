namespace Oil_System.Models
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
