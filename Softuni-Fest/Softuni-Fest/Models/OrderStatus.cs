namespace Softuni_Fest.Models
{
    public class OrderStatus
    {
        public OrderStatus() 
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; } = null!;
        public string Name { get; set; } = null!;
        public string NormalizedName { get; set; } = null!;

        public const string Completed = "Completed";
        public const string Pending = "Pending";
    }
}
