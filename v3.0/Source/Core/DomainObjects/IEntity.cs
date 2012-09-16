namespace Kigg.DomainObjects
{
    using System;

    public interface IEntity
    {
        Guid Id
        {
            get;
        }

        DateTime CreatedAt
        {
            get;
        }
    }
}