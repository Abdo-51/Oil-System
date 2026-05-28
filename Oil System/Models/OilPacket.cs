using System.ComponentModel.DataAnnotations.Schema;

namespace Oil_System.Models
{
    public class OilPacket : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public int Capacity { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal BuyingPrice { get; set; }
        //Relationship one to many between brand and oil
        [ForeignKey("Brand")]
        public Guid BrandId { get; set; }
        public virtual Brand? Brand { get; set; }

        public Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
