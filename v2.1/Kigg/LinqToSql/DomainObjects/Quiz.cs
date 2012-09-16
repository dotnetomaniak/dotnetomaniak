namespace Kigg.DomainObjects
{
    using System;
    using System.Collections.Generic;

    public partial class Quiz : IQuiz
    {

        public List<IUser> Contestants
        {
            get;
            set;
        }
    }
}
