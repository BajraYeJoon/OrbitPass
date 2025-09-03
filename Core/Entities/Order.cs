using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int EventId { get; set; }
    public Event Event { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal TotalOrbitCoins { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // Navigation Properties
    public ICollection<Ticket> Tickets { get; set; } = [];
}