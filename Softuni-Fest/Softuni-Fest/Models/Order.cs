using System;
using System.Collections.Generic;

namespace Softuni_Fest
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
