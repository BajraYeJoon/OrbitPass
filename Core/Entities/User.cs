using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Attendee;
    public decimal OrbitCoins { get; set; } = 200;

    // Navigation Properties
    public ICollection<Event> Events { get; set; } = [];  // C# 12 collection expression
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<OrbitCoinTransaction> Transactions { get; set; } = [];
}