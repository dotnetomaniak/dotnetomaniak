namespace Kigg.DomainObjects
{
    using System;
    using System.Collections.Generic;

    public interface IQuiz
    {
        string Title { get; set; }
        List<IUser> Contestants { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}
