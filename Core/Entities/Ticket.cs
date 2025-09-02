using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities;

public class Ticket : BaseEntity
{
    public int OrderId { get; set; } // Foreign key to Order
    public int EventId { get; set; } // Foreign key to Event
    public int UserId { get; set; } // Foreign key to User
    public TicketType Type { get; set; } = TicketType.Standard;
    public string QRCode { get; set; } = string.Empty;
    public bool IsUsed { get; set; } = false;
}