namespace Kigg.Infrastructure.EF.POCO
{
    public class UserScore: Entity
    {
        public System.Guid UserId { get; set; }

        public System.DateTime Timestamp { get; set; }

        public DomainObjects.UserAction ActionType { get; set; }

        public decimal Score { get; set; }

        public User User { get; set; }
    }
}