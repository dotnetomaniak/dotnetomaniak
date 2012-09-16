namespace Kigg.DomainObjects
{
    using System;

    [Flags]
    public enum Roles
    {
        User = 0,
        Bot = 1,
        Moderator = 2,
        Administrator = 4
    }
}