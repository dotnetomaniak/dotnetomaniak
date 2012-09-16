namespace Kigg.Web
{
    using System.Web.Mvc;
    using Kigg.Repository;
    using System;
    using Kigg.DomainObjects;

    public class QuizController : BaseController
    {

        [Compress]
        public ActionResult List()
        {
            var quizViewData = new QuizViewData()
                                   {
                                       Title = "Jestem dotnetomaniakiem",
                                       StartDate = new DateTime(2009, 10, 15),
                                       EndDate = new DateTime(2010, 11, 12)
                                   };
            return View("QuizView",quizViewData);
        }        
    }
}
