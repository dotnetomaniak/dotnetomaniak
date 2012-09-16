namespace Kigg.Service
{
    using Kigg.Core.DomainObjects;
    using System.Net;
    using System.Collections.Generic;
    using System.Linq;

    public class QuestionsService : IQuestionsService
    {
        public IEnumerable<IQuestion> GetQuestionsByTag(string tag)
        {
            Check.Argument.IsNotEmpty(tag, "tag");
            return GetQuestionsBySearch("[{0}]".FormatWith(tag));
        }

        public IEnumerable<IQuestion> GetQuestionsBySearch(string phrase)
        {
            Check.Argument.IsNotEmpty(phrase, "phrase");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://devpytania.pl/szukaj?q={0}".FormatWith(phrase));
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                return new List<IQuestion>().AsEnumerable();

            return new List<IQuestion>().AsEnumerable();
        }
    }
}
