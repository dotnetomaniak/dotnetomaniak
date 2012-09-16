namespace Kigg.Web
{
    using System;

    public class QuizViewData : BaseViewData
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }
}
