using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities
{
    public class User: BaseEntity
    {
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Attendee;
    public decimal OrbitCoins { get; set; } = 200; // Starting balance
    
    // // Navigation properties
    // public ICollection<Event> Events { get; set; } = new List<Event>(); // For Organizers
    // public ICollection<Order> Orders { get; set; } = new List<Order>();
    // public ICollection<OrbitCoinTransaction> Transactions { get; set; } = new List<OrbitCoinTransaction>();
}
}