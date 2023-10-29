using System;
using System.Collections.Generic;

namespace Softuni_Fest
{
    public partial class Product
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public string Id { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; } = null;
        public long ProductPrice { get; set; }
        public string VendorId { get; set; } = null!;
        public int QuantityInStock { get; set; }

        public virtual User Vendor { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
