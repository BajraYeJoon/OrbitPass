namespace OrbitPass.Core.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}