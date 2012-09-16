using System;

namespace Kigg.DomainObjects
{
    public interface IUserAchievement
    {
        Guid UserId { get; }
        DateTime DateAchieved { get; }
        IAchievement Achievement { get; }
        bool Displayed { get; }
    }
}