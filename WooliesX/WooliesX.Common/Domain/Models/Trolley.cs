namespace WooliesX.Common.Domain.Models
{
    public class Trolley
    {
        public Product[] Products { get; set; }

        public Special[] Specials { get; set; }

        public ProductQuantity[] Quantities { get; set; }
    }
}
