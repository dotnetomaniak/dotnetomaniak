using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kigg.Core.DomainObjects;
using System.Collections.ObjectModel;

namespace Kigg.Service
{
    public interface IQuestionsService
    {
        IEnumerable<IQuestion> GetQuestionsByTag(string tag);
        IEnumerable<IQuestion> GetQuestionsBySearch(string phrase);
    }
}
