using OrbitPass.Core.Enums;

namespace OrbitPass.Core.Entities
{
    public class OrbitCoinTransaction : BaseEntity
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public TransactionTypes Type { get; set; }
        public int? RelatedOrderId { get; set; }
        public int? RelatedEventId { get; set; }
    }
}