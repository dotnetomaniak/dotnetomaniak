namespace Kigg.Infrastructure.EF.POCO
{
    public class UserScore
    {
        public long Id { get; set; }

        public System.Guid UserId { get; set; }

        public System.DateTime Timestamp { get; set; }

        public DomainObjects.UserAction ActionType { get; set; }

        public decimal Score { get; set; }

        public User User { get; set; }
    }
}