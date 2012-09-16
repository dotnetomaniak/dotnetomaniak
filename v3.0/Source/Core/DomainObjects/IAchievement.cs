using System;

namespace Kigg.DomainObjects
{
    public interface IAchievement : IUniqueNameEntity
    {
        string Name { get; }

        string Description { get; }

        Types AchievementType { get; }
    }
}
