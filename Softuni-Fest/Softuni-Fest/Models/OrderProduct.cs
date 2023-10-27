using System;
using System.Collections.Generic;

namespace Softuni_Fest
{
    public partial class OrderProduct
    {
        public OrderProduct() 
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public long Quantity { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
