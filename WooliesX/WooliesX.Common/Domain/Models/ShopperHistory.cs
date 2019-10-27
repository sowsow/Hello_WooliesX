using System.Collections.Generic;

namespace WooliesX.Common.Domain.Models
{
    public class ShopperHistory
    {
        public string CustomerId { get; set; }
        
        public List<Product> Products { get; set; }
    }
}
