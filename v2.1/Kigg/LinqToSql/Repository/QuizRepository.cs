namespace Kigg.Repository.LinqToSql
{
    using System;
    using DomainObjects;
    using Repository;

    public class QuizRepository : BaseRepository<IQuiz, Quiz>, IQuizRepository
    {
        public QuizRepository(IDatabase database)
            : base(database)
        {

        }

        public QuizRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }

        public IQuiz FindQuiz(DateTime currentDate)
        {
            return new Quiz();
        }

        public bool RegisterUser(IUser user, IQuiz quiz)
        {
            return true;
        }
    }
}
