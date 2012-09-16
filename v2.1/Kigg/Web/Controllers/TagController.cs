namespace Kigg.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;

    using DomainObjects;
    using Infrastructure;
    using Repository;

    public class TagController : BaseController
    {
        private readonly ITagRepository _tagRepository;

        public TagController(ITagRepository tagRepository)
        {
            Check.Argument.IsNotNull(tagRepository, "tagRepository");

            _tagRepository = tagRepository;
        }

        [OutputCache(CacheProfile = "JsonTagCache"), Compress]
        public ActionResult SuggestTags(string q, int? limit, string client)
        {
            ICollection<ITag> tags = null;

            q = q.NullSafe();

            if (!string.IsNullOrEmpty(q))
            {
                if (limit == null)
                {
                    limit = 10;
                }

                limit = (limit < 10) ? 10 : ((limit > 10) ? 10 : limit);

                try
                {
                    tags = _tagRepository.FindMatching(q, limit.Value);
                }
                catch (Exception e)
                {
                    Log.Exception(e);
                }
            }

            tags = tags ?? new List<ITag>();
            string[] tagNames = tags.Select(t => t.Name).ToArray();

            bool isClientBrowserItSelf = (!string.IsNullOrEmpty(client)) &&
                                         (string.Compare(client, "browser", StringComparison.OrdinalIgnoreCase) == 0);

            if (!isClientBrowserItSelf)
            {
                return Json(tagNames);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string content = "[{0}, {1}]".FormatWith(serializer.Serialize(q), serializer.Serialize(tagNames));

            return Content(content, "application/json");
        }

        public ActionResult Tabs()
        {
            ICollection<ITag> userTags = IsCurrentUserAuthenticated ? CurrentUser.Tags.ToList() : new List<ITag>();

            TagTabsViewData viewData = new TagTabsViewData
                                           {
                                               PopularTags = _tagRepository.FindByUsage(Settings.TopTags),
                                               UserTags = userTags
                                           };

            return View(viewData);
        }
    }
}