using Kigg.DomainObjects;

namespace Kigg.LinqToSql.DomainObjects
{
    public partial class UserAchievement : IUserAchievement
    {

        IAchievement IUserAchievement.Achievement
        {
            get { return (IAchievement) this.Achievement; }
        }
    }
}
