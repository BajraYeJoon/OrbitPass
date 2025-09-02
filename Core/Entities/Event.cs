using OrbitPass.Core.Entities;

namespace OrbitPass.Core.Entities
{
    public class Event: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        public int TicketsAvailable { get; set; }
        public decimal PriceInOrbitCoins { get; set; }
        public int OrganizerId { get; set; }
    }
}