using System;

namespace Kigg.DataServices.Data
{
    public class ShoutedStory
    {
        public string Title
        {
            get;
            internal set;
        }

        public string Url
        {
            get;
            internal set;
        }

        public string Description
        {
            get;
            internal set;
        }

        public string Category
        {
            get; 
            internal set;
        }

        public DateTime CreatedOn
        {
            get;
            internal set;
        }

        public DateTime? PublishedOn
        {
            get;
            internal set;
        }

        public int ShoutCount
        {
            get;
            internal set;
        }

        public int CommentCount
        {
            get; 
            internal set;
        }
        
        public string Tags
        {
            get;
            internal set;
        }
        
        public ShoutUser User
        {
            get; 
            internal set;
        }
    }
}
