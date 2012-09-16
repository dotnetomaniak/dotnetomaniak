using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kigg.DomainObjects;

namespace Kigg.Web
{
    public class QuizStatisticsViewData : BaseViewData
    {
        public List<IStory> TopStories { get; set;}
        public List<IUser> TopUsers { get; set; }
    }
}
