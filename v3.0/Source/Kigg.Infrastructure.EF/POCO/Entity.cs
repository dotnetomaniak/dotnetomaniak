using System;
using Kigg.DomainObjects;

namespace Kigg.Infrastructure.EF.POCO
{
    public class Entity: IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}