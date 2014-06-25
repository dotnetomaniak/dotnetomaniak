namespace Kigg.Web
{
	using DomainObjects;

	public abstract class BaseViewData
	{
		public string SiteTitle
		{
			get;
			set;
		}

		public string RootUrl
		{
			get;
			set;
		}

		public string MetaKeywords
		{
			get;
			set;
		}

		public string MetaDescription
		{
			get;
			set;
		}

		public IUser CurrentUser
		{
			get;
			set;
		}

		public bool IsCurrentUserAuthenticated
		{
			get;
			set;
		}

		public bool CanCurrentUserModerate
		{
			get
			{
				return (IsCurrentUserAuthenticated && CurrentUser.CanModerate());
			}
		}

		public int UpcomingStoriesCount
		{
			get;
			set;
		}

        public string FacebookAppId
        {
            get;
            set;
        }
	}
}