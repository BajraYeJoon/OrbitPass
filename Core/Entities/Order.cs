using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities;

public class Order: BaseEntity
{
    public int UserId { get; set; }
    public int EventId { get; set; } //FOREIGN KEY TO EVENT
    public int Quantity { get; set; }
    public decimal TotalOrbitCoins { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}