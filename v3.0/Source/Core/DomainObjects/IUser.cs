using System.Linq;

namespace Kigg.DomainObjects
{
    using System;

    public interface IUser : IEntity, ITagContainer
    {
        string UserName
        {
            get;
        }

        string Password
        {
            get;
        }

        string Email
        {
            get;
        }

        bool IsActive
        {
            get;
            set;
        }

        bool IsLockedOut
        {
            get;
        }

        Roles Role
        {
            get;
            set;
        }

        DateTime LastActivityAt
        {
            get;
            set;
        }

        decimal CurrentScore
        {
            get;
        }

        PagedResult<IUserAchievement> Achievements { get; }
        PagedResult<IUserAchievement> NewAchievements { get; }

        void ChangeEmail(string email);

        void ChangePassword(string oldPassword, string newPassword);

        string ResetPassword();

        void Lock();

        void Unlock();

        decimal GetScoreBetween(DateTime startTimestamp, DateTime endTimestamp);

        void IncreaseScoreBy(decimal score, UserAction reason);

        void DecreaseScoreBy(decimal score, UserAction reason);

        void MarkAchievementsAsDisplayed();
    }
}