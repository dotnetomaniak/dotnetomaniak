namespace Kigg.Core.DomainObjects
{
    using System;

    public interface IQuestion
    {
        int Votes { get; set; }
        string Title { get; set; }
        Uri Link { get; set; }
    }
}
