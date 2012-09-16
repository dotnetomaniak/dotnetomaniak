namespace Kigg.Repository
{
    using System;
    using System.Collections.Generic;        
    using Kigg.DomainObjects;

    public interface IQuizRepository
    {
        IQuiz FindQuiz(DateTime currentDate);
        IQuiz FindQuiz(Guid id);
        bool RegisterUser(IUser user, IQuiz quiz);
    }
}
